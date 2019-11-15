using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using BangazonWorkforceMVC.Models;

namespace BangazonWorkforceMVC.Models
{
    public class Department
    {
        public int Id { get; set; }
        [Display(Name = "Department Name")]
        [Required]
        public string Name { get; set; }

        [Required]
        public int Budget { get; set; }
        public List<Employee> employees { get; set; } = new List<Employee>();
    }
}