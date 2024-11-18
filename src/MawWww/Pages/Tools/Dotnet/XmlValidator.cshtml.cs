using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using Microsoft.AspNetCore.Mvc;
using MawWww.Models;

namespace MawWww.Pages.Tools.Dotnet;

public class XmlValidatorModel
    : MawFormPageModel<XmlValidatorForm>
{
    readonly StringBuilder _errors = new();
    int _currError = 0;

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
            ValidateXml();
            SubmitSuccess = true;
        }

        return Page();
    }

    public void ValidateXml()
    {
        Stream? xmlStream = null;
        Stream? xsdStream = null;

        try
        {
            xmlStream = Form.XmlSource!.ToStream();

            if (!string.IsNullOrEmpty(Form.SchemaOrDtdSource))
            {
                xsdStream = Form.SchemaOrDtdSource.ToStream();
            }

            ValidateXml(xmlStream, xsdStream);
        }
        finally
        {
            xsdStream?.Close();
            xmlStream?.Close();
        }
    }

    void ValidateXml(Stream xmlStream, Stream? xsdStream)
    {
        XmlReader? reader = null;
        XmlReader? schemaReader = null;

        try
        {
            var settings = new XmlReaderSettings();

            if (xsdStream != null)
            {
                schemaReader = XmlReader.Create(xsdStream);
                var schema = XmlSchema.Read(schemaReader, ValidationHandler);

                if(schema != null)
                {
                    settings.Schemas.Add(schema);
                }
            }
            else
            {
                settings.ValidationType = ValidationType.None;
            }

            reader = XmlReader.Create(xmlStream, settings);

            // now that we have fully prepared the validating reader,
            // read through all contents of the xml doc to validate
            while (reader.Read())
            {
                // do nothing
            }
        }
        catch (XmlSchemaException ex)
        {
            _currError++;
            _errors.Append($"[{ _currError }] Error Parsing XML Schema\n");
            _errors.Append($"[{ _currError }] Line: { ex.LineNumber }\n");
            _errors.Append($"[{ _currError }] Position: { ex.LinePosition }\n");
            _errors.Append($"[{ _currError }] Message: { ex.Message }\n\n");
        }
        catch (XmlException ex)
        {
            _currError++;
            _errors.Append($"[{ _currError }] Error Parsing XML Schema\n");
            _errors.Append($"[{ _currError }] Line: { ex.LineNumber }\n");
            _errors.Append($"[{ _currError }] Position: { ex.LinePosition }\n");
            _errors.Append($"[{ _currError }] Message: { ex.Message }\n\n");
        }
        catch (Exception ex)
        {
            _currError++;
            _errors.Append($"[{ _currError }] Error Validating XML: { ex.Message }");
        }
        finally
        {
            schemaReader?.Close();
            reader?.Close();
        }
    }

    void ValidationHandler(object? sender, ValidationEventArgs e)
    {
        _currError++;
        _errors.Append($"[{ _currError }] Error Validating XML\n");
        _errors.Append($"[{ _currError }] Line: { e.Exception.LineNumber }\n");
        _errors.Append($"[{ _currError }] Position: { e.Exception.LinePosition }\n");
        _errors.Append($"[{ _currError }] Message: { e.Exception.Message }\n\n");
    }
}

public class XmlValidatorForm
{
    [Required(ErrorMessage = "Please enter the XML source")]
    [DataType(DataType.MultilineText)]
    public string? XmlSource { get; set; } = string.Empty;

    [DataType(DataType.MultilineText)]
    public string? SchemaOrDtdSource { get; set; }
}
