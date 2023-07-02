using DeepL;
using DeepL.Model;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace imarketer_translation_api.Controllers;

[Route("[controller]")]
public class TranslationController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly ITranslator _translator;

    public TranslationController(IConfiguration configuration)
    {
        _configuration = configuration;
        _translator = new Translator(_configuration["DeepL:ServiceApiKey"]);
    }

    [HttpGet("/translations")]
    public async Task<ActionResult<IList<string>>> GetTranslations([FromQuery, Required] string text, [FromQuery, Required] IEnumerable<string> languages)
    {
        List<string> textResults = new();
        try
        {
            foreach (var language in languages)
            {
                textResults.Add((await _translator.TranslateTextAsync(text, null, language)).Text);
            }
            return Ok(textResults);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e);
        }
    }
}