using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonWorkforceMVC.Models.ViewModels
{
    public class EmployeeTrainingsCount
    {
        public String FirstName { get; set; }
        public string LastName { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int TrainingsCount { get; set; }
    }
}
