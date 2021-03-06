using Dapper;
using FreeCourse.Shared.Dtos;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Service.Discount.Services
{
    public class DiscountService : IDiscountService
    {

        private readonly IConfiguration _configuration;
        private readonly IDbConnection _dbConnection;
        public DiscountService(IConfiguration configuration)
        {
            _configuration = configuration;
            _dbConnection = new NpgsqlConnection(_configuration.GetConnectionString("PostgreSql"));
        }

        public async Task<Response<NoContent>> Delete(int id)
        {
            var status = await _dbConnection.ExecuteAsync("DELETE FROM discount where id=@Id", new { Id = id });
            return status > 0 ? Response<NoContent>.Success(204) : Response<NoContent>.Fail("Discount Not Found", 404);
        }

        public async Task<Response<List<Model.Discount>>> GetAll()
        {
            var discounts = await _dbConnection.QueryAsync<Model.Discount>("Select * from discount");

            return Response<List<Model.Discount>>.Success(discounts.ToList(), 200);
        }

        public async Task<Response<Model.Discount>> GetByCodeAndUserId(string code, string userId)
        {
            var discounts = await _dbConnection.QueryAsync<Model.Discount>("Select * from discount where userid=@UserId and code=@Code",new { UserId=userId,Code=code});

            var hasDiscount = discounts.FirstOrDefault();

            if (hasDiscount==null)
            {
                return Response<Model.Discount>.Fail("Discount Not Found", 404);
            }
            return Response<Model.Discount>.Success(hasDiscount, 200);
        }

        public async Task<Response<Model.Discount>> GetById(int id)
        {
            var discount =( await _dbConnection.QueryAsync<Model.Discount>("Select * from discount where id=@Id",new {Id=id})).SingleOrDefault();
            if (discount==null)
            {
                return Response<Model.Discount>.Fail("Discount not found", 404);
            }
            return Response<Model.Discount>.Success(discount, 200);
        }

        public async Task<Response<NoContent>> Save(Model.Discount discount)
        {
            var saveStatus = await _dbConnection.ExecuteAsync("INSERT INTO discount (userid,rate,code) VALUES(@UserId,@Rate,@Code)",discount);
            if (saveStatus>0)
            {
                return Response<NoContent>.Success(204);
            }
            return Response<NoContent>.Fail("an error occured while adding", 500);
        }

        public async Task<Response<NoContent>> Update(Model.Discount discount)
        {
            var status = await _dbConnection.ExecuteAsync("UPDATE discount SET userid=@UserId,code=@Code,rate=@Rate WHERE id=@Id", new
            {  //Burda direkt olarak discount objesini de gönderebilirdik, o zaman otomatik maplerdi 
                Id = discount.Id,   
                UserId = discount.UserId,
                Rate = discount.Rate,
                Code = discount.Code
            });
            if (status>0)
            {
                return Response<NoContent>.Success(204);
            }
            return Response<NoContent>.Fail("Discount Not Found", 404);
        }
    }
}
