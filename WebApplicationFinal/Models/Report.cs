//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplicationFinal.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Report
    {
        public int ReportId { get; set; }
        public int ManagerId { get; set; }
        public int AdminId { get; set; }
        public string Report1 { get; set; }
    
        public virtual PropertyManager PropertyManager { get; set; }
        public virtual PropertyOwner PropertyOwner { get; set; }
    }
}