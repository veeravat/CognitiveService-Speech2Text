// See https://aka.ms/new-console-template for more information
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

string YourSubscriptionKey = "<YourSubScriptionKey>";
string YourServiceRegion = "<Region>";


var speechConfig = SpeechConfig.FromSubscription(YourSubscriptionKey, YourServiceRegion);
var autoDetectSourceLanguageConfig = AutoDetectSourceLanguageConfig.FromLanguages(new string[] { "en-US", "th-TH" });

//To recognize speech from an audio file, use `FromWavFileInput` instead of `FromDefaultMicrophoneInput`:
//using var audioConfig = AudioConfig.FromWavFileInput("YourAudioFile.wav");
using var audioConfig = AudioConfig.FromDefaultMicrophoneInput();
using var speechRecognizer = new SpeechRecognizer(speechConfig, autoDetectSourceLanguageConfig, audioConfig);
var stopRecognition = new TaskCompletionSource<int>();
Console.WriteLine("Speak into your microphone.");

var FinalText = new List<string>();

//When recognizing
speechRecognizer.Recognizing += (s, e) =>
{
    Console.Clear();
    Console.WriteLine($"กำลังฟัง: {e.Result.Text}");
};

speechRecognizer.Recognized += (s, e) =>
{
    Console.Clear();
    if (e.Result.Reason == ResultReason.RecognizedSpeech)
    {
        FinalText.Add(e.Result.Text);
        Console.WriteLine($"ผลลัพธ์: {e.Result.Text}");
        if (e.Result.Text.ToLower() == "stop" || e.Result.Text.ToLower() == "หยุด")
        {
            FinalText.RemoveAt(FinalText.Count - 1);
            stopRecognition.TrySetResult(0);
        }
    }
    else if (e.Result.Reason == ResultReason.NoMatch)
    {
        Console.WriteLine($"NOMATCH: Speech could not be recognized.");
    }
};
speechRecognizer.Canceled += (s, e) =>
{
    Console.WriteLine($"CANCELED: Reason={e.Reason}");

    if (e.Reason == CancellationReason.Error)
    {
        Console.WriteLine($"CANCELED: ErrorCode={e.ErrorCode}");
        Console.WriteLine($"CANCELED: ErrorDetails={e.ErrorDetails}");
        Console.WriteLine($"CANCELED: Did you update the subscription info?");
    }

    stopRecognition.TrySetResult(0);
};

speechRecognizer.SessionStarted += (s, e) =>
{
    Console.WriteLine("\n    Session started event.");
};

speechRecognizer.SessionStopped += (s, e) =>
{
    Console.WriteLine("\n    Session stopped event.");
    Console.WriteLine("\nStop recognition.");
    stopRecognition.TrySetResult(0);
};

// Starts continuous recognition. Uses StopContinuousRecognitionAsync() to stop recognition.
await speechRecognizer.StartContinuousRecognitionAsync().ConfigureAwait(false);

// Waits for completion.
// Use Task.WaitAny to keep the task rooted.
Task.WaitAny(new[] { stopRecognition.Task });

// Stops recognition.
await speechRecognizer.StopContinuousRecognitionAsync().ConfigureAwait(false);

Console.WriteLine("Showing all result");
foreach (var text in FinalText)
{
    Console.WriteLine(text);
}