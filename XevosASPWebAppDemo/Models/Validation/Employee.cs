using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace XevosASPWebAppDemo.Models
{
    [MetadataType(typeof(EmployeeMetadata))]
    public partial class Employee
    {
    }

    public class EmployeeMetadata
    {        
        [Required(ErrorMessage = "Please enter First Name!")]
        public string Jmeno { get; set; }
        [Required(ErrorMessage = "Please enter Last Name!")]
        public string Prijmeni { get; set; }
        [Required(ErrorMessage = "Please enter Date of work begin!")]
        public DateTime Date { get; set; }
    }
}