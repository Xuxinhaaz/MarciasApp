using ErrorOr;

namespace MarciaApi.Presentation.Errors.RepositoryErrors;

public static class UserRepositoryErrors
{
    public static readonly Error UserNotSignedUp = Error.Unauthorized("User.UserNotSignedUp", "Você não está autenticado na nossa api!");
    public static readonly Error HaventFoundAnyUserWithProvidedId = Error.NotFound("User.HaventFoundAnyUserWithProvidedId", "Não há usuarios na nossa api com este id!");
}