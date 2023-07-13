using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MicrophoneRecorder : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private Button record;
    [SerializeField] private TMP_Dropdown microphones;
    private AudioClip _audio;
    private bool _isRecording;
    private string _currentMicrophone;

    public event Action<AudioClip> OnStoppedRecording;

    private void Awake()
    {
        microphones.options.Clear();

        for (int i = 0; i < Microphone.devices.Length; i++)
        {
            microphones.options.Add(new TMP_Dropdown.OptionData($"Microphone: {i}"));    
        }

        microphones.onValueChanged.AddListener(index => _currentMicrophone = Microphone.devices[index]);
        
        record.onClick.AddListener(() =>
        {
            RecordAndSave();
            _isRecording = !_isRecording;
            buttonText.text = _isRecording ? "stop" : "start";
        });
    }

    private void RecordAndSave()
    {
        if (_isRecording)
        {
            Microphone.End(_currentMicrophone);

            OnStoppedRecording?.Invoke(_audio);
        }
        else
        {
            _audio = Microphone.Start(_currentMicrophone, false, 30, 44100);
        }
    }
}