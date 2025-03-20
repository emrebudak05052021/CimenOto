using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace cimenOtoV11.Models
{
    public class OdemeModeli
    {
        [Required(ErrorMessage = "Kart sahibi adı zorunludur.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Kart sahibi adı 3 ile 50 karakter arasında olmalıdır.")]
        [RegularExpression(@"^[a-zA-ZÇçĞğİıÖöŞşÜü\s]+$", ErrorMessage = "Kart sahibi adı sadece harflerden oluşmalıdır.")]
        public string KartSahibiAdi { get; set; }

        [Required(ErrorMessage = "TC Kimlik Numarası zorunludur.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "TC Kimlik Numarası 11 haneli olmalıdır.")]
        [RegularExpression(@"^[1-9][0-9]{10}$", ErrorMessage = "Geçerli bir TC Kimlik Numarası giriniz.")]
        public string tc { get; set; }

        [Required(ErrorMessage = "Kart numarası zorunludur.")]
        [StringLength(16, MinimumLength = 16, ErrorMessage = "Kart numarası 16 haneli olmalıdır.")]
        [RegularExpression(@"^\d{16}$", ErrorMessage = "Kart numarası sadece rakamlardan oluşmalıdır.")]
        public string KartNumarasi { get; set; }

        [Required(ErrorMessage = "Son kullanma ayı zorunludur.")]
        [RegularExpression(@"^(0[1-9]|1[0-2])$", ErrorMessage = "Geçerli bir ay giriniz (01-12).")]
        public string SonKullanmaAyi { get; set; }

        [Required(ErrorMessage = "Son kullanma yılı zorunludur.")]
        [RegularExpression(@"^\d{4}$", ErrorMessage = "Geçerli bir yıl giriniz (örn: 2025).")]
        public string SonKullanmaYili { get; set; }

        [Required(ErrorMessage = "Güvenlik kodu (CVV) zorunludur.")]
        [RegularExpression(@"^\d{3,4}$", ErrorMessage = "Güvenlik kodu 3 veya 4 haneli olmalıdır.")]
        public string GuvenlikKodu { get; set; }

        [Required(ErrorMessage = "Ödeme tutarı zorunludur.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Tutar 0'dan büyük olmalıdır.")]
        public decimal Tutar { get; set; }

        [Required(ErrorMessage = "Sözleşmeyi kabul etmelisiniz.")]
        [Range(typeof(bool), "true", "true", ErrorMessage = "Ödeme yapmak için sözleşmeyi kabul etmelisiniz.")]
        public bool SozlesmeKabul { get; set; }
    }
}