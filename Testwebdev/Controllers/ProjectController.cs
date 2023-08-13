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
    public class ProjectController : Controller
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        // GET: Project
        //public ActionResult Index()
        //{
        //    return View();
        //}




        public ActionResult Index()
        {
            List<Project> projects = GetActiveProjects();
            return View(projects);
        }




          
        private List<Project> GetActiveProjects()
        {
            List<Project> projects = new List<Project>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("USP_GetActiveProjects", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Project project = new Project
                            {
                                ProjectId = Convert.ToInt32(reader["ProjectId"]),
                                ProjectName = GetStringOrNull(reader["ProjectName"]),
                                DomainName = GetStringOrNull(reader["DomainName"]),
                                ProjectDescription = GetStringOrNull(reader["ProjectDescription"]),
                                ProjectType = GetStringOrNull(reader["ProjectType"]),
                                ApplicationOwnerId = (int)GetNullableInt(reader["ApplicationOwnerId"]),
                                ProductionServerIp = GetStringOrNull(reader["ProductionServerIp"]),
                                ProductionDatabaseIp = GetStringOrNull(reader["ProductionDatabaseIp"]),
                                ProductionDatabaseDomain = GetStringOrNull(reader["ProductionDatabaseDomain"]),
                                UATServerIp = GetStringOrNull(reader["UATServerIp"]),
                                UARDomainName = GetStringOrNull(reader["UARDomainName"]),
                                UATDatabaseIp = GetStringOrNull(reader["UATDatabaseIp"]),
                                UATDatabaseDomain = GetStringOrNull(reader["UATDatabaseDomain"]),
                                ExternalAPIName = GetStringOrNull(reader["ExternalAPIName"]),
                                ExternalAPIEndpoint = GetStringOrNull(reader["ExternalAPIEndpoint"]),
                                APIDetailsAll = GetStringOrNull(reader["APIDetailsAll"]),
                                AdditionalDetails = GetStringOrNull(reader["AdditionalDetails"]),
                                ProjectFile1 = GetStringOrNull(reader["ProjectFile1"]),
                                ProjectFile1Path = GetStringOrNull(reader["ProjectFile1Path"]),
                                ProjectFile2 = GetStringOrNull(reader["ProjectFile2"]),
                                ProjectFile2Path = GetStringOrNull(reader["ProjectFile2Path"]),
                                CreatedDateTime = (DateTime)(reader["CreatedDateTime"] is DBNull ? (DateTime?)null : Convert.ToDateTime(reader["CreatedDateTime"])),
                                CreatedBy = GetStringOrNull(reader["CreatedBy"]),
                                UpdatedDateTime = reader["UpdatedDateTime"] is DBNull ? (DateTime?)null : Convert.ToDateTime(reader["UpdatedDateTime"]),
                                UpdatedBy = GetStringOrNull(reader["UpdatedBy"])
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

        private string GetStringOrNull(object value)
        {
            return value is DBNull ? string.Empty : value.ToString();
        }

        private int? GetNullableInt(object value)
        {
            return value is DBNull ? (int?)null : Convert.ToInt32(value);
        }
    







    public ActionResult Create()
        {

            IEnumerable<ApplicationOwner> applicationOwners = GetApplicationOwners();
            List<string> activeProjectTypeNames = GetActiveProjectTypeNamesFromSP();

            ViewBag.ApplicationOwners = new SelectList(applicationOwners, "ApplicationOwnerId", "DisplayName");
            ViewBag.ActiveProjectTypes = new SelectList(activeProjectTypeNames);

            return View();
        }

        public List<string> GetActiveProjectTypeNamesFromSP()
        {
            List<string> activeProjectTypeNames = new List<string>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("USP_GetActiveProjectTypes", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            activeProjectTypeNames.Add(reader["Name"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception
            }

            return activeProjectTypeNames;
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
                                Name = reader["Name"].ToString(),
                                DisplayName = reader["ApplicationOwnerId"] + " - " + reader["Name"].ToString()
                            };
                            applicationOwners.Add(owner);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
 
                ViewBag.Error = "An error occurred while retrieving application owners: " + ex.Message;
            }

            return applicationOwners;
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Project project, int selectedApplicationOwnerId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("USP_InsertProject", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    // Add parameters and their values from the model
                    command.Parameters.AddWithValue("@ProjectName", project.ProjectName);
                    command.Parameters.AddWithValue("@DomainName", project.DomainName);
                    command.Parameters.AddWithValue("@ProjectDescription", project.ProjectDescription);
                    command.Parameters.AddWithValue("@ProjectType", project.ProjectType);
                    command.Parameters.AddWithValue("@ApplicationOwnerId", selectedApplicationOwnerId);
                    command.Parameters.AddWithValue("@ProductionServerIp", project.ProductionServerIp);
                    command.Parameters.AddWithValue("@ProductionDatabaseIp", project.ProductionDatabaseIp);
                    command.Parameters.AddWithValue("@ProductionDatabaseDomain", project.ProductionDatabaseDomain);
                    command.Parameters.AddWithValue("@UATServerIp", project.UATServerIp);
                    command.Parameters.AddWithValue("@UARDomainName", project.UARDomainName);
                    command.Parameters.AddWithValue("@UATDatabaseIp", project.UATDatabaseIp);
                    command.Parameters.AddWithValue("@UATDatabaseDomain", project.UATDatabaseDomain);
                    command.Parameters.AddWithValue("@ExternalAPIName", project.ExternalAPIName);
                    command.Parameters.AddWithValue("@ExternalAPIEndpoint", project.ExternalAPIEndpoint);
                    command.Parameters.AddWithValue("@APIDetailsAll", project.APIDetailsAll);

                    command.Parameters.AddWithValue("@CreatedBy", "Admin");

                    // Execute the stored procedure
                    command.ExecuteNonQuery();

                    TempData["SuccessMessage"] = "Project Inserted successfully.";
                    return RedirectToAction("Index", "Project");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "An error occurred: " + ex.Message;
            }

            return View();
        }




        public ActionResult Edit(int id)
        {
            Project projectToUpdate = GetProjectById(id);
            if (projectToUpdate == null)
            {
                return HttpNotFound();
            }
            IEnumerable<ApplicationOwner> applicationOwners = GetApplicationOwners();
            ViewBag.ApplicationOwners = new SelectList(applicationOwners, "ApplicationOwnerId", "DisplayName");

            ViewBag.ActiveProjectTypes = new SelectList(GetActiveProjectTypeNamesFromSP());

            return View(projectToUpdate);
        }


        private Project GetProjectById(int projectId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("USP_GetProjectUpdateByProjectId", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ProjectId", projectId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Project project = new Project
                            {
                                ProjectId = (int)reader["ProjectId"],
                                ProjectName = GetStringOrNull(reader["ProjectName"]),
                                DomainName = GetStringOrNull(reader["DomainName"]),
                                ProjectDescription = GetStringOrNull(reader["ProjectDescription"]),
                                //ProjectType = GetStringOrNull(reader["ProjectType"]),
                                //ApplicationOwnerId = (int)reader["ApplicationOwnerId"],
                                ProductionServerIp = GetStringOrNull(reader["ProductionServerIp"]),
                                ProductionDatabaseIp = GetStringOrNull(reader["ProductionDatabaseIp"]),
                                ProductionDatabaseDomain = GetStringOrNull(reader["ProductionDatabaseDomain"]),
                                UATServerIp = GetStringOrNull(reader["UATServerIp"]),
                                UARDomainName = GetStringOrNull(reader["UARDomainName"]),
                                UATDatabaseIp = GetStringOrNull(reader["UATDatabaseIp"]),
                                UATDatabaseDomain = GetStringOrNull(reader["UATDatabaseDomain"]),
                                ExternalAPIName = GetStringOrNull(reader["ExternalAPIName"]),
                                ExternalAPIEndpoint = GetStringOrNull(reader["ExternalAPIEndpoint"]),
                                APIDetailsAll = GetStringOrNull(reader["APIDetailsAll"]),
                                AdditionalDetails = GetStringOrNull(reader["AdditionalDetails"]),
                                ProjectFile1 = GetStringOrNull(reader["ProjectFile1"]),
                                ProjectFile1Path = GetStringOrNull(reader["ProjectFile1Path"]),
                                ProjectFile2 = GetStringOrNull(reader["ProjectFile2"]),
                                ProjectFile2Path = GetStringOrNull(reader["ProjectFile2Path"])
                            };
                            return project;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Project project, int selectedApplicationOwnerId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("USP_UpdateProject", connection);
                    command.CommandType = CommandType.StoredProcedure;


                    // Add parameters and their values from the model
                    command.Parameters.AddWithValue("@ProjectId", project.ProjectId);
                    command.Parameters.AddWithValue("@ProjectName", project.ProjectName);
                    command.Parameters.AddWithValue("@DomainName", project.DomainName);
                    command.Parameters.AddWithValue("@ProjectDescription", project.ProjectDescription);
                    command.Parameters.AddWithValue("@ProjectType", project.ProjectType);
                    command.Parameters.AddWithValue("@ApplicationOwnerId", selectedApplicationOwnerId);
                    command.Parameters.AddWithValue("@ProductionServerIp", project.ProductionServerIp);
                    command.Parameters.AddWithValue("@ProductionDatabaseIp", project.ProductionDatabaseIp);
                    command.Parameters.AddWithValue("@ProductionDatabaseDomain", project.ProductionDatabaseDomain);
                    command.Parameters.AddWithValue("@UATServerIp", project.UATServerIp);
                    command.Parameters.AddWithValue("@UARDomainName", project.UARDomainName);
                    command.Parameters.AddWithValue("@UATDatabaseIp", project.UATDatabaseIp);
                    command.Parameters.AddWithValue("@UATDatabaseDomain", project.UATDatabaseDomain);
                    command.Parameters.AddWithValue("@ExternalAPIName", project.ExternalAPIName);
                    command.Parameters.AddWithValue("@ExternalAPIEndpoint", project.ExternalAPIEndpoint);
                    command.Parameters.AddWithValue("@APIDetailsAll", project.APIDetailsAll);
                    command.Parameters.AddWithValue("@UpdatedBy", "Admin");

                    // Execute the stored procedure
                    command.ExecuteNonQuery();

                    // Execute the stored procedure
                    command.ExecuteNonQuery();

                    TempData["SuccessMessage"] = "Project Updated successfully.";
                    return RedirectToAction("Index", "Project");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "An error occurred: ";
            }

            return View("Index");
        }

    }
}