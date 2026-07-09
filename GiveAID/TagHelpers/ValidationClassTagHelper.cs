using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GiveAID.TagHelpers;

[HtmlTargetElement("span", Attributes = "asp-validation-for")]
public class ValidationClassTagHelper : TagHelper
{
    public override int Order => 0;

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        const string tailwindClasses = "validation-message";

        if (output.Attributes.ContainsName("class"))
        {
            string? existingClasses = output.Attributes["class"].Value?.ToString();
            output.Attributes.SetAttribute("class", $"{existingClasses} {tailwindClasses}".Trim());
        }
        else
        {
            output.Attributes.SetAttribute("class", tailwindClasses);
        }
    }
}