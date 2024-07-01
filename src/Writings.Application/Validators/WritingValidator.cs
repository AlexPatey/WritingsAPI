using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Writings.Application.Models;
using Writings.Application.Repositories.Interfaces;

namespace Writings.Application.Validators
{
    public class WritingValidator : AbstractValidator<Writing>
    {
        public WritingValidator()
        {
            RuleFor(w => w.Id)
                .NotEmpty();

            RuleFor(w => w.Title)
                .NotEmpty()
                .MaximumLength(255);

            RuleFor(w => w.Body)
                .NotEmpty();

            RuleFor(w => w.Type)
                .NotEmpty();

            RuleFor(w => w.YearOfCompletion)
                .Must(ValidateYearOfCompletion)
                .WithMessage($"Year of completion must be less than or equal to {DateTimeOffset.Now.Year}");
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
