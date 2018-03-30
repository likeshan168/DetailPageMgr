using System.Collections.Generic;

namespace DetailPage.Models
{
    public class PageModel
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
        public string SearchWord { get; set; }
        public IEnumerable<DetailPageModel> Data { get; set; }
    }
}