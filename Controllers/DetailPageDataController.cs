using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DetailPage.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DetailPage.Controllers
{
    [Route("api/[controller]")]
    public class DetailPageDataController : Controller
    {
        private readonly DetailPageContext _detailPageContext;
        private readonly IHostingEnvironment _hostingEnvironment;

        public DetailPageDataController(DetailPageContext detailPageContext, IHostingEnvironment hostingEnvironment)
        {
            _detailPageContext = detailPageContext;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<DetailPageModel>> DetailPages()
        {
            var list = await _detailPageContext.DetailPages.AsNoTracking().ToListAsync();
            foreach (var detailPageModel in list)
            {
                //获取图片文件
                var masterPath = Path.Combine(_hostingEnvironment.WebRootPath,
                    $"Uploads/Master/{detailPageModel.ProductNo}");
                var detailPath = Path.Combine(_hostingEnvironment.WebRootPath,
                    $"Uploads/Detail/{detailPageModel.ProductNo}");

                if (Directory.Exists(masterPath))
                {
                    var files = Directory.GetFiles(masterPath);
                    detailPageModel.MasterImages = new List<string>();
                    foreach (var file in files)
                    {
                        detailPageModel.MasterImages.Add(Path.GetFileName(file));
                    }
                }

                if (Directory.Exists(detailPath))
                {
                    var files = Directory.GetFiles(detailPath);
                    detailPageModel.DetailImages = new List<string>();
                    foreach (var file in files)
                    {
                        detailPageModel.DetailImages.Add(Path.GetFileName(file));
                    }
                }
            }

            return list;
        }

        [HttpPost("[action]")]
        public async Task<ResponseResultViewModel> Create([FromBody] [Bind("Name,ProductNo,HtmlContent,Remark")]
            DetailPageModel detailPageModel)
        {
            var responseResult = new ResponseResultViewModel();
            try
            {
                if (ModelState.IsValid)
                {
                    _detailPageContext.Add(detailPageModel);
                    await _detailPageContext.SaveChangesAsync();
                    responseResult.IsSuccess = true;
                    responseResult.Message = "保存成功";
                }
                else
                {
                    responseResult.IsSuccess = false;
                    responseResult.Message = "提交的数据有问题";
                }
            }
            catch (DbUpdateException e)
            {
                responseResult.IsSuccess = false;
                responseResult.Message = "保存失败";
            }

            return responseResult;
        }

        [HttpPost("[action]")]
        public async Task<ResponseResultViewModel> Delete(int id, string productNo)
        {
            var responseResult = new ResponseResultViewModel();
            try
            {
                DetailPageModel detailPageModel = new DetailPageModel() {ID = id};
                _detailPageContext.Entry(detailPageModel).State = EntityState.Deleted;
                await _detailPageContext.SaveChangesAsync();
                //删除图片
                var detailPath = Path.Combine(_hostingEnvironment.WebRootPath,
                    $"Uploads/Detail/{productNo}");
                var masterPath = Path.Combine(_hostingEnvironment.WebRootPath,
                    $"Uploads/Master/{productNo}");
                if (Directory.Exists(detailPath))
                {
                    foreach (var file in Directory.GetFiles(detailPath))
                    {
                        System.IO.File.Delete(file);
                    }
                }

                if (Directory.Exists(masterPath))
                {
                    foreach (var file in Directory.GetFiles(masterPath))
                    {
                        System.IO.File.Delete(file);
                    }
                }
                
                responseResult.IsSuccess = true;
                responseResult.Message = "删除成功";
            }
            catch (DbUpdateException e)
            {
                responseResult.IsSuccess = false;
                responseResult.Message = "删除失败";
            }

            return responseResult;
        }

        [HttpPost("[action]")]
        public async Task<ResponseResultViewModel> Edit(int id,
            [FromBody] [Bind("ID,Name,ProductNo,HtmlContent,Remark")]
            DetailPageModel detailPageModel)
        {
            var responseResult = new ResponseResultViewModel();
            if (id != detailPageModel.ID)
            {
                responseResult.IsSuccess = false;
                responseResult.Message = "没有找到数据";
            }

            try
            {
                _detailPageContext.Update(detailPageModel);
                await _detailPageContext.SaveChangesAsync();
                responseResult.IsSuccess = true;
                responseResult.Message = "更新成功";
            }
            catch (DbUpdateException e)
            {
                responseResult.IsSuccess = false;
                responseResult.Message = "更新失败";
            }

            return responseResult;
        }
    }
}