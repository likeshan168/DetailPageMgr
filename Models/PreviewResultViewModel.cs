using System;
using System.Collections.Generic;

namespace DetailPage.Models
{
    public class PreviewResultViewModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public List<string> MasterImages { get; set; }
        public List<string> DetailImages { get; set; }
    }
}