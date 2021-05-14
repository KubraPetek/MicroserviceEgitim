using AutoMapper;
using FreeCource.Service.Catalog.Dtos;
using FreeCource.Service.Catalog.Models;
using FreeCource.Service.Catalog.Settings;
using FreeCourse.Shared.Dtos;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FreeCource.Service.Catalog.Services
{
    public class CategoryService:ICategoryService
    {
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMapper _mapper;

        public CategoryService(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);//Hangi sunucuya (client) bağlanacağımız bilgisini veriyoruz
            var database = client.GetDatabase(databaseSettings.DatabaseName);//hangi veri tabanına  bağlanacağımız bilgisini veriyoruz 
            
            _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);//Hangi collectiona bağlanacağımız bilgisini veriyoruz 
            _mapper = mapper;
        }
        public async Task<Response<List<CategoryDto>>> GetAllAsnyc()
        {
            var categories = await _categoryCollection.Find(category => true).ToListAsync();//Tüm dataları al 
            return Response<List<CategoryDto>>.Success(_mapper.Map<List<CategoryDto>>(categories), 200);
        }
        public async Task<Response<CategoryDto>> CreateAsync(CategoryDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
          await _categoryCollection.InsertOneAsync(category);
            return Response<CategoryDto>.Success(_mapper.Map<CategoryDto>(category),200);//200 -OK 

        }
        public async Task<Response<CategoryDto>> GetByIdAsync(string id)
        {
            var category = await _categoryCollection.Find<Category>(x => x.Id == id).FirstOrDefaultAsync();
            if (category == null)
            {
                return Response<CategoryDto>.Fail("Category not found", 404);
            }
            return Response<CategoryDto>.Success(_mapper.Map<CategoryDto>(category), 200);
            
        }
    }
}
