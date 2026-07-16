using FluentValidation;
using SoftOne.Models.Requests;

namespace SoftOne.Validators;

public class TaskUpdateRequestValidator : AbstractValidator<TaskUpdateRequest>
{
    public TaskUpdateRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Id must be greater than zero.");

        RuleFor(x => x.RowVersion)
            .NotEmpty().WithMessage("RowVersion is required.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title must be at most 200 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Description must be at most 2000 characters.");

        RuleFor(x => x.Priority)
            .InclusiveBetween(1, 5).WithMessage("Priority must be between 1 and 5.");
    }
}
