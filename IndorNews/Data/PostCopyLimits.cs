namespace IndorNews.Data;

public static class PostCopyLimits
{
    public const int TitleMax = 180;
    public const int BodyMax = 4000;

    public static string? Validate(string? title, string? body)
    {
        title = (title ?? "").Trim();
        body = (body ?? "").Trim();

        if (title.Length == 0)
        {
            return "El título es obligatorio.";
        }

        if (title.Length > TitleMax)
        {
            return $"El título no puede pasar de {TitleMax} caracteres (ahora {title.Length}).";
        }

        if (body.Length > BodyMax)
        {
            return $"El texto no puede pasar de {BodyMax} caracteres (ahora {body.Length}).";
        }

        return null;
    }
}
