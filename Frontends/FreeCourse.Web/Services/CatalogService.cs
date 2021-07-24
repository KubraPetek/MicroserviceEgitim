using FreeCourse.Shared.Dtos;
using FreeCourse.Web.Models;
using FreeCourse.Web.Models.Catalog;
using FreeCourse.Web.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FreeCourse.Web.Services
{
    public class CatalogService : ICatalogService
    {

        private readonly HttpClient _httpClient;

        public CatalogService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> CreateCourseAsync(CourseCreateInput courseCreateInput)
        {
            var response = await _httpClient.PostAsJsonAsync<CourseCreateInput>("cources", courseCreateInput);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteCourseAsync(string courseId)
        {
            var response = await _httpClient.DeleteAsync($"cources/{courseId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<List<CategoryViewModel>> GetAllCategoryAsync()
        {
            //http:localhost:5000/services/catalog/categories
            var response = await _httpClient.GetAsync("categories");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var responseSuccess = await response.Content.ReadFromJsonAsync<Response<List<CategoryViewModel>>>();
            return responseSuccess.Data;
        }

        public async Task<List<CourseViewModel>> GetAllCourseAsync()
        {
            //http:localhost:5000/services/catalog/cources
            var response = await _httpClient.GetAsync("cources");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var responseSuccess = await response.Content.ReadFromJsonAsync<Response<List<CourseViewModel>>>();
            return responseSuccess.Data;
        }

        public async Task<List<CourseViewModel>> GetAllCourseByUserIdAsync(string userId)
        {
            //http:localhost:5000/services/catalog/ -->  [controller]/GetByUserId/{userId}
            var response = await _httpClient.GetAsync($"cources/GetByUserId/{userId}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var responseSuccess = await response.Content.ReadFromJsonAsync<Response<List<CourseViewModel>>>();
            return responseSuccess.Data;
        }

        public async Task<CourseViewModel> GetByCourseId(string courseId)
        {
            //http:localhost:5000/services/catalog/ -->  {id}
            var response = await _httpClient.GetAsync($"cources/{courseId}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var responseSuccess = await response.Content.ReadFromJsonAsync<Response<CourseViewModel>>();
            return responseSuccess.Data;
        }

        public async Task<bool> UpdateCourseAsync(CourseUpdateInput courseUpdateInput)
        {
            var response = await _httpClient.PutAsJsonAsync<CourseUpdateInput>("cources", courseUpdateInput);
            return response.IsSuccessStatusCode;
        }
    }
}
