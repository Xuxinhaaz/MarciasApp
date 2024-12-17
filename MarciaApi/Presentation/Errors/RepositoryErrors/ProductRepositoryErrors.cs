using ErrorOr;

namespace MarciaApi.Presentation.Errors.RepositoryErrors;

public static class ProductRepositoryErrors
{
    public static readonly Error HaventFoundProductWithProvidedId = 
        Error.NotFound("Product.HaventFoundProductWithProvidedId", "Não há produtos na nossa api com este id!");

    public static readonly Error ThereIsAnExistingProductWithSameName = Error.Failure("Product.ThereIsAnExistingProductWithSameName", "já existe um produto com este nome cadastrado!");
    public static readonly Error ThereIsntAnExistingProductWithSameName = Error.Failure("Product.ThereIsntAnExistingProductWithSameName", "não existe um produto com este nome cadastrado!");
}