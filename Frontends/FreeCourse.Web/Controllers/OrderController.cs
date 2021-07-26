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

            var orderStatus = await _orderService.CreateOrder(checkOutInfoInput);
            if (!orderStatus.IsSucces)
            {
                var basket = await _basketService.Get();
                ViewBag.basket = basket;

                //TempData["error"] = orderStatus.Error;
                ViewBag.error = orderStatus.Error;
                return View();
            }

            return RedirectToAction(nameof(SuccesfullCheckout), new { orderId = orderStatus.OrderId });

        }

        public IActionResult SuccesfullCheckout(int orderId)
        {
            ViewBag.orderId = orderId;
            return View();
        }
    }
}
