using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Service.Discount.Model
{
    [Dapper.Contrib.Extensions.Table("discount")] //PostgreSQL içindeki discount tablosu Discount classına eşitlenecek
    public class Discount
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int Rate { get; set; }
        public string Code { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
