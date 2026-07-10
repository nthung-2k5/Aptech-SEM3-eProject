namespace GiveAID;

public static class FormFileExtensions
{
    public static async Task<string> ToDataUrlAsync(this IFormFile file)
    {
        if (file.Length == 0) return string.Empty;

        using var ms = new MemoryStream();

        await file.CopyToAsync(ms);
        string base64String = Convert.ToBase64String(ms.ToArray());
        
        return $"data:{file.ContentType};base64,{base64String}";
    }
}
