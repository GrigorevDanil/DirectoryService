namespace DirectoryService.Application.Constants;

public static class CachingKeys
{
    public const string DEPARTMENTS_KEY = "departments_";

    public static string CreateDepartmentsKey(params string[] parameters)
        => DEPARTMENTS_KEY + CreateKey(parameters);

    private static string CreateKey(params string[] parameters) => string.Join('_', parameters);
}