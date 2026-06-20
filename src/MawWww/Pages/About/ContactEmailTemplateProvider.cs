using Fluid;

namespace MawWww.Pages.About;

public class ContactEmailTemplateProvider
{
    static string TemplateDir => Path.Combine(AppContext.BaseDirectory, "EmailTemplates");

    public IFluidTemplate Template { get; }

    public ContactEmailTemplateProvider(FluidParser parser)
    {
        var templatePath = Path.Combine(TemplateDir, "ContactUs.liquid");
        var templateSource = File.ReadAllText(templatePath);
        Template = parser.Parse(templateSource);
    }
}
