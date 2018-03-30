using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DetailPage.Common;
using DetailPage.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DetailPage.Controllers
{
    [Route("api/[controller]")]
    [ValidationFilter]
    public class DetailPageDataController : Controller
    {
        private readonly DetailPageContext _detailPageContext;
        private readonly IHostingEnvironment _hostingEnvironment;

        public DetailPageDataController(DetailPageContext detailPageContext, IHostingEnvironment hostingEnvironment)
        {
            _detailPageContext = detailPageContext;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost("[action]")]
        public async Task<PageModel> DetailPages([FromBody] PageModel pageModel)
        {
            List<DetailPageModel> list ;
            if (string.IsNullOrWhiteSpace(pageModel.SearchWord))
            {
                list = await _detailPageContext.DetailPages.OrderBy(p=>p.ID)
                    .Skip((pageModel.PageNumber - 1) * pageModel.PageSize)
                    .Take(pageModel.PageSize)
                    .AsNoTracking().ToListAsync();
                pageModel.Total = _detailPageContext.DetailPages.Count();
            }
            else
            {
                list = await _detailPageContext.DetailPages.OrderBy(p=>p.ID)
                    .Where(p => p.ProductNo.Contains(pageModel.SearchWord) || 
                                p.Name.Contains(pageModel.SearchWord) ||
                                p.HtmlContent.Contains(pageModel.SearchWord))
                    .Skip((pageModel.PageNumber - 1) * pageModel.PageSize)
                    .Take(pageModel.PageSize)
                    .AsNoTracking().ToListAsync();
                pageModel.Total = _detailPageContext.DetailPages.Count(p => p.ProductNo.Contains(pageModel.SearchWord) || 
                                p.Name.Contains(pageModel.SearchWord) ||
                                p.HtmlContent.Contains(pageModel.SearchWord));
            }

           
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

            pageModel.Data = list;

            return pageModel;
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

        [HttpPost("[action]")]
        public async Task<IEnumerable<DetailPageModel>> Search(string searchWord)
        {
            List<DetailPageModel> list;
            if (string.IsNullOrWhiteSpace(searchWord))
            {
                list = await _detailPageContext.DetailPages.AsNoTracking().ToListAsync();
            }
            else
            {
                list = await _detailPageContext.DetailPages
                    .Where(p => p.ProductNo.Contains(searchWord) || p.Name.Contains(searchWord) ||
                                p.HtmlContent.Contains(searchWord)).AsNoTracking().ToListAsync();
            }

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
    }
}