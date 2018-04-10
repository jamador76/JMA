using System;
using System.ComponentModel.DataAnnotations;

namespace JMA.Web.Models
{
    public sealed class IndexViewModel
    {
        [Required(ErrorMessage = "Required")]
        [StringLength(8, MinimumLength = 8)]
        public string Claim8 { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(6, MinimumLength = 6)]
        public string PIN { get; set; }

        public string Error { get; set; }
    }
}