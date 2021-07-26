using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Web.Models.Orders
{
    public class OrderViewModel
    {
        public int Id { get; set; }

        public DateTime CreatedDate { get; set; }

        //public Address Address { get; set; } --Ödeme geçmişinde adres alanına ihtiyaç olmadığından dolayı kaldırıldı
        public string BuyerId { get; set; }

        public List<OrderItemViewModel> OrderItems { get; set; }
    }
}
