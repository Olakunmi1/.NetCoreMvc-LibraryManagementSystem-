using LibraryData;
using LibraryData.Model;
using LibraryManagement.ViewModels.Branch;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.Controllers
{
    public class BranchController : Controller
    {
        private readonly ILibraryBranch _branch;

        //this give acces to the physical path of WWWroot, so we have acces to images folder
        private readonly IWebHostEnvironment _hostingEnvironment;


        //Inorder to have access to the root of WWWroot we inject IwebHosting interface 
        public BranchController(ILibraryBranch branch, IWebHostEnvironment hostingEnvironment)
        {
            _branch = branch;
            _hostingEnvironment = hostingEnvironment;
        }

        //Serves the NewBranch page
        [HttpGet]
        public IActionResult NewBranch()
        {

            return View();
        }

        //Responds tho post request
        [HttpPost]
        public IActionResult NewBranch(NewBranchViewModel model)
        {
            if (ModelState.IsValid)
            {
                string UniqueFilePath = UploadFileMethod(model);
                var newBranch = new LibraryBranch
                {
                    Id = model.Id,
                    Name = model.Name,
                    Address = model.Address,
                    Telephone = model.Telephone,
                    Description = model.Description,
                    OpenDate = model.OpenDate,
                    ImageUrl = UniqueFilePath
                };
                _branch.Add(newBranch);
                return RedirectToAction("Detail", new { id = model.Id });

            }
            return View();
        }

        //serves the EditBranch Page
        [HttpGet]
        public IActionResult EditBranch(int id)
        {
            var branch = _branch.GetById(id);
            var editbranchViewModel = new EditBranchViewModel
            {
                Id = branch.Id, 
                Name = branch.Name,
                Address = branch.Address,
                Telephone = branch.Telephone,
                Description = branch.Description,
                OpenDate = branch.OpenDate,
                ExistingImageName = branch.ImageUrl
            };
            return View(editbranchViewModel);
        }

        //Responds to Post request
        [HttpPost]
        public IActionResult EditBranch(EditBranchViewModel model)
        {
            var branch = _branch.GetById(model.Id);
            if (ModelState.IsValid)
            {
                branch.Id = model.Id;
                branch.Name = model.Name;
                branch.Address = model.Address;
                branch.Telephone = model.Telephone;
                branch.Description = model.Description;
                branch.OpenDate = model.OpenDate;
                if (model.ImageUrl != null)
                {
                    //if (model.ExistingImageName != null)
                    //{
                    //    var filepath = _hostingEnvironment.WebRootPath;
                    //    string name = model.ExistingImageName;
                    //    string filepaths = Path.Combine(filepath, name);
                    //    System.IO.File.Delete(filepaths);
                    //}
                    branch.ImageUrl = UploadFileMethod(model);
                    _branch.Update(branch);
                    return RedirectToAction("Detail", new { id = model.Id });
                }
                branch.ImageUrl = model.ExistingImageName;
                //call the update method
                _branch.Update(branch);

                return RedirectToAction("Detail", new { id = model.Id });
            }

            return View();
        }

        public IActionResult Index()
        {
            var branches = _branch.GetAll();

            var branchModel = branches
                .Select(b => new branchDetailViewModel
                {
                     Id = b.Id,
                     BranchName = b.Name,
                     NumberOfAssets = _branch.GetAssetCount(b.Id),
                     NumberOfPatrons = _branch.GetPatronCount(b.Id),
                     IsOpen = _branch.IsBranchOpen(b.Id)
                }).ToList();

            var model = new branchIndexModel
            {
                Branches = branchModel
            };

            return View(model);
        }

        public IActionResult Detail(int id)
        {
            var branch = _branch.GetById(id);
            var model = new branchDetailViewModel
            {
                Id = branch.Id,
                BranchName = branch.Name,
                Description = branch.Description,
                Address = branch.Address,
                Telephone = branch.Telephone,
                BranchOpenedDate = branch.OpenDate.ToString("yyy-MM-dd"),
                NumberOfPatrons = _branch.GetPatronCount(id),
                NumberOfAssets = _branch.GetAssetCount(id),
                TotalAssetValue = _branch.GetAssetsValue(id),
                ImageUrl = branch.ImageUrl,
                HoursOpen = _branch.GetBranchHours(id)
            };

            return View(model);
        }


        //The private method below helps to Extract the FullPath of the image uploaded...

        private string UploadFileMethod(NewBranchViewModel model)
        {
            string UniqueFilePath = null;
            if (model.ImageUrl != null)
            {
                //this provides us with the physical path to WWWroot folder
                //then we combine the path with the "images", "branches" path 
                string RootFilePath = Path.Combine(_hostingEnvironment.WebRootPath, "images", "branches");

                //extracting the image name
                string imageName = model.ImageUrl.FileName;
                string UniqueFilePaths = Path.Combine(RootFilePath, imageName);
                UniqueFilePath = imageName;

                //"copyTo" copies to the images/branches folder then "Filemode.create" "avails it n d server"
                //We r wrapping it up in the Using block so it will be disposed off properly wen done
                using (var fileStream = new FileStream(UniqueFilePaths, FileMode.Create))
                {
                    model.ImageUrl.CopyTo(fileStream);
                }
            }

            return UniqueFilePath;
        }
    }
}
