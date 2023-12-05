using FluentValidation;
using GraphQLDemo.API.Schema.Mutations;

namespace GraphQLDemo.API.Validators
{
    public class CourseTypeInputValidator : AbstractValidator<CourseInput>
    {
        public CourseTypeInputValidator()
        {
            RuleFor(c => c.Name)
                .MinimumLength(3).MaximumLength(50)
                .WithMessage("Course Name Length error")
                .WithErrorCode("NAME_LENGTH_ERROR");
        }
    }
}
