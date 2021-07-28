using AutoMapper;
using FreeCource.Service.Catalog.Dtos;
using FreeCource.Service.Catalog.Models;
using FreeCource.Service.Catalog.Settings;
using FreeCourse.Shared.Dtos;
using FreeCourse.Shared.Messages;
using MassTransit;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCource.Service.Catalog.Services
{
    public class CourseService : ICourseService
    {
        private readonly IMongoCollection<Course> _courseCollection;
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMapper _mapper;

        private readonly IPublishEndpoint _publishEndpoint;

        public CourseService(IMapper mapper,IDatabaseSettings databaseSettings, IPublishEndpoint publishEndpoint  )
        {
            var client = new MongoClient(databaseSettings.ConnectionString);//Hangi sunucuya (client) bağlanacağımız bilgisini veriyoruz
            var database = client.GetDatabase(databaseSettings.DatabaseName);//hangi veri tabanına  bağlanacağımız bilgisini veriyoruz 

            _courseCollection = database.GetCollection<Course>(databaseSettings.CourseCollectionName);//Hangi collectiona bağlanacağımız bilgisini veriyoruz 
            _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);//Hangi collectiona bağlanacağımız bilgisini veriyoruz 


            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
        }


        public async Task<FreeCourse.Shared.Dtos.Response<List<CourseDto>>> GetAllAsnyc()
        {
            var courses = await _courseCollection.Find(course => true).ToListAsync();


            if (courses.Any())
            {
                foreach(var course in courses)
                {
                    course.Category = await _categoryCollection.Find<Category>(x => x.Id == course.CategoryId).FirstAsync();
                }
            }
            else
            {
                courses = new List<Course>();
            }
            return FreeCourse.Shared.Dtos.Response<List<CourseDto>>.Success(_mapper.Map<List<CourseDto>>(courses), 200);
        }

        public async Task<FreeCourse.Shared.Dtos.Response<CourseDto>> GetByIdAsync(string id)
        {
            var course = await _courseCollection.Find<Course>(x => x.Id == id).FirstOrDefaultAsync();
            if (course == null)
            {
                return FreeCourse.Shared.Dtos.Response<CourseDto>.Fail("Course not found", 404);
            }
            course.Category = await _categoryCollection.Find<Category>(x => x.Id == course.CategoryId).FirstAsync();
            return FreeCourse.Shared.Dtos.Response<CourseDto>.Success(_mapper.Map<CourseDto>(course), 200);

        }
        public async Task<FreeCourse.Shared.Dtos.Response<List<CourseDto>>> GetByUserIdAsync(string userId)
        {
            var courses = await _courseCollection.Find<Course>(x => x.UserId == userId).ToListAsync();
            if (courses.Any())
            {
                foreach (var course in courses)
                {
                    course.Category = await _categoryCollection.Find<Category>(x => x.Id == course.CategoryId).FirstAsync();
                }
            }
            else
            {
                courses = new List<Course>();
            }
            return FreeCourse.Shared.Dtos.Response<List<CourseDto>>.Success(_mapper.Map<List<CourseDto>>(courses), 200);

        }

        public async Task<FreeCourse.Shared.Dtos.Response<CourseDto>> CreateAsync(CourseCreateDto courseCreateDto)
        {

            var newCourse = _mapper.Map<Course>(courseCreateDto);
            newCourse.CreatedTime = DateTime.Now;


            await _courseCollection.InsertOneAsync(newCourse);
            return FreeCourse.Shared.Dtos.Response<CourseDto>.Success(_mapper.Map<CourseDto>(newCourse), 200);

        }
        public async Task<FreeCourse.Shared.Dtos.Response<NoContent>> UpdateAsync(CourseUpdateDto courseUpdateDto)
        {
            var updateCourse = _mapper.Map<Course>(courseUpdateDto);

            var result = await _courseCollection.FindOneAndReplaceAsync(x => x.Id == courseUpdateDto.Id, updateCourse);
            if (result==null)
            {
                return FreeCourse.Shared.Dtos.Response<NoContent>.Fail("Course not found", 404);

            }

            //değişiklik olabilceke diğer db lere event gönderiyoruz 
            await _publishEndpoint.Publish<CourseNameChangedEvent>(new CourseNameChangedEvent { CourseId=updateCourse.Id,UpdateName=courseUpdateDto.Name });

            return FreeCourse.Shared.Dtos.Response<NoContent>.Success(204);//updated durum kodu 
        }
        public async Task<FreeCourse.Shared.Dtos.Response<NoContent>> DeleteAsync(string id)
        {
            var result = await _courseCollection.DeleteOneAsync(x=>x.Id==id);//Bu metotlar MongoDb.Driver kütüphanesinden geliyor
            if (result.DeletedCount > 0)//0 dan büyük dönerse gerrçekten sildiği anlamına geliyor
            {
                return FreeCourse.Shared.Dtos.Response<NoContent>.Success(204);//updated durum kodu 
            }
            else
            {
                return FreeCourse.Shared.Dtos.Response<NoContent>.Fail("Course not found", 404);//silmek istenilen işlem bulunamadı anlamında 
            }
        }
    }
}
