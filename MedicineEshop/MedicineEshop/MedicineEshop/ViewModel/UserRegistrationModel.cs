using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedicineEshop.ViewModel
{
    public class UserRegistrationModel
    {
        [Required]
        [DisplayName("Employee ID")]
        public string EmployeeId { get; set; }

        [Required]
        [DisplayName("Employee Name")]
        public string EmployeeName { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 8, ErrorMessage = "Minimum 8 Characters Required")]
        [DisplayName("Password")]
        public string Password { get; set; }
        
        [DisplayName("Active Status")]
        public bool ActiveStatus { get; set; }
        public string ActiveYN { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [DisplayName("Contact No")]
        public string ContactNo { get; set; }

        //[Required]
        [DisplayName("NID No")]
        public string NID { get; set; }

        [Required]
        [DisplayName("Role Name")]
        public string RoleName { get; set; }

        public string CreateBy { get; set; }

        [DisplayName("Shop Name")]
        public int ShopId { get; set; }

        public string FromDate { get; set; }

        public string ToDate { get; set; }

        public string RadioFor { get; set; }

        public string ReportType { get; set; }

        public List<int> ShopIdList { get; set; }

        public string Shops { get; set; }
    }
}