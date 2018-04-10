using System;

namespace JMA.Mapping.DTOs
{
    public class ClaimantDTO
    {
        public string Claim8 { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Addr1 { get; set; }
        public string Addr2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Email { get; set; }
        public string PhoneHome { get; set; }
        public string PhoneWork { get; set; }
        public bool? IsForeign { get; set; }
        public string FProv { get; set; }
        public string FZip { get; set; }
        public string FCountry { get; set; }
        public DateTime? SubmitDate { get; set; }
        public string SubmitSource { get; set; }
        public string IPAddress { get; set; }
        public string SessionID { get; set; }
        public string UserAgentString { get; set; }
    }
}