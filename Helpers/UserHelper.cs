using Blog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.Helpers
{
    public class UserHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public string GetDisplayName(string userId)
        {
            var user = db.Users.Find(userId);
            return user.DisplayName;
        }

        public string GetFullName(String userId)
        {
            var user = db.Users.Find(userId);
            var firstName = user.FirstName;
            var lastName = user.LastName;
            return firstName +"" + lastName;
        }
    }
}