using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonWorkforceMVC.Models
{
    public class TrainingProgram
    {
        public int Id { get; set; }
        [Display(Name = "Program Name")]
        public string Name { get; set; }
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }
        [Display(Name = "Max Capacity")]
        public int MaxAttendees { get; set; }
        [Display(Name = "Employees Enrolled")]
        public Employee Employee { get; set; } = new Employee();
        public List<Employee> EmployeeList { get; set; } = new List<Employee>();
    }
}