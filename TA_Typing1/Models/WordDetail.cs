using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TA_Typing1.Models
{
    public class WordDetail
    {
        public int Id {get;set;}
        public DateTime CreatedTime { get; set; }
        [Required]
        public string WContext { get; set; }
        public int WType { get; set; }
        public int Level { get; set; }
        public string WPronounce { get; set; }
    }
}