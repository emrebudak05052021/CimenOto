//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace cimenOtoV11.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_SİparisDetay
    {
        public int siparisDetayID { get; set; }
        public Nullable<int> siparisID { get; set; }
        public Nullable<int> parcaID { get; set; }
        public Nullable<int> adres { get; set; }
        public Nullable<byte> miktar { get; set; }
        public Nullable<decimal> birimFiyat { get; set; }
        public Nullable<decimal> toplamTutar { get; set; }
    
        public virtual tbl_Adres tbl_Adres { get; set; }
        public virtual tbl_AracParca tbl_AracParca { get; set; }
        public virtual tbl_Siparisler tbl_Siparisler { get; set; }
    }
}
