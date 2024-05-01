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
        private static readonly string[] AcceptableSortFields =
        {
            "title",
            "type",
            "yearofcompletion"
        };

        public GetAllWritingsOptionsValidator()
        {
            RuleFor(o => o.Title)
                .Must(ValidateTitle)
                .WithMessage($"Title length must be less than or equal to 255 characters");

            RuleFor(o => o.YearOfCompletion)
                .Must(ValidateYearOfCompletion)
                .WithMessage($"Year of completion must be less than or equal to {DateTimeOffset.Now.Year}");

            RuleFor(o => o.SortField)
                .Must(s => s is null || AcceptableSortFields.Contains(s, StringComparer.OrdinalIgnoreCase))
                .WithMessage("You can only sort by 'title' or 'type' or 'yearofcompletion'");

            RuleFor(o => o.Page)
                .NotEmpty()
                .GreaterThanOrEqualTo(1)
                .WithMessage("Page must be greater than or equal to 1");

            RuleFor(o => o.PageSize)
                .NotEmpty()
                .InclusiveBetween(1, 25)
                .WithMessage("Page Size must be greater than or equal to 1 and less than or equal to 25");
        }

        private bool ValidateTitle(string? title)
        {
            if (title is null)
            {
                return true;
            }

            return title.Length <= 255;
        }

        private bool ValidateYearOfCompletion(int? yearOfCompletion)
        {
            if (yearOfCompletion is null)
            {
                return true;
            }

            return yearOfCompletion <= DateTimeOffset.Now.Year;
        }
    }
}
