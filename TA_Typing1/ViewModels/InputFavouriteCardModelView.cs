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
    public class InputFavouriteCardModelView
    {
        [Required]
        public int id { get; set; }
        public bool fFavourite { get; set; }
    }
}