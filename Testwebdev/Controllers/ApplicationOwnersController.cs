using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Testwebdev.Models;

namespace Testwebdev.Controllers
{
    //public class ApplicationOwners
    //{
    //}

        public class ApplicationOwnersController : Controller
        {
            //private string connectionString = "Your_Connection_String_Here";
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        // GET: ApplicationOwners
        public ActionResult Index()
        
        
        {
            List<ApplicationOwner> owners = GetActiveApplicationOwners();
            return View(owners);
        }

        private List<ApplicationOwner> GetActiveApplicationOwners()
        {
            List<ApplicationOwner> owners = new List<ApplicationOwner>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "EXEC USP_GetActiveApplicationOwners"; // Call your stored procedure
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ApplicationOwner owner = new ApplicationOwner
                    {
                        ApplicationOwnerId = (int)reader["ApplicationOwnerId"],
                        Name = (string)reader["Name"],
                        EmployeeId = (string)reader["EmployeeId"],
                        Email = (string)reader["Email"],
                        PhoneNumber = (string)reader["PhoneNumber"],
                        Designation = (string)reader["Designation"],
                        CreatedDateTime = (DateTime)reader["CreatedDateTime"],
                        CreatedBy = (string)reader["CreatedBy"]
                    };
                    owners.Add(owner);
                }
            }

            return owners;
        }




            // GET: ApplicationOwners/Create
            public ActionResult Create()
            {
                return View();
            }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ApplicationOwner applicationOwner)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = "EXEC USP_InsertApplicationOwner @Name, @EmployeeId, @Email, @PhoneNumber, @Designation, @CreatedBy";
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@Name", applicationOwner.Name);
                        command.Parameters.AddWithValue("@EmployeeId", applicationOwner.EmployeeId);
                        command.Parameters.AddWithValue("@Email", applicationOwner.Email);
                        command.Parameters.AddWithValue("@PhoneNumber", applicationOwner.PhoneNumber);
                        command.Parameters.AddWithValue("@Designation", applicationOwner.Designation);
                        command.Parameters.AddWithValue("@CreatedBy", "Admin");

                        command.ExecuteNonQuery();
                    }

                    // Display a Toastr notification for success
                    TempData["SuccessMessage"] = "ApplicationOwner created successfully!";
                    return RedirectToAction("Index"); // Redirect to the Index action method
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                ModelState.AddModelError(string.Empty, "An error occurred while processing your request.");
            }

            // Display a Toastr notification for failure
            TempData["ErrorMessage"] = "An error occurred while creating ApplicationOwner.";
            return View(applicationOwner); // Return the view with the model
        }
    


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ApplicationOwner owner;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("USP_GetApplicationOwnerById", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ApplicationOwnerId", id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            owner = new ApplicationOwner
                            {
                                ApplicationOwnerId = (int)reader["ApplicationOwnerId"],
                                Name = (string)reader["Name"],
                                EmployeeId = (string)reader["EmployeeId"],
                                Email = (string)reader["Email"],
                                PhoneNumber = (string)reader["PhoneNumber"],
                                Designation = (string)reader["Designation"],
                            };
                        }
                        else
                        {
                            return HttpNotFound();
                        }
                    }
                }
            }

            return View(owner);
        }

[HttpPost]
[ValidateAntiForgeryToken]
public ActionResult Edit(ApplicationOwner model)
{
    if (ModelState.IsValid)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            string updateQuery = "USP_UpdateApplicationOwner";
            using (SqlCommand command = new SqlCommand(updateQuery, connection))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@ApplicationOwnerId", model.ApplicationOwnerId);
                command.Parameters.AddWithValue("@Name", model.Name);
                command.Parameters.AddWithValue("@EmployeeId", model.EmployeeId);
                command.Parameters.AddWithValue("@Email", model.Email);
                command.Parameters.AddWithValue("@PhoneNumber", model.PhoneNumber);
                command.Parameters.AddWithValue("@Designation", model.Designation);
                command.Parameters.AddWithValue("@UpdatedBy", "Admin");

                try
                {
                    command.ExecuteNonQuery();
                    TempData["SuccessMessage"] = "ApplicationOwner Updated successfully!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // Handle exception, log error, etc.
                    TempData["ErrorMessage"] = "An error occurred while updating the record.";
                }
            }
        }
    }
    else
    {
        TempData["ErrorMessage"] = "Invalid data. Please check the input.";
    }

    return View("Edit", model);
}

    }


}