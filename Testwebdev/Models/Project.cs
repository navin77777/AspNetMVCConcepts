using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Testwebdev.Models
{

    public class Project
    {
        public int ProjectId { get; set; }

        [Required(ErrorMessage = "Project Name is required")]
        [StringLength(200)]
        public string ProjectName { get; set; }

        [Required(ErrorMessage = "Domain Name is required")]
        [StringLength(200)]
        public string DomainName { get; set; }

        [Required(ErrorMessage = "Project Description is required")]
        public string ProjectDescription { get; set; }

        [Required(ErrorMessage = "Project Type is required")]
        [StringLength(200)]
        public string ProjectType { get; set; }

        [Required(ErrorMessage = "Application Owner Id is required")]
        public int ApplicationOwnerId { get; set; }

        [Required(ErrorMessage = "Production Server IP is required")]
        [StringLength(200)]
        public string ProductionServerIp { get; set; }

        [Required(ErrorMessage = "Production Database IP is required")]
        [StringLength(200)]
        public string ProductionDatabaseIp { get; set; }

        [Required(ErrorMessage = "Production Database Domain is required")]
        [StringLength(200)]
        public string ProductionDatabaseDomain { get; set; }

        [Required(ErrorMessage = "UAT Server IP is required")]
        [StringLength(200)]
        public string UATServerIp { get; set; }

        [Required(ErrorMessage = "UAR Domain Name is required")]
        [StringLength(200)]
        public string UARDomainName { get; set; }

        [Required(ErrorMessage = "UAT Database IP is required")]
        [StringLength(200)]
        public string UATDatabaseIp { get; set; }

        [Required(ErrorMessage = "UAT Database Domain is required")]
        [StringLength(200)]
        public string UATDatabaseDomain { get; set; }

        [Required(ErrorMessage = "External API Name is required")]
        [StringLength(200)]
        public string ExternalAPIName { get; set; }

        [Required(ErrorMessage = "External API Endpoint is required")]
        [StringLength(500)]
        public string ExternalAPIEndpoint { get; set; }

        [StringLength(1000)]
        public string APIDetailsAll { get; set; }

        [StringLength(1000)]
        public string AdditionalDetails { get; set; }

        [StringLength(200)]
        public string ProjectFile1 { get; set; }

        [StringLength(500)]
        public string ProjectFile1Path { get; set; }

        [StringLength(200)]
        public string ProjectFile2 { get; set; }

        [StringLength(500)]
        public string ProjectFile2Path { get; set; }

        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Created By is required")]
        [StringLength(100)]
        public string CreatedBy { get; set; }

        public DateTime? UpdatedDateTime { get; set; }
        [StringLength(100)]
        public string UpdatedBy { get; set; }

        public IEnumerable<SelectListItem> ProjectTypeList { get; set; }


    }

}