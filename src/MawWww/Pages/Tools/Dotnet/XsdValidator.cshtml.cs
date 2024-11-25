using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using Microsoft.AspNetCore.Mvc;
using MawWww.Models;

namespace MawWww.Pages.Tools.Dotnet;

public class XsdValidatorPageModel
    : MawFormPageModel<XsdValidatorForm>
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
            ValidateSchema();
            SubmitSuccess = true;
        }

        return Page();
    }

    public void ValidateSchema()
    {
        Stream? xsdStream = null;
        XmlReader? reader = null;

        try
        {
            var settings = new XmlReaderSettings();

            xsdStream = Utils.ToStream(Form.XmlSchemaSource!);

            reader = XmlReader.Create(xsdStream, settings);

            XmlSchema.Read(reader, ValidationHandler);
        }
        catch (XmlSchemaException ex)
        {
            _currError++;
            _errors.Append($"[{ _currError }] Error Parsing XML Schema:\n");
            _errors.Append($"[{ _currError }] { ex.Source }\n");
            _errors.Append($"[{ _currError }] Line: { ex.LineNumber }\n");
            _errors.Append($"[{ _currError }] Position: { ex.LinePosition }\n");
            _errors.Append($"[{ _currError }] Message: { ex.Message }\n\n");
        }
        catch (XmlException ex)
        {
            _currError++;
            _errors.Append($"[{ _currError }] Error Validating XSD:\n");
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
            reader?.Close();
            xsdStream?.Close();
        }
    }

    void ValidationHandler(object? sender, ValidationEventArgs e)
    {
        _currError++;

        _errors.Append($"[{ _currError }] Error Validating XSD:\n");
        _errors.Append($"[{ _currError }] Line: { e.Exception.LineNumber }\n");
        _errors.Append($"[{ _currError }] Position: { e.Exception.LinePosition }\n");
        _errors.Append($"[{ _currError }] Message: { e.Exception.Message }\n\n");
    }
}

public class XsdValidatorForm
{
    [Required(ErrorMessage = "Please enter the XML Schema source")]
    public string? XmlSchemaSource { get; set; } = string.Empty;
}
