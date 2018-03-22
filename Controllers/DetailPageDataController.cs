using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DetailPage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DetailPage.Controllers
{
    [Route("api/[controller]")]
    public class DetailPageDataController : Controller
    {
        private DetailPageContext _detailPageContext;

        public DetailPageDataController(DetailPageContext detailPageContext)
        {
            _detailPageContext = detailPageContext;
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<DetailPageModel>> DetailPages()
        {
            return await _detailPageContext.DetailPages.ToListAsync();
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
        public async Task<ResponseResultViewModel> Delete(int id)
        {
            var responseResult = new ResponseResultViewModel();
            try
            {
                DetailPageModel detailPageModel = new DetailPageModel() {ID = id};
                _detailPageContext.Entry(detailPageModel).State = EntityState.Deleted;
                await _detailPageContext.SaveChangesAsync();
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
            if (id!= detailPageModel.ID)
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