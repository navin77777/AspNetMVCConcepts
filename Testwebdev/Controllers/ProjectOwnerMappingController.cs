using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Testwebdev.Models;
using System.Configuration;

namespace Testwebdev.Controllers
{
    public class ProjectOwnerMappingController : Controller
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public ActionResult Create()
        {
            IEnumerable<Project> projects = GetActiveProjects();
            IEnumerable<ApplicationOwner> applicationOwners = GetApplicationOwners();

            ViewBag.ProjectId = new SelectList(projects, "ProjectId", "ProjectName");  
            ViewBag.ApplicationOwners = new SelectList(applicationOwners, "ApplicationOwnerId", "Name");

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProjectOwnerMapping mapping)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand("USP_InsertProjectOwnerMapping", connection);
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@ProjectId", mapping.ProjectId);
                        command.Parameters.AddWithValue("@ApplicationOwnerId", mapping.ApplicationOwnerId);
                        command.Parameters.AddWithValue("@StartDate", mapping.StartDate);
                        command.Parameters.AddWithValue("@EndDate", mapping.EndDate);
                        command.Parameters.AddWithValue("@CreatedBy", "Admin");

                        command.ExecuteNonQuery();

                        //ViewBag.Message = "Mapping inserted successfully.";
                        TempData["SuccessMessage"] = "Project Mapped successfully.";

                    }
                }
                catch (Exception ex)
                {
                    //ViewBag.Error = "An error occurred: " + ex.Message;
                }
            }

            return View(mapping);
        }

        private List<Project> GetActiveProjects()
        {
            List<Project> projects = new List<Project>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("USP_GetActiveProjectsForDropdown", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Project project = new Project
                            {
                                ProjectId = Convert.ToInt32(reader["ProjectId"]),
                                ProjectName = reader["ProjectName"].ToString()
                            };
                            projects.Add(project);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception
            }

            return projects;
        }
        private List<ApplicationOwner> GetApplicationOwners()
        {
            List<ApplicationOwner> applicationOwners = new List<ApplicationOwner>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("USP_GetApplicationOwners", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ApplicationOwner owner = new ApplicationOwner
                            {
                                ApplicationOwnerId = Convert.ToInt32(reader["ApplicationOwnerId"]),
                                Name = reader["Name"].ToString()
                            };
                            applicationOwners.Add(owner);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception
            }

            return applicationOwners;
        }

    }
}
