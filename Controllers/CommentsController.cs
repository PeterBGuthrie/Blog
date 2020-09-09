using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Blog.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Ajax.Utilities;

namespace Blog.Controllers
{
    public class CommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Comments
        public ActionResult Index()
        {
            // TODO: paging for comments
            //int pageSize = 10; // Specifies the number of posts per page
            //int pageNumber = (page ?? 1);
            //var allBlogPosts = db.BlogPosts.OrderBy(b => b.Created).ToPagedList(pageNumber, pageSize);
            var comments = db.Comments.Include(c => c.Author).Include(c => c.BlogPost);
            return View(comments.ToList());
        }

        // GET: Comments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // GET: Comments/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.AuthorId = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.BlogPostId = new SelectList(db.BlogPosts, "Id", "Title");
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string commentBody, int blogPostId, string slug)
        {
            if (commentBody.IsNullOrWhiteSpace())
            {
                return RedirectToAction("Details", "BlogPosts", new { slug = slug });
            }
            var comment = new Comment
            {
                commentBody = commentBody,
                Created     = DateTime.Now,
                BlogPostId  = blogPostId,
                AuthorId    = User.Identity.GetUserId()
            };

                db.Comments.Add(comment);
                db.SaveChanges();
            // slug here can be simplified. THis works because I pass into this controller slug by the same name.
            //                     ("Details", "BlogPosts", new { slug});
            return RedirectToAction("Details", "BlogPosts", new { slug = slug });

        }

        // GET: Comments/Edit/5
        [Authorize(Roles = "Admin, Moderator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            ViewBag.AuthorId = new SelectList(db.Users, "Id", "FirstName", comment.AuthorId);
            ViewBag.BlogPostId = new SelectList(db.BlogPosts, "Id", "Title", comment.BlogPostId);
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin, Moderator")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,BlogPostId,AuthorId,commentBody,Created,Updated,UpdateReason")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                comment.Updated = DateTime.Now;
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                // TODO: redirect to the post the comment is on
                return RedirectToAction("Index");
                //return RedirectToAction("Details", "BlogPosts", neSystem.Data.Entity.Infrastructure.DbUpdateException: w { slug = slug });
            }
            ViewBag.AuthorId = new SelectList(db.Users, "Id", "FirstName", comment.AuthorId);
            ViewBag.BlogPostId = new SelectList(db.BlogPosts, "Id", "Title", comment.BlogPostId);
            return View(comment);
        }

        // GET: Comments/Delete/5
        [Authorize(Roles = "Admin, Moderator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]

        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comment comment = db.Comments.Find(id);
            db.Comments.Remove(comment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
