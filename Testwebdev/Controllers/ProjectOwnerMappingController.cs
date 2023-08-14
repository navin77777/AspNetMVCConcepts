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
        public ActionResult Index()
        {
            List<ProjectOwnerMapping> mappings = GetProjectOwnerMappings(); 
            return View(mappings);
        }

        private List<ProjectOwnerMapping> GetProjectOwnerMappings()
        {
            List<ProjectOwnerMapping> mappings = new List<ProjectOwnerMapping>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("USP_GetProjectOwnerMappings", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ProjectOwnerMapping mapping = new ProjectOwnerMapping
                            {
                                MappingId = Convert.ToInt32(reader["MappingId"]),
                                ProjectName = reader["ProjectName"].ToString(),
                                ApplicationOwnerName = reader["ApplicationOwnerName"].ToString(),
                                StartDate = Convert.ToDateTime(reader["StartDate"]),
                                EndDate = Convert.ToDateTime(reader["EndDate"])
                            };
                            mappings.Add(mapping);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception
            }

            return mappings;
        }


        public ActionResult Edit(int id)
        {
            ProjectOwnerMapping mapping = GetMappingById(id);

            if (mapping == null)
            {
                return RedirectToAction("Index");
            }

            IEnumerable<Project> projects = GetActiveProjects();
            IEnumerable<ApplicationOwner> applicationOwners = GetApplicationOwners();

            ViewBag.Projects = new SelectList(projects, "ProjectId", "ProjectName");
            ViewBag.ApplicationOwners = new SelectList(applicationOwners, "ApplicationOwnerId", "Name");

            return View(mapping);
        }



        private ProjectOwnerMapping GetMappingById(int id)
        {
            ProjectOwnerMapping mapping = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("USP_GetMappingById", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@MappingId", id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            mapping = new ProjectOwnerMapping
                            {
                                MappingId = Convert.ToInt32(reader["MappingId"]),
                                ProjectId = Convert.ToInt32(reader["ProjectId"]),
                                ApplicationOwnerId = Convert.ToInt32(reader["ApplicationOwnerId"]),
                                StartDate = Convert.ToDateTime(reader["StartDate"]),
                                EndDate = Convert.ToDateTime(reader["EndDate"])
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception
            }

            return mapping;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProjectOwnerMapping mapping)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand("USP_UpdateProjectOwnerMapping", connection);
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@MappingId", mapping.MappingId);
                        command.Parameters.AddWithValue("@ProjectId", mapping.ProjectId);
                        command.Parameters.AddWithValue("@ApplicationOwnerId", mapping.ApplicationOwnerId);
                        command.Parameters.AddWithValue("@StartDate", mapping.StartDate);
                        command.Parameters.AddWithValue("@EndDate", mapping.EndDate);
                        command.Parameters.AddWithValue("@UpdatedBy", "Admin");

                        command.ExecuteNonQuery();

                        TempData["SuccessMessage"] = "Project-Owner Mapping updated successfully.";
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception ex)
                {
                    //TempData["ErrorMessage"] = "An error occurred while updating the mapping: " + ex.Message;
                }
            }

            return RedirectToAction("Index"); 
        }

    }
}
