using FluentValidation;
using MarciaApi.Presentation.ViewModel.Orders;

namespace MarciaApi.Application.Validator.Orders;

public class OrderViewModelValidator : AbstractValidator<OrdersViewModel>
{
    public OrderViewModelValidator()
    {
        var notEmpty = "este campo nao pode estar vazio!";
        
        RuleFor(m => m.UserName)
            .NotEmpty().WithMessage(notEmpty)
            .MinimumLength(2).WithMessage(MinLength("Nome de usuario", 2))
            .MaximumLength(50).WithMessage(MaxLength("Nome de usuario", 50));

        RuleFor(m => m.UserPhone)
            .NotEmpty().WithMessage(notEmpty)
            .Matches(@"^\(?[1-9]{2}\)? ?[2-9]{2}\d{3}\d{4}$").WithMessage("Telefone deve esstar em um formato valido");

        RuleFor(m => m.PaymentType)
            .NotEmpty().WithMessage(notEmpty)
            .MinimumLength(2).WithMessage(MinLength("Tipo de pagamento", 2));

        RuleFor(m => m.Location).ChildRules(x =>
        {
            x.RuleFor(j => j.CEP)
                .NotEmpty().WithMessage(notEmpty)
                .Matches(@"^\d{5}\d{3}$").WithMessage("CEP deve estar em um formato valido!");

            x.RuleFor(j => j.Neighborhood)
                .NotEmpty().WithMessage(notEmpty)
                .MinimumLength(5).WithMessage(MinLength("Bairro", 5))
                .MaximumLength(100).WithMessage(MaxLength("Bairro", 100));

            x.RuleFor(j => j.Number)
                .NotEmpty().WithMessage(notEmpty)
                .MinimumLength(1).WithMessage(MinLength("Numero da casa", 1))
                .MaximumLength(8).WithMessage(MaxLength("Numero da casa", 8));

            x.RuleFor(j => j.Street)
                .NotEmpty().WithMessage(notEmpty)
                .MinimumLength(3).WithMessage(MinLength("Rua", 3))
                .MaximumLength(60).WithMessage(MinLength("Rua", 60));

            x.RuleFor(j => j.AdditionalAdressInfo)
                .NotEmpty().WithMessage(notEmpty)
                .MinimumLength(3).WithMessage(MinLength("Logradouro", 3))
                .MaximumLength(50).WithMessage(MaxLength("Locagradouro", 50));
        })
        .NotEmpty().WithMessage(notEmpty);
    }


    private string MinLength(string prop, int quantity)
    {
        return $"{prop} deve ter pelo menos {quantity} caracteres";
    }
    
    private string MaxLength(string prop, int quantity)
    {
        return $"{prop} deve ter no máximo {quantity} caracteres";
    }
}