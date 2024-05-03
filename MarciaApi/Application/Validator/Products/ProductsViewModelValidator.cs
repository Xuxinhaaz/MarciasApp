using FluentValidation;
using MarciaApi.Presentation.ViewModel.Products;
using Microsoft.IdentityModel.Protocols;

namespace MarciaApi.Application.Validator.Products;

public class ProductsViewModelValidator : AbstractValidator<ProductsViewModel>
{
    public ProductsViewModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(NotEmpty("nome"))
            .MinimumLength(3)
            .WithMessage(MinLength("nome", 3))
            .MaximumLength(50)
            .WithMessage(MaxLength("nome", 50));

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage(NotEmpty("descrição"))
            .MinimumLength(3)
            .WithMessage(MinLength("descrição", 3))
            .MaximumLength(100)
            .WithMessage(MaxLength("descrição", 100));

        RuleFor(x => x.Price)
            .NotEmpty()
            .WithMessage(NotEmpty("preço"));

        RuleForEach(x => x.ItemsNames).ChildRules(x =>
        {
            x.RuleFor(m => m)
                .NotEmpty()
                .WithMessage(NotEmpty("o nome dos items enviados não existem no nosso sistema!"))
                .MinimumLength(3)
                .WithMessage(MinLength("", 3));

        });
    }

    private string NotEmpty(string field)
    {
        if (field.Length > 20)
        {
            return field;
        }
        
        return $"o campo {field} nao pode estar vazio!";
    }

    private string MinLength(string field, int quantity)
    {
        if(field.Length > 20)
        {
            return field;
        }
        
        return $"o campo {field} deve conter pelo menos {quantity} caracteres";
    }
    
    private string MaxLength(string field, int quantity)
    {
        if(field.Length >= 20)
        {
            return field;
        }
        
        return $"o campo {field} deve conter no máximo {Convert.ToString(quantity)} caracteres";
    }
    
   
}