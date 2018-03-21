using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DetailPage.Models
{
    public class DetailPageModel
    {
        public int ID { get; set; }
        [StringLength(30)] public string Name { get; set; }
        [StringLength(30)] public string ProductNo { get; set; }
        public string HtmlContent { get; set; }
        [StringLength(1000)] public string Url { get; set; }
        [StringLength(400)] public string Remark { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None), NotMapped]
        public List<string> MasterImages { get; set; }  
        [DatabaseGenerated(DatabaseGeneratedOption.None), NotMapped]
        public List<string> DetailImages { get; set; } 
    }
}