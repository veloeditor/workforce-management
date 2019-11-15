using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using BangazonWorkforceMVC.Models;

namespace BangazonWorkforceMVC.Models
{
    public class Employee 
    {
        public int Id { get; set; }
        [Display(Name = "First Name")]
        [Required]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; }
        [Required]
        public int DepartmentId { get; set; }
        public bool IsSupervisor { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Department Department { get; set; } = new Department();
        public Computer Computer { get; set; }
    }
}