using System;
using System.ComponentModel.DataAnnotations;

namespace JMA.Mapping.ViewModels
{
    public class ClaimFormViewModel
    {
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Required")]
        [StringLength(30, ErrorMessage = "too many characters")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Required")]
        [StringLength(30, ErrorMessage = "too many characters")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(30, ErrorMessage = "too many characters")]
        public string Addr1 { get; set; }

        [StringLength(30, ErrorMessage = "too many characters")]
        public string Addr2 { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(30, ErrorMessage = "too many characters")]
        public string City { get; set; }

        public string State { get; set; }

        [StringLength(5, MinimumLength = 5)]
        public string Zip { get; set; }

        public bool IsForeign { get; set; }

        [Display(Name = "Province")]
        [StringLength(30, ErrorMessage = "too many characters")]
        public string FProv { get; set; }

        [Display(Name = "Postal Code")]
        [StringLength(14, ErrorMessage = "too many characters")]
        public string Fzip { get; set; }

        [Display(Name = "Country")]
        public string FCountry { get; set; }

        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "not a valid email")]
        [StringLength(255, ErrorMessage = "too many characters")]
        public string Email { get; set; }

        [Display(Name = "Phone (Home)")]
        [Required(ErrorMessage = "Required")]
        [StringLength(20, ErrorMessage = "too many characters")]
        public string PhoneHome { get; set; }

        [Display(Name = "Phone (Work)")]
        [StringLength(20, ErrorMessage = "too many characters")]
        public string PhoneWork { get; set; }

        public bool IAgree { get; set; }

        //public string ClaimID { get; set; }

        public string Error { get; set; }
    }
}