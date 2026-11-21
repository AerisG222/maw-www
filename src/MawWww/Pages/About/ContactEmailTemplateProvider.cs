using Fluid;
using Microsoft.Extensions.FileProviders;

namespace MawWww.Pages.About;

public sealed class ContactEmailTemplateProvider
{
    static string TemplateDir => Path.Combine(AppContext.BaseDirectory, "EmailTemplates");

    public IFluidTemplate Template { get; }

    // the template's {% render %} tags (header and footer) resolve against TemplateDir.
    // built once: since fluid 3 options are immutable, so one instance serves every render
    public TemplateOptions Options { get; }

    public ContactEmailTemplateProvider(FluidParser parser)
    {
        var templatePath = Path.Combine(TemplateDir, "ContactUs.liquid");
        var templateSource = File.ReadAllText(templatePath);
        Template = parser.Parse(templateSource);

        Options = new TemplateOptionsBuilder()
            .WithFileProvider(new FileProviderTemplateFileProvider(new PhysicalFileProvider(TemplateDir)))
            .Build();
    }
}
