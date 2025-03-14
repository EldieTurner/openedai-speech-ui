using System.Text.Json.Serialization;

namespace OpenedAiSpeechUI.Models;

public class TextToSpeechRequest
{
    [JsonPropertyName("input")]
    public string Input { get; set; }

    [JsonPropertyName("model")]
    public string Model { get; set; }

    [JsonPropertyName("voice")]
    public string Voice { get; set; }

    [JsonPropertyName("response_format")]
    public string ResponseFormat { get; set; }

    [JsonPropertyName("speed")]
    public double Speed { get; set; }

    // Advanced parameters (used with tts-1-hd)
    [JsonPropertyName("temperature")]
    public double Temperature { get; set; }

    [JsonPropertyName("top_p")]
    public double TopP { get; set; }

    [JsonPropertyName("top_k")]
    public int TopK { get; set; }

    [JsonPropertyName("length_penalty")]
    public double LengthPenalty { get; set; }

    [JsonPropertyName("repetition_penalty")]
    public double RepetitionPenalty { get; set; }
}
