using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Threading.Tasks;
using DetailPage.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DetailPage.Controllers
{
    [Route("api/[controller]")]
    public class UploadMasterImageController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public UploadMasterImageController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost("[action]")]
        [Consumes("application/json", "multipart/form-data")] //此处为新增
        public async Task<FileResponseViewModel> Post(string productNo, IFormCollection files)
        {
            var responseViewModel = new FileResponseViewModel();
            responseViewModel.FileList = new List<FileInfomation>();
            try
            {
                if (string.IsNullOrWhiteSpace(productNo))
                {
                    responseViewModel.IsSuccess = false;
                    responseViewModel.Message = "商品编码不能为空";
                }
                else
                {
                    var detailPath = Path.Combine(_hostingEnvironment.WebRootPath, $"Uploads/Master/{productNo}");
                    if (!Directory.Exists(detailPath))
                    {
                        Directory.CreateDirectory(detailPath);
                    }


                    var originName = string.Empty;
                    var extension = string.Empty;
                    var newName = string.Empty;
//                    var index = 0;
                    var newPath = string.Empty;
                    foreach (var file in files.Files)
                    {
                        originName = file.FileName;
                        //eg .jpg
                        extension = Path.GetExtension(originName);
                        newName = $"{productNo}_master_{DateTime.Now:yyyyMMddHHmmssfff}{extension}";
                        newPath = Path.Combine(detailPath, newName);
                        if (System.IO.File.Exists(newPath))
                        {
                            System.IO.File.Delete(newPath);
                        }

                        using (var stream = new FileStream(newPath, FileMode.CreateNew))
                        {
                            await file.CopyToAsync(stream);

                            responseViewModel.FileList.Add(new FileInfomation()
                            {
                                Name = newName,
                                Path = $"Uploads/Master/{productNo}/"
                            });
                        }
                    }

                    responseViewModel.IsSuccess = true;
                    responseViewModel.Message = "图片上传成功";
                }
            }
            catch (Exception e)
            {
                responseViewModel.IsSuccess = false;
                responseViewModel.Message = "上传文件失败";
            }

            return responseViewModel;
        }

        [HttpPost("[action]")]
        public async Task<FileResponseViewModel> Delete(string productNo, string fileName)
        {
            var responseViewModel = new FileResponseViewModel();
            try
            {
                if (string.IsNullOrWhiteSpace(productNo))
                {
                    responseViewModel.IsSuccess = false;
                    responseViewModel.Message = "商品编码不能为空";
                }
                else
                {
                    var detailPath = Path.Combine(_hostingEnvironment.WebRootPath,
                        $"Uploads/Master/{productNo}/{fileName}");
                    if (System.IO.File.Exists(detailPath))
                    {
                        System.IO.File.Delete(detailPath);
                        responseViewModel.IsSuccess = true;
                        responseViewModel.Message = "图片删除成功";
                    }
                    else
                    {
                        responseViewModel.IsSuccess = false;
                        responseViewModel.Message = "图片不存在";
                    }
                }
            }
            catch (Exception e)
            {
                responseViewModel.IsSuccess = false;
                responseViewModel.Message = "图片删除失败";
            }

            return responseViewModel;
        }
    }
}