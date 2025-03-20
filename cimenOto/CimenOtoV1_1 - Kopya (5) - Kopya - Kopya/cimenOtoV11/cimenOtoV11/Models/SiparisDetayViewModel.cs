using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cimenOtoV11.Models
{
    public class SiparisDetayViewModel
    {
        public tbl_SİparisDetay SiparisDetay { get; set; }
        public tbl_Siparisler Siparis { get; set; }
        public tbl_User Musteri { get; set; }
        public tbl_AracParca Parca { get; set; } // List olarak güncellendi

        public List<tbl_AracParca> Parcalar { get; set; } // List olarak güncellendi
    }
}