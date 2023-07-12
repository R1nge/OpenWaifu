using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Whisper;
using Whisper.Utils;

public class WhisperTest : MonoBehaviour
{
    public WhisperManager whisper;
    public MicrophoneRecord microphoneRecord;
    public bool streamSegments = true;
    public bool printLanguage = true;

    [Header("UI")] public Button button;
    public TextMeshProUGUI outputText;
    public TMP_Dropdown languageDropdown;
    public Toggle translateToggle;

    private string _buffer;

    private void Awake()
    {
        button.onClick.AddListener(OnButtonPressed);

        languageDropdown.value = languageDropdown.options
            .FindIndex(op => op.text == whisper.language);
        languageDropdown.onValueChanged.AddListener(OnLanguageChanged);

        translateToggle.isOn = whisper.translateToEnglish;
        translateToggle.onValueChanged.AddListener(OnTranslateChanged);

        microphoneRecord.OnRecordStop += Transcribe;

        if (streamSegments)
            whisper.OnNewSegment += WhisperOnOnNewSegment;
    }

    private void OnButtonPressed()
    {
        if (!microphoneRecord.IsRecording)
            microphoneRecord.StartRecord();
        else
            microphoneRecord.StopRecord();
    }

    private void OnLanguageChanged(int ind)
    {
        var opt = languageDropdown.options[ind];
        whisper.language = opt.text;
    }

    private void OnTranslateChanged(bool translate)
    {
        whisper.translateToEnglish = translate;
    }

    private async void Transcribe(float[] data, int frequency, int channels, float length)
    {
        _buffer = "";

        var sw = new Stopwatch();
        sw.Start();

        var res = await whisper.GetTextAsync(data, frequency, channels);
        
        if (res == null)
            return;

        var text = res.Result;
        if (printLanguage)
            text += $"\n\nLanguage: {res.Language}";
        outputText.text = text;
    }

    private void WhisperOnOnNewSegment(WhisperSegment segment)
    {
        _buffer += segment.Text;
        outputText.text = _buffer + "...";
    }
}