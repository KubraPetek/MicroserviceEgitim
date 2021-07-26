using FreeCourse.Web.Models.Orders;
using FreeCourse.Web.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Web.Services
{
    public class OrderService : IOrderService
    {
        public Task<OrderCreatedViewModel> CreateOrder(CheckOutInfoInput checkOutInfoInput)
        {
            throw new NotImplementedException();
        }

        public Task<List<OrderViewModel>> GetOrder()
        {
            throw new NotImplementedException();
        }

        public Task SuspendOrder(CheckOutInfoInput checkOutInfoInput)
        {
            throw new NotImplementedException();
        }
    }
}
