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
    
    public partial class tbl_Siparisler
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_Siparisler()
        {
            this.MesafeliSatisSozlesmeleri = new HashSet<MesafeliSatisSozlesmeleri>();
            this.tbl_SİparisDetay = new HashSet<tbl_SİparisDetay>();
        }
    
        public int siparisID { get; set; }
        public Nullable<System.Guid> kulID { get; set; }
        public Nullable<System.DateTime> siparisTarihi { get; set; }
        public Nullable<decimal> toplamTutar { get; set; }
        public Nullable<byte> sipDurum { get; set; }
        public Nullable<int> adres { get; set; }
        public Nullable<bool> sozlesmeKabul { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MesafeliSatisSozlesmeleri> MesafeliSatisSozlesmeleri { get; set; }
        public virtual tbl_Adres tbl_Adres { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_SİparisDetay> tbl_SİparisDetay { get; set; }
        public virtual tbl_SiparisDurum tbl_SiparisDurum { get; set; }
        public virtual tbl_User tbl_User { get; set; }
        public virtual tbl_User tbl_User1 { get; set; }
    }
}
