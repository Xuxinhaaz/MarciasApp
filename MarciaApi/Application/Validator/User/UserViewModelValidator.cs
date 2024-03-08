using FluentValidation;
using MarciaApi.Presentation.ViewModel.User;

namespace MarciaApi.Application.Services.Validator;

public class UserViewModelValidator : AbstractValidator<UserViewModel>
{
    public UserViewModelValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Seu email nao pode estar vazio!")
            .EmailAddress().WithMessage("Seu email deve ser válido!");
    }
}