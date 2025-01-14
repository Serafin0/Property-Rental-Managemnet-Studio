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
    
    public partial class Apartment
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Apartment()
        {
            this.Leases = new HashSet<Leas>();
        }
    
        public int ApartmentNumber { get; set; }
        public int Bedroom { get; set; }
        public int Bathroom { get; set; }
        public decimal Squarefeet { get; set; }
        public decimal Price { get; set; }
        public int Status { get; set; }
        public int BuildingId { get; set; }
        public int ManagerId { get; set; }
    
        public virtual Building Building { get; set; }
        public virtual PropertyManager PropertyManager { get; set; }
        public virtual Status Status1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Leas> Leases { get; set; }
    }
}
