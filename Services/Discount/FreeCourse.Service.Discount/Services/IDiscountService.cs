using FreeCourse.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Service.Discount.Services
{
    public interface IDiscountService
    {
        Task<Response<List<Model.Discount>>> GetAll();

        Task<Response<List<Model.Discount>>> GetById(int id);
        Task<Response<List<NoContent>>> Save(Model.Discount discount);

        Task<Response<List<NoContent>>> Update(Model.Discount discount);

        Task<Response<List<NoContent>>> Delete(int id);

        Task<Response<List<Model.Discount>>> GetByCodeAndUserId(string code, string userId);

    }
}
