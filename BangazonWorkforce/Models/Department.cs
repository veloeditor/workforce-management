using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using BangazonWorkforceMVC.Models;

namespace BangazonWorkforceMVC.Models
{
    public class Department
    {   [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        
        public decimal Budget { get; set; }
       
        public int TotalEmployees { get; set; }

        public List<Employee> employees { get; set; } = new List<Employee>();
    }
}