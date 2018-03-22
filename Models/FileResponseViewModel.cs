using System.Collections.Generic;

namespace DetailPage.Models
{
    public class FileResponseViewModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public List<FileInfomation> FileList { get; set; }
    }

    public class FileInfomation
    {
        public string Name { get; set; }
        public string Path { get; set; } 
    }
}