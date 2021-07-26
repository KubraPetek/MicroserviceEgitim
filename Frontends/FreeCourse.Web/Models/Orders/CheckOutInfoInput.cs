using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Web.Models.Orders
{
    public class CheckOutInfoInput
    {
        [Display(Name ="İl")]
        public string Province { get; private set; }
        [Display(Name = "İlçe")]
        public string District { get; private set; }
        [Display(Name = "Cadde")]
        public string Street { get; private set; }
        [Display(Name = "Posta Kodu")]
        public string ZipCode { get; private set; }
        [Display(Name = "adres")]
        public string Line { get; private set; }
        [Display(Name = "Kart isim soyisim")]
        public string CardName { get; set; }

        [Display(Name = "Kart Numarası")]
        public string CardNumber { get; set; }

        [Display(Name = "Son kullanma tarih (ay,yıl)")]
        public string Expiration { get; set; }
        [Display(Name = "Güvenlik Kodu")]
        public string CVV { get; set; }
        [Display(Name = "Toplam tutar")]
        public decimal TotalPrice { get; set; }
    }
}
