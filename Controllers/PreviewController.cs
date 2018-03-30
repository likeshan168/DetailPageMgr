using System.Collections.Generic;
using System.IO;
using DetailPage.Common;
using DetailPage.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.OData;

namespace DetailPage.Controllers
{
    [ValidationFilter]
    public class PreviewController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        public PreviewController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        // GET
        public IActionResult Index(string productNo)
        {
//            ViewData["ProductNo"] = productNo;
            PreviewResultViewModel resultViewModel = new PreviewResultViewModel();
            if (string.IsNullOrWhiteSpace(productNo))
            {
                resultViewModel.IsSuccess = false;
                resultViewModel.Message = "商品编码不能为空";
            }
            else
            {
                resultViewModel.IsSuccess = true;
                resultViewModel.Message = "获取图片成功";
                ViewData["ProductNo"] = productNo;
                
                var masterPath = Path.Combine(_hostingEnvironment.WebRootPath, $"Uploads/Master/{productNo}");
                var detailPath = Path.Combine(_hostingEnvironment.WebRootPath, $"Uploads/Detail/{productNo}");
                resultViewModel.MasterImages = new List<string>();
                resultViewModel.DetailImages = new List<string>();
                if (Directory.Exists(masterPath))
                {
                    var files = Directory.GetFiles(masterPath);
                    foreach (var file in files)
                    {
                        resultViewModel.MasterImages.Add($"Uploads/Master/{productNo}/{Path.GetFileName(file)}");
                    }
                }
                if (Directory.Exists(detailPath))
                {
                    var files = Directory.GetFiles(detailPath);
                    foreach (var file in files)
                    {
                        resultViewModel.DetailImages.Add($"Uploads/Detail/{productNo}/{Path.GetFileName(file)}");
                    }
                }
            }
            return View(resultViewModel);
//            return View("~/Views/Preview/Index.html");
        }
    }
}