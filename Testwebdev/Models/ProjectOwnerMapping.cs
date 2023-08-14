using System;
using System.ComponentModel.DataAnnotations;

namespace Testwebdev.Models
{
    public class ProjectOwnerMapping
    {
        public int MappingId { get; set; }

        [Required(ErrorMessage = "Project is required.")]
        public int ProjectId { get; set; }

        [Required(ErrorMessage = "Application owner is required.")]
        public int ApplicationOwnerId { get; set; }

        [Required(ErrorMessage = "Start date is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        //[GreaterThan(nameof(StartDate), ErrorMessage = "End date must be greater than Start date.")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        [Display(Name = "Deleted")]
        public bool IsDeleted { get; set; }
        public string ProjectName { get; set; }
        public string ApplicationOwnerName { get; set; }


        public DateTime CreatedDateTime { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public string UpdatedBy { get; set; }
    }
}
