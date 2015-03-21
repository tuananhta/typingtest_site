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
    public class WordDef
    {
        public int id { get; set; }
        [Required]
        public int wordId { get; set; }
        [Required(ErrorMessage = "This field can not be empty.")]
        public string wType { get; set; }
        [Required(ErrorMessage = "This field can not be empty.")]
        public string wDefinition { get; set; }
        public string wExample { get; set; }
        public virtual Word word { get; set; }
    }
}