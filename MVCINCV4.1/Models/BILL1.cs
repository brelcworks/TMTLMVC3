//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MVCINCV4._1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    
    public partial class BILL1
    {
        [Key]
        public int BID { get; set; }
        [Display(Name = "BILL DATE")]
        [Required]
        public System.DateTime BDATE { get; set; }
        [Display(Name = "BILL NO")]
        public string BNO { get; set; }
        [Display(Name = "CUSTOMER")]
        public string CUST { get; set; }
        [Display(Name = "SITE NAME")]
        public string SNAME { get; set; }
        [Display(Name = "GRAND TOTAL")]
        public Nullable<decimal> GTOT { get; set; }
        public Nullable<decimal> TOTAL { get; set; }
        public Nullable<decimal> PAYMENT { get; set; }
        public string SECTOR { get; set; }
        public string ADDRESS { get; set; }
        [Display(Name = "ROUND OFF")]
        public Nullable<decimal> ROUND { get; set; }
        [Display(Name = "NET TOTAL")]
        public Nullable<decimal> NTOT { get; set; }
        [Display(Name = "TAX VALUE")]
        public Nullable<decimal> TVAL { get; set; }
        public string USER1 { get; set; }
        [Display(Name = "BILL MODE")]
        public string MODE { get; set; }
        [Display(Name = "VAT NO")]
        public string VNO { get; set; }
        public Nullable<decimal> CBILL { get; set; }
        public Nullable<decimal> BAPAY { get; set; }
        public string BST { get; set; }
        public string SSTA { get; set; }
        public string DPCODE { get; set; }
        public string LMODI { get; set; }
        public string BID1 { get; set; }
        public string AEDT { get; set; }
        public Nullable<decimal> BAMT { get; set; }
    }
}
