namespace Blog.Migrations
{
    using Blog.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Blog.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Blog.Models.ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));

            // See if a role is present in the DB
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }

            if (!context.Roles.Any(r => r.Name == "Moderator"))
            {
                roleManager.Create(new IdentityRole { Name = "Moderator" });
            }

            // Creates a user
            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));

            if(!context.Users.Any(u => u.Email == "peterbguthrie@gmail.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email       = "peterbguthrie@gmail.com",
                    UserName    = "peterbguthrie@gmail.com",
                    FirstName   = "Peter",
                    LastName    = "Guthrie",
                    DisplayName = "PeterG"
                }, "Abc&123!");
            }

            //Grab the ID
            var userId = userManager.FindByEmail("peterbguthrie@gmail.com").Id;
            // Now that I have created a user I want to assign this person to a role
            userManager.AddToRole(userId, "Admin");


            // Adding Drew as a Moderator
            if (!context.Users.Any(u => u.Email == "arussell@coderfoundry.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "arussell@coderfoundry.com",
                    UserName = "arussell@coderfoundry.com",
                    FirstName = "Andrew",
                    LastName = "Russell",
                    DisplayName = "Chief"
                }, "Abc&123!");
            }
            userId = userManager.FindByEmail("arussell@coderfoundry.com").Id;
            userManager.AddToRole(userId, "Moderator");

        }
    }
}