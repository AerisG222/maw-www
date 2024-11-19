using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml;
using Microsoft.AspNetCore.Mvc;
using MawWww.Models;
using System.Xml.Xsl;

namespace MawWww.Pages.Tools.Dotnet;

public class XslTransformModel
    : MawFormPageModel<XslTransformForm>
{
    readonly StringBuilder _errors = new();
    int _currError = 0;

    public string? TransformResult { get; set; }
    public bool AreErrors => _currError > 0;
    public string ErrorMessage => _errors.ToString();

    public IActionResult OnGet()
    {
        return Page();
    }

    public IActionResult OnPostAsync()
    {
        SubmitAttempted = true;

        if(ModelState.IsValid)
        {
            ExecuteTransform();
            SubmitSuccess = true;
        }

        return Page();
    }

    public void ExecuteTransform()
    {
        var xmlStream = Form.XmlSource!.ToStream();
        var xslStream = Form.XsltSource!.ToStream();

        XmlReader? xmlReader = null;
        XmlReader? xslReader = null;
        MemoryStream? ms = null;
        XmlWriter? xmlWriter = null;
        TextReader? tr = null;

        try
        {
            var settings = new XmlReaderSettings();

            xmlReader = XmlReader.Create(xmlStream, settings);
            xslReader = XmlReader.Create(xslStream, settings);

            var xslTransform = new XslCompiledTransform(true);
            xslTransform.Load(xslReader);

            var xmlWriterSettings = new XmlWriterSettings
            {
                Indent = true,
                NewLineHandling = NewLineHandling.Replace
            };

            ms = new MemoryStream();
            xmlWriter = XmlWriter.Create(ms);

            xslTransform.Transform(xmlReader, xmlWriter);

            ms.Seek(0, SeekOrigin.Begin);
            tr = new StreamReader(ms);

            TransformResult = tr.ReadToEnd();
        }
        catch (XsltException ex)
        {
            _currError++;
            _errors.Append($"[{ _currError }] Error Executing XSLT:\n");
            _errors.Append($"[{ _currError }] { ex.Source }\n");
            _errors.Append($"[{ _currError }] Line: { ex.LineNumber }\n");
            _errors.Append($"[{ _currError }] Position: { ex.LinePosition }\n");
            _errors.Append($"[{ _currError }] Message: { ex.Message }\n\n");
        }
        catch (XmlException ex)
        {
            _currError++;
            _errors.Append($"[{ _currError }] Error Parsing XML or XSL:\n");
            _errors.Append($"[{ _currError }] { ex.Source }\n");
            _errors.Append($"[{ _currError }] Line: { ex.LineNumber }\n");
            _errors.Append($"[{ _currError }] Position: { ex.LinePosition }\n");
            _errors.Append($"[{ _currError }] Message: { ex.Message }\n\n");
        }
        catch (Exception ex)
        {
            _currError++;
            _errors.Append(string.Concat("[", _currError, "] Error Executing transform: ", ex.Message, "\n"));
        }
        finally
        {
            xslReader?.Close();
            xmlReader?.Close();
            xmlWriter?.Close();
            ms?.Close();
            tr?.Close();
        }
    }
}

public class XslTransformForm
{
    [Required(ErrorMessage = "Please enter the XML source")]
    public string? XmlSource { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter the XSLT source")]
    public string? XsltSource { get; set; }
}
