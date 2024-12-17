using ErrorOr;

namespace MarciaApi.Presentation.Errors.RepositoryErrors;

public static class OrderRepositoryErrors
{
    public static readonly Error HaventFoundAnyOrder = Error.NotFound("Order.HaventFoundAnyOrder", "Não há nenhum pedido");
    public static readonly Error HaventFoundAnyOrderWithProvidedId = Error.NotFound("Order.HaventFoundAnyOrderWithProvidedId", "Não há pedidos na nossa api com este id!");
}