using FreeCourse.Web.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Web.Services.Interfaces
{
    public interface IOrderService
    {
        /// <summary>
        /// Senkron iletişim  ->Sipariş direkt olarak order servise istek yapılacak
        /// </summary>
        /// <param name="checkOutInfoInput"></param>
        /// <returns></returns>
        Task<OrderCreatedViewModel> CreateOrder(CheckOutInfoInput checkOutInfoInput);

        /// <summary>
        /// Asenkron iletişim-->sipariş bilgileri rabbit mq ya gönderilecek
        /// </summary>
        /// <param name="checkOutInfoInput"></param>
        /// <returns></returns>

        Task<OrderSuspendViewModel> SuspendOrder(CheckOutInfoInput checkOutInfoInput);

        Task<List<OrderViewModel>> GetOrder();
    }
}
