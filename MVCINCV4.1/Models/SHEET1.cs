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
    
    public partial class SHEET1
    {
        [Key]
        public int RID { get; set; }
         [DisplayName("PART NO")]
        public string PART_NO { get; set; }
         [DisplayName("PART NAME")]
        public string PARTI { get; set; }
        public string MRP { get; set; }
         [DisplayName("ITEM GROUP")]
        public string GROP { get; set; }
         [DisplayName("ITEM TYPE")]
        public string CATE { get; set; }
         [DisplayName("DEALER CODE")]
        public string DPCODE { get; set; }
        public string lmodi { get; set; }
         [DisplayName("TAX RATE")]
        public string TRATE { get; set; }
        public string rid1 { get; set; }
         [DisplayName("UNIT")]
        public string unit { get; set; }
        public string AEDT { get; set; }
    }
}
