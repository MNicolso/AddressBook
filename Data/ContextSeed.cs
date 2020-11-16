using AddressBook.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace IssueTracker.Enums
{
    public static class ContextSeed
    {

        public enum Roles
        {
            Admin,
            AddressBookUser
        }


        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManger)
        {
            await roleManger.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleManger.CreateAsync(new IdentityRole(Roles.AddressBookUser.ToString()));


        }

        public static async Task SeedDefaultUserAsync(UserManager<ABUser> userManager)
        {
            //ADMIN
            var defaultAdmin = new ABUser
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                FirstName = "Bill",
                LastName = "Admin",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultAdmin.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultAdmin, "Abc&123!");
                    await userManager.AddToRoleAsync(defaultAdmin, Roles.Admin.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("**error**");
                Debug.WriteLine("error Seeding Default Admin User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("****************");
            }

            //AddressBookUser
            var defaultAddressBookUser = new ABUser
                {
                    UserName = "user@gmail.com",
                    Email = "user@gmail.com",
                    FirstName = "Tom",
                    LastName = "User",
                    EmailConfirmed = true
                };
                try
                {
                    var user = await userManager.FindByEmailAsync(defaultAddressBookUser.Email);
                    if (user == null)
                    {
                        await userManager.CreateAsync(defaultAddressBookUser, "Abc&123!");
                        await userManager.AddToRoleAsync(defaultAddressBookUser, Roles.AddressBookUser.ToString());
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("**error**");
                    Debug.WriteLine("error Seeding Default User User.");
                    Debug.WriteLine(ex.Message);
                    Debug.WriteLine("****************");
                }
            }
        }
    }
