using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Blog.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int BlogPostId { get; set; }
        public string AuthorId { get; set; }
        [AllowHtml]
        public string commentBody { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated {get;set;}
        public string UpdateReason { get; set; }

        // Navigational properties
        public virtual BlogPost BlogPost { get; set; }
        public virtual ApplicationUser Author { get; set; }
    }
}