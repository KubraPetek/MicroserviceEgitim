﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Web.Models.Catalog
{
    public class CourseUpdateInput
    {
        public string Id { get; set; }
        [Display(Name = "Kurs İsmi")]
        [Required]
        public string Name { get; set; }
        [Display(Name = "Kurs Fiyatı")]
        [Required]
        public decimal Price { get; set; }
        [Display(Name = "Kurs Açıklama")]
        [Required]
        public string Description { get; set; }
        public string UserId { get; set; }
        public FeatureViewModel Feature { get; set; }

        [Display(Name = "Kurs Kategori")]
        [Required]
        public string CategoryId { get; set; }

        public string Picture { get; set; }

        [Display(Name = "Kurs Resmi")]
        public IFormFile PhotoFormFile { get; set; }
    }
}
