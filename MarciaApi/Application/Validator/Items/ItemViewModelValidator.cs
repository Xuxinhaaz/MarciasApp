using FluentValidation;
using MarciaApi.Presentation.ViewModel.Items;

namespace MarciaApi.Application.Validator.Items;

public class ItemViewModelValidator : AbstractValidator<ItemsViewModel>
{
    public ItemViewModelValidator()
    {
        RuleFor(x => x.ItemPrice)
            .NotNull().WithMessage("O item não pode estar vazio")
            .DependentRules(() =>
            {
                RuleFor(x => x.ItemPrice)
                    .GreaterThanOrEqualTo(0).WithMessage("O item precisa ter um valor maior ou igual a 0");
            });
        RuleFor(x => x.ItemName)
            .NotNull().WithMessage("O item não pode estar vazio")
            .DependentRules(() =>
            {
                RuleFor(x => x.ItemName)
                    .MinimumLength(3).WithMessage("O item precisa ter um nome com mais de 3 caracteres");
            });
    }
}