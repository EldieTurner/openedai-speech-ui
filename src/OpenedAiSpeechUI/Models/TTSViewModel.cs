namespace OpenedAiSpeechUI.Models;

public class TTSViewModel
{
    public TextToSpeechRequest Request { get; set; }
    // This property holds the relative path (e.g. /audio/filename.mp3) to the saved audio file.
    public string AudioFilePath { get; set; }
}
