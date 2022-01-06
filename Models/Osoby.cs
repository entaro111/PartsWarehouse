//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PartsWarehouse.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Osoby
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Osoby()
        {
            this.Wydania = new HashSet<Wydania>();
        }
    
        public int Id_Osoby { get; set; }
        [Required(ErrorMessage = "Podaj imi�")]
        [StringLength(50, ErrorMessage = "Maksymalnie 50 znak�w")]
        public string Imie { get; set; }
        [Required(ErrorMessage = "Podaj nazwisko")]
        [StringLength(50, ErrorMessage = "Maksymalnie 50 znak�w")]
        public string Nazwisko { get; set; }
        public Nullable<int> Id_Dzial { get; set; }
    
        public virtual Dzialy Dzialy { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Wydania> Wydania { get; set; }
    }
}
