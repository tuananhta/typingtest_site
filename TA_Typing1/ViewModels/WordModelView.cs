using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TA_Typing1.ViewModels
{
    public class WordModelView
    {
        public int Id { get; set; }
        [Display(Name = "Created")]
        public DateTime CreatedTime { get; set; }
        [Display(Name = "Word Context")]
        [Required(ErrorMessage = "This field can not be empty.")]
        public string WContext { get; set; }
        public int WType { get; set; }
        public int Level { get; set; }
        public string WPronounce { get; set; }
    }
}