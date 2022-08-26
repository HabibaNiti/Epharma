using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace MedicineEshop.ViewModel
{
    public class AuthModel
    {
        [Required]
        public string EmployeeId { get; set; }

        [Display(Name = "Old Password")]
        [DataType(DataType.Password)]
        public string EmployeePassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        [StringLength(15, MinimumLength = 6, ErrorMessage = "Minimum 6 characters required")]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Re-type password")]
        [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
        public string RetypePassword { get; set; }

        [Required]
        [RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9]+)*\\.([a-z]{2,4})$", ErrorMessage = "Invalid email format.")]
        [Display(Name = "Email")]
        public string EmployeeEmail { get; set; }

        [Required]
        [Display(Name = "Contact Number")]
        [StringLength(15, MinimumLength = 11, ErrorMessage = "Enter Valid Contact Number")]
        public string EmployeePhone { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string EmployeeName { get; set; }

        [Display(Name = "Role")]
        public string EmployeeRole { get; set; }

        [Display(Name = "Designation")]
        public string EmployeeDesignation { get; set; }

        public string EmployeeImage { get; set; }
        public HttpPostedFileBase Image { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string EmployeeArea { get; set; }
        

        public bool Message { get; set; }

        public string UpdateBy { get; set; }
    }
}