using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using BangazonWorkforceMVC.Models;

namespace BangazonWorkforceMVC.Models
{
    public class ComputerEmployee
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }

        public int ComputerId { get; set; }
    }
}
