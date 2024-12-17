using ErrorOr;

namespace MarciaApi.Presentation.Errors.AuthErrors;

public static class JwtErrors
{
    public static Error TokenIsNull = 
        Error.Unexpected(code: "Jwt.Unexpected", description: "Um erro inesperado ocorreu");
    public static Error NotSignedUp = 
        Error.Unauthorized(code: "Jwt.NotSignedUp", description: "você não esta logado no nosso site!");
    public static Error UserIsNotAllowed =
        Error.Unauthorized(code: "Jwt.UserIsNotAllowed", description: "você não pode acessar este endpoint!");
}