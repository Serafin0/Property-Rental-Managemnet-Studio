using WebApplicationFinal.Models;  // Ensure to import your models
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using LoginViewModel = WebApplicationFinal.Models.LoginViewModel;


namespace PropertyProjectManager.Controllers
{
    public class AccountController : Controller
    {
        private PropertyProjectManagerEntities db = new PropertyProjectManagerEntities();  // Database context, replace with your context name

        // GET: Account/Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                string hashedPassword = HashPassword(model.Password); // Ensure proper password hashing

                // Check if the email and password match any of the user tables based on UserType
                if (model.UserType == "Manager")
                {
                    var manager = db.PropertyManagers
                                    .FirstOrDefault(m => m.Email == model.Email && m.Password == hashedPassword);

                    if (manager != null)
                    {
                        // Store session information for the manager
                        Session["UserID"] = manager.ManagerId;
                        Session["UserType"] = "Manager";

                        return RedirectToAction("Index", "Home");
                    }
                }
                else if (model.UserType == "Owner")
                {
                    var owner = db.PropertyOwners
                                  .FirstOrDefault(o => o.Email == model.Email && o.Password == hashedPassword);

                    if (owner != null)
                    {
                        // Store session information for the owner
                        Session["UserID"] = owner.LandlordId;
                        Session["UserType"] = "Owner";

                        return RedirectToAction("Index", "Home");
                    }
                }
                else if (model.UserType == "Tenant")
                {
                    var tenant = db.Tenants
                                   .FirstOrDefault(t => t.Email == model.Email && t.Password == hashedPassword);

                    if (tenant != null)
                    {
                        // Store session information for the tenant
                        Session["UserID"] = tenant.TenantId;
                        Session["UserType"] = "Tenant";

                        return RedirectToAction("Index", "Home");
                    }
                }

                // If login fails, add a general error message
                // Added comment: Make sure the ValidationSummary in Login.cshtml displays this error
                ModelState.AddModelError("", "Invalid email, password, or user type.");
            }
            else
            {
                // Added handling for model validation errors
                // Example: In case the email field is empty or improperly formatted
                ModelState.AddModelError("", "Please provide valid credentials.");
            }

            // Ensure email is retained in the view even if login fails
            // Added comment: Email should not be cleared for user convenience
            model.Password = string.Empty; // Clear the password field for security purposes

            // Return the view with validation errors if login fails
            return View(model);
        }

        // GET : Account/Register
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if the email already exists in any of the user tables
                bool emailExists = db.PropertyManagers.Any(m => m.Email == model.Email) ||
                                   db.PropertyOwners.Any(o => o.Email == model.Email) ||
                                   db.Tenants.Any(t => t.Email == model.Email);

                if (emailExists)
                {
                    // Add a model error and return the same view if the email exists
                    ModelState.AddModelError("Email", "This email is already registered. Please use a different email.");
                    return View(model);
                }

                // Hash the password for security
                string hashedPassword = HashPassword(model.Password);

                // Determine the user type and add to the appropriate table
                if (model.UserType == "Manager")
                {
                    var newManager = new PropertyManager
                    {
                        Email = model.Email,
                        Password = hashedPassword // Make sure the PropertyManager table has a Password column
                                                  // Add other default values or required fields if needed
                    };

                    db.PropertyManagers.Add(newManager);
                }
                else if (model.UserType == "Owner")
                {
                    var newOwner = new PropertyOwner
                    {
                        Email = model.Email,
                        Password = hashedPassword // Make sure the PropertyOwner table has a Password column
                                                  // Add other default values or required fields if needed
                    };

                    db.PropertyOwners.Add(newOwner);
                }
                else if (model.UserType == "Tenant")
                {

                    int managerId = 300; // Connects to a property manager that already exists

                    var newTenant = new Tenant()
                    {
                        FirstName = model.FirstName,  // Map the FirstName from the model
                        LastName = model.LastName,    // Map the LastName from the model
                        Email = model.Email,          // Map the Email from model
                        Password = hashedPassword,    // Map the Password from model
                        AdminId = managerId           // Set the AdminId (ManagerId) from the session
                    };

                    db.Tenants.Add(newTenant);
                }
                else
                {
                    ModelState.AddModelError("", "Invalid User Type selected.");
                    return View(model);
                }

                // Save changes to the database
                db.SaveChanges();

                // Redirect to the login page after successful registration
                return RedirectToAction("Login", "Account");
            }

            // If model validation fails, return the same view with error messages
            return View(model);
        }


        // Log out user and clear session
        public ActionResult Logout()
        {
            Session.Clear();  // Clear the session data
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }

        // Helper method to hash the password
        private string HashPassword(string password)
        {
            // For demo purposes, let's use simple hashing. In a real app, use a stronger hashing algorithm such as SHA256 or bcrypt
            return password; // Implement your own password hashing logic (e.g., SHA256, bcrypt)
        }




        // You can implement other actions like Register, Forgot Password, etc., here if needed.
    }
}



