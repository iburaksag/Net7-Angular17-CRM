using FluentValidation;

namespace CustomerManagement.Application.Users.Commands.Register
{
	public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
		public RegisterCommandValidator()
		{
            RuleFor(u => u.Username)
                .NotEmpty().WithMessage("Username is required.")
                .MaximumLength(15).WithMessage("Username cannot exceed 15 characters.");

            RuleFor(u => u.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MaximumLength(25).WithMessage("Password cannot exceed 20 characters.");

            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("Email is required.")
                .MaximumLength(100).WithMessage("Email cannot exceed 100 characters.")
                .EmailAddress().WithMessage("Invalid email address.");
        }
	}
}

