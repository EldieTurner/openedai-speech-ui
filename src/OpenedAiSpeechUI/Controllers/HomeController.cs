using System.Diagnostics;
using System.Text.Json;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using OpenedAiSpeechUI.Models;

namespace OpenedAiSpeechUI.Controllers;

public class HomeController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public HomeController(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(configuration["TTSApi:ApiUrl"]);
        _configuration = configuration;
    }

    [HttpGet]
    public IActionResult Index()
    {
        // Initialize with default values.
        var model = new TTSViewModel
        {
            Request = new TextToSpeechRequest
            {
                Input = "",
                Model = "tts-1",
                Voice = "alloy",
                ResponseFormat = "mp3",
                Speed = 1.0,
                Temperature = 0.75,
                TopP = 0.85,
                TopK = 50,
                LengthPenalty = 1.0,
                RepetitionPenalty = 10
            },
            AudioFilePath = null
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Index(TTSViewModel model)
    {
        if (string.IsNullOrEmpty(model.Request.Input))
        {
            ModelState.AddModelError("Request.Input", "Please enter some text.");
            return View(model);
        }

        try
        {
            var route = "v1/audio/speech";
            var jsonString = JsonSerializer.Serialize(model.Request);

            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync(route, content);

            if (response.IsSuccessStatusCode)
            {
                // Read the returned audio data.
                var audioBytes = await response.Content.ReadAsByteArrayAsync();

                // Save the file to wwwroot/audio (create folder if needed)
                var audioFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "audio");
                if (!Directory.Exists(audioFolder))
                {
                    Directory.CreateDirectory(audioFolder);
                }

                // Create a unique filename.
                var fileName = $"speech_{DateTime.Now.Ticks}.{model.Request.ResponseFormat}";
                var filePath = Path.Combine(audioFolder, fileName);
                await System.IO.File.WriteAllBytesAsync(filePath, audioBytes);

                // Set the AudioFilePath for playback.
                model.AudioFilePath = $"/audio/{fileName}";
            }
            else
            {
                ModelState.AddModelError(string.Empty, $"Error: {response.ReasonPhrase}");
            }
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, "Exception: " + ex.Message);
        }

        return View(model);
    }
}