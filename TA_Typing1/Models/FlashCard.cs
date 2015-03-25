using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TA_Typing1.Models
{
    public class FlashCard
    {
        public int id { get;set; }
        public int wordId { get; set; }
        public DateTime createdTime { get; set; }
        public string fColor { get; set; }
        public bool fFavourite { get; set; }
        public virtual Word word { get; set; }
    }
}