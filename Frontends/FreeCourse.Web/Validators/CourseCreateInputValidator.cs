using FluentValidation;
using FreeCourse.Web.Models.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Web.Validators
{
    public class CourseCreateInputValidator:AbstractValidator<CourseCreateInput>
    {

        public CourseCreateInputValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("İsim alanı boş olamaz");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Açıklama alanı boş olamaz");
            RuleFor(x => Convert.ToDecimal(x.Feature.Duration)).InclusiveBetween(1, int.MaxValue).WithMessage("Süre alanı boş yada 0 olamaz");
            RuleFor(x => Convert.ToDecimal(x.Price)).NotEmpty().WithMessage("Fiyat alanı boş olamaz").ScalePrecision(2,6).WithMessage("Hatalı para formatı");//Virgülden sonra 2 toplamda 6 karakter olabilr 
            RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Kategori alanı seçilmelidir");
        }

    }
}
