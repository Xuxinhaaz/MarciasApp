using ErrorOr;

namespace MarciaApi.Presentation.Errors.RepositoryErrors;

public static class ItemsRepositoryErrors
{
    public static readonly Error ThereIsntItemWithProvidedSameName = 
        Error.NotFound(
            "Item.ThereIsntItemWithProvidedSameName", 
            "não existe um item com este nome cadastrado!");

    public static readonly Error ThereIsntItemWithProvidedId =
        Error.NotFound(
            "Item.ThereIsntItemWithProvidedId", 
            "não existe um item com este id na nossa api!");

    public static readonly Error AlreadyExistsItemWithProvidedName = 
        Error.Failure( 
            "Item.AlreadyExistsItemWithProvidedName", 
            "já existe um item com este nome cadastrado");
}