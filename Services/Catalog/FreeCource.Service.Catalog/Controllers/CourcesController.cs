using FreeCource.Service.Catalog.Dtos;
using FreeCource.Service.Catalog.Services;
using FreeCourse.Shared.ControllerBases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCource.Service.Catalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourcesController : CustomBaseController
    {
        private readonly ICourseService _courseService;

        public CourcesController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var response = await _courseService.GetByIdAsync(id);

            return CreateActionResultInstance(response);

        }
        //[HttpGet("{userId}")]  -->Bu şekilde yazarsak GetById ile karışacak
        [Route("/api/[controller]/GetByUserId/{userId}")]//daha spesifik bir route'lama 
        public async Task<IActionResult> GetByUserId(string userId)
        {
            var response = await _courseService.GetByUserIdAsync(userId);

            return CreateActionResultInstance(response);

        }
        public async Task<IActionResult> GetAll()
        {
            var response = await _courseService.GetAllAsnyc();

            return CreateActionResultInstance(response);

        }
        [HttpPost]
        public async Task<IActionResult> Create(CourseCreateDto courseCreateDto)
        {
            var response = await _courseService.CreateAsync(courseCreateDto);

            return CreateActionResultInstance(response);

        }
        [HttpPut]
        public async Task<IActionResult> Update(CourseUpdateDto courseUpdateDto)
        {
            var response = await _courseService.UpdateAsync(courseUpdateDto);

            return CreateActionResultInstance(response);

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var response = await _courseService.DeleteAsync(id);

            return CreateActionResultInstance(response);

        }
    }
}
