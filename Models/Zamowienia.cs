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
    
    public partial class Zamowienia
    {
        public int Id_Zamowienia { get; set; }
        public System.DateTime Data_zamowienia { get; set; }
        public bool Realizacja { get; set; }
        public Nullable<int> Id_Kartoteki { get; set; }
        public Nullable<int> Ilosc { get; set; }
    
        public virtual Kartoteki Kartoteki { get; set; }
    }
}
