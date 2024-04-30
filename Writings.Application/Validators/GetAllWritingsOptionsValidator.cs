using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Writings.Application.Models;

namespace Writings.Application.Validators
{
    public class GetAllWritingsOptionsValidator : AbstractValidator<GetAllWritingsOptions>
    {
        public GetAllWritingsOptionsValidator()
        {
            RuleFor(o => o.Title)
                .Must(ValidateTitle)
                .WithMessage($"Title length must be less than or equal to 255 characters");

            RuleFor(o => o.YearOfCompletion)
                .Must(ValidateYearOfCompletion)
                .WithMessage($"Year of completion must be less than or equal to {DateTimeOffset.Now.Year}");
        }

        private bool ValidateTitle(string? title)
        {
            if (title is null)
            {
                return true;
            }

            return title.Length <= 255;
        }

        private bool ValidateYearOfCompletion(int? yearOfCompleteion)
        {
            if (yearOfCompleteion is null)
            {
                return true;
            }

            return yearOfCompleteion <= DateTimeOffset.Now.Year;
        }
    }
}
