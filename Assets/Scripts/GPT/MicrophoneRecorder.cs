using System;
using UnityEngine;

namespace GPT
{
    public class MicrophoneRecorder
    {
        private string _currentMicrophone;
        public bool IsRecording { get; private set; }
        private AudioClip _audio;
        
        public event Action<AudioClip> OnStoppedRecording;

        public void SetMicrophone(int index)
        {
            _currentMicrophone = Microphone.devices[index];
        }

        public void RecordAndSave()
        {
            if (IsRecording)
            {
                Microphone.End(_currentMicrophone);

                OnStoppedRecording?.Invoke(_audio);
            }
            else
            {
                _audio = Microphone.Start(_currentMicrophone, false, 30, 44100);
            }

            IsRecording = !IsRecording;
        }
    }
}