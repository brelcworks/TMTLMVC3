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
    
    public partial class CUST
    {
        [Key]
        public int CID { get; set; }
        public string CUST1 { get; set; }
        public string SECTOR { get; set; }
        public string DPCODE { get; set; }
    }
}