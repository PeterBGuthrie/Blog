using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.Models
{
    public class BlogPost
    {
        public int Id            { get; set; }
        [Display(Name = "Title:")]
        public string Title      { get; set; }
        public string Slug       { get; set; }

        [AllowHtml]
        [Display(Name = "Article")]
        public string Body       { get; set; }
        public string Abstract   { get; set; }
        [Display(Name = "Created: ")]
        public DateTime Created  { get; set; }
        [Display(Name = "Revised On: ")]
        public DateTime? Updated { get; set; }

        // MediaPath instead of MediaURL
        [Display(Name = "Image Path")]
        public string MediaPath  { get; set; }
        [Display(Name = "Published:")]
        public bool Published    { get; set; }


        // Navigational properties
        public virtual ICollection<Comment> Comments { get; set; }

        public BlogPost()
        {
            Comments = new HashSet<Comment>();
        }
    }
}