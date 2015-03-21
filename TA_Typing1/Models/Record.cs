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
    public class Record
    {
        public int Id { get; set; }
        public bool pri_rec { get; set; } // to check if the record is private or public
        public int wpm { get; set; }

        [Display(Name = "Created")]
        public DateTime CreatedTime { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}