using BangazonWorkforceMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonWorkforceMVC.Models.ViewModels
{
    public class ComputerEmployeeViewModel
    {
        public Employee Employee { get; set; }
        public Computer Computer { get; set; }
    }
}
