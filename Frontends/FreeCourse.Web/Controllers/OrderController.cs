using FreeCourse.Web.Models.Orders;
using FreeCourse.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IBasketService _basketService;
        private readonly IOrderService _orderService;

        public OrderController(IBasketService basketService, IOrderService orderService)
        {
            _basketService = basketService;
            _orderService = orderService;
        }

        public async Task<IActionResult> Checkout()
        {
            var basket = await _basketService.Get();
            ViewBag.basket = basket;

            return View(new CheckOutInfoInput());
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CheckOutInfoInput checkOutInfoInput)
        {
            //1.yol-->senkron iletişm 
            var orderSuspend = await _orderService.SuspendOrder(checkOutInfoInput);

            //2.yol -->asenkron iletişim
            //var orderStatus = await _orderService.CreateOrder(checkOutInfoInput);
            if (!orderSuspend.IsSucces)
            {
                var basket = await _basketService.Get();
                ViewBag.basket = basket;

                //TempData["error"] = orderStatus.Error;
                ViewBag.error = orderSuspend.Error;
                return View();
            }


            // return RedirectToAction(nameof(SuccesfullCheckout), new { orderId = orderStatus.OrderId });  -->1.yol-->senkron iletişm 
            return RedirectToAction(nameof(SuccesfullCheckout), new {orderId=new Random().Next(1,1000) });

        }

        public IActionResult SuccesfullCheckout(int orderId)
        {
            ViewBag.orderId = orderId;
            return View();
        }
    }
}
