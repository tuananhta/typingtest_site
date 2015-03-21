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
    public class Word
    {
        public int Id { get; set; }

        [Display(Name = "Word Context")]
        [Required(ErrorMessage = "This field can not be empty.")]
        public string WContext { get; set; }

        [Display(Name = "Created")]
        public DateTime CreatedTime { get; set; }
        public int Level { get; set; }
        public string fColor { get; set; }
        public string WPronounce { get; set; }
        public virtual IEnumerable<WordDef> WordDefs { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}