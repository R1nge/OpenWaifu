using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace GPT
{
    public class MicrophoneRecorderView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI buttonText;
        [SerializeField] private Button record;
        [SerializeField] private TMP_Dropdown microphones;
        private MicrophoneRecorder _microphoneRecorder;

        [Inject]
        private void Construct(MicrophoneRecorder microphoneRecorder)
        {
            _microphoneRecorder = microphoneRecorder;
        }

        private void Awake()
        {
            
            microphones.options.Clear();

            for (int i = 0; i < Microphone.devices.Length; i++)
            {
                microphones.options.Add(new TMP_Dropdown.OptionData($"Microphone: {i}"));    
            }

            microphones.onValueChanged.AddListener(index => _microphoneRecorder.SetMicrophone(index));
        
            record.onClick.AddListener(() =>
            {
                _microphoneRecorder.RecordAndSave();
                buttonText.text = _microphoneRecorder.IsRecording ? "stop" : "start";
            });
        }
    }
}