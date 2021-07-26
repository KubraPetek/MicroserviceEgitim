using FreeCourse.Services.FakePayment.Models;
using FreeCourse.Shared.ControllerBases;
using FreeCourse.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;


namespace FreeCourse.Services.FakePayment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FakePaymentController : CustomBaseController
    {
        [HttpPost]
        public IActionResult RecievePayment(PaymentDto paymentDto )
        {
            //paymentDto ile ödeme gerçekleştirmek lazım 
            return CreateActionResultInstance<NoContent>(Response<NoContent>.Success(200));
        }
    }
}
