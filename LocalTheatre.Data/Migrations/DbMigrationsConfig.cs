namespace LocalTheatre.Data.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class DbMigrationsConfig : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public DbMigrationsConfig()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            // Seed initial data when the database is empty

            if (!context.Users.Any())
            {
                var adminEmail = "admin@admin.com";
                var adminUserName = adminEmail;
                var adminFullName = "Default Admin";
                var adminPassword = "default";
                string adminRole = "Admin";

                // creates default admin user
                var adminUser = new ApplicationUser
                {
                    UserName = adminUserName,
                    FullName = adminFullName,
                    Email = adminEmail
                };

                var userStore = new UserStore<ApplicationUser>(context);
                var UserManager = new UserManager<ApplicationUser>(userStore);
                UserManager.PasswordValidator = new PasswordValidator
                {
                    RequiredLength = 1,
                    RequireNonLetterOrDigit = false,
                    RequireDigit = false,
                    RequireLowercase = false,
                    RequireUppercase = false
                };
                var userCreateResult = UserManager.Create(adminUser, adminPassword);
                if (!userCreateResult.Succeeded)
                {
                    throw new Exception(string.Join("; ", userCreateResult.Errors));
                }

                // creates the admin role

                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                var roleCreateResult = roleManager.Create(new IdentityRole(adminRole));
                if(!roleCreateResult.Succeeded)
                {
                    throw new Exception(string.Join("; ", roleCreateResult.Errors));
                }

                // adds default admin to the admin role

                var addAdminRoleResult = UserManager.AddToRole(adminUser.Id, adminRole);
                if (!addAdminRoleResult.Succeeded)
                {
                    throw new Exception(string.Join("; ", addAdminRoleResult.Errors));
                }

                // adds default events to the database

                context.Announcements.Add(new Announcement
                {
                    Title = "Welcome To Our Website",
                    StartDateTime = DateTime.Now.AddDays(2),
                    Author = context.Users.First()
                });

                // adds a event with comments

                context.Announcements.Add(new Announcement
                {
                    Title = "Comment Test",
                    StartDateTime = DateTime.Now,
                    Duration = TimeSpan.FromHours(2),
                    Comments = new HashSet<Comment>()
                    {
                        new Comment() {Text = "anon comment" },
                        new Comment() {Text = "posted by a user", Author = context.Users.First() }
                    }
                });

                // adds a past event
                context.Announcements.Add(new Announcement
                {
                    Title = "Comment Test2",
                    StartDateTime = DateTime.Now.AddDays(-10),
                    Duration = TimeSpan.FromHours(2),
                    Comments = new HashSet<Comment>()
                    {
                        new Comment() {Text = "anon comment2" },
                        new Comment() {Text = "posted by a user2", Author = context.Users.First() }
                    }
                });
            }


        }
    }
}
