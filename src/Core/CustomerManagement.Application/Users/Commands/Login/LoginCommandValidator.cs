using FluentValidation;

namespace CustomerManagement.Application.Users.Commands.Login
{
	public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
		public LoginCommandValidator()
		{
            RuleFor(req => req.Email)
                .MaximumLength(100).WithMessage("Email cannot exceed 100 characters.")
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email address.");

            RuleFor(req => req.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MaximumLength(25).WithMessage("Password cannot exceed 25 characters.");
        }
	}
}

