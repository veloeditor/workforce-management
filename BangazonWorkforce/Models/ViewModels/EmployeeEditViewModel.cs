using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonWorkforceMVC.Models.ViewModels
{
    public class EmployeeEditViewModel
    {
        public List<Department> departments { get; set; } = new List<Department>();

        public List<Computer> computers { get; set; } = new List<Computer>();

        public ComputerEmployee ComputerEmployee = new ComputerEmployee();
        public Employee employee { get; set; } = new Employee();

        public List<SelectListItem> DepartmentOptions
        {
            get
            {
                if (departments == null) return null;
                return departments
                    .Select(d => new SelectListItem(d.Name, d.Id.ToString()))
                    .ToList();
            }
        }

        public List<SelectListItem> ComputerOptions
        {
            get
            {
                if (computers == null) return null;
                return computers
                    .Select(c => new SelectListItem(c.Make, c.Id.ToString()))
                    .ToList();
            }
        }

    }
}
