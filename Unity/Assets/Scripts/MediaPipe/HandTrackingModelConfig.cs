using System;
using System.Collections.Generic;
using Mediapipe.Unity;
using Mediapipe.Unity.UI;
using UnityEngine;
using UnityEngine.UI;

public class HandTrackingModelConfig : ModalContents
{
    //TODO: inject
    private HandTrackingModel _solution;
    //TODO: create setting menu, delete samples
    [SerializeField] private Dropdown modelComplexityInput;
    [SerializeField] private InputField maxNumHandsInput;
    [SerializeField] private InputField minDetectionConfidenceInput;
    [SerializeField] private InputField minTrackingConfidenceInput;
    [SerializeField] private Dropdown runningModeInput;
    [SerializeField] private InputField timeoutMillisecInput;

    private bool _isChanged;

    private void Start()
    {
        _solution = FindObjectOfType<HandTrackingModel>();
        InitializeContents();
    }

    public override void Exit()
    {
        GetModal().CloseAndResume(_isChanged);
    }

    public void SwitchModelComplexity()
    {
        _solution.modelComplexity = (HandTrackingGraph.ModelComplexity)modelComplexityInput.value;
        _isChanged = true;
    }

    public void UpdateMaxNumHands()
    {
        if (int.TryParse(maxNumHandsInput.text, out var value))
        {
            _solution.maxNumHands = Mathf.Max(0, value);
            _isChanged = true;
        }
    }

    public void SetMinDetectionConfidence()
    {
        if (float.TryParse(minDetectionConfidenceInput.text, out var value))
        {
            _solution.minDetectionConfidence = value;
            _isChanged = true;
        }
    }

    public void SetMinTrackingConfidence()
    {
        if (float.TryParse(minTrackingConfidenceInput.text, out var value))
        {
            _solution.minTrackingConfidence = value;
            _isChanged = true;
        }
    }

    public void SwitchRunningMode()
    {
        _solution.runningMode = (RunningMode)runningModeInput.value;
        _isChanged = true;
    }

    public void SetTimeoutMillisec()
    {
        if (int.TryParse(timeoutMillisecInput.text, out var value))
        {
            _solution.timeoutMillisec = value;
            _isChanged = true;
        }
    }

    private void InitializeContents()
    {
        InitializeModelComplexity();
        InitializeMaxNumHands();
        InitializeMinDetectionConfidence();
        InitializeMinTrackingConfidence();
        InitializeRunningMode();
        InitializeTimeoutMilliseconds();
    }

    private void InitializeModelComplexity()
    {
        modelComplexityInput.ClearOptions();

        var options = new List<string>(Enum.GetNames(typeof(HandTrackingGraph.ModelComplexity)));
        modelComplexityInput.AddOptions(options);

        var currentModelComplexity = _solution.modelComplexity;
        var defaultValue = options.FindIndex(option => option == currentModelComplexity.ToString());

        if (defaultValue >= 0)
        {
            modelComplexityInput.value = defaultValue;
        }

        modelComplexityInput.onValueChanged.AddListener(delegate { SwitchModelComplexity(); });
    }

    private void InitializeMaxNumHands()
    {
        maxNumHandsInput.text = _solution.maxNumHands.ToString();
        maxNumHandsInput.onEndEdit.AddListener(delegate { UpdateMaxNumHands(); });
    }

    private void InitializeMinDetectionConfidence()
    {
        minDetectionConfidenceInput.text = _solution.minDetectionConfidence.ToString();
        minDetectionConfidenceInput.onValueChanged.AddListener(delegate { SetMinDetectionConfidence(); });
    }

    private void InitializeMinTrackingConfidence()
    {
        minTrackingConfidenceInput.text = _solution.minTrackingConfidence.ToString();
        minTrackingConfidenceInput.onValueChanged.AddListener(delegate { SetMinTrackingConfidence(); });
    }

    private void InitializeRunningMode()
    {
        runningModeInput.ClearOptions();

        var options = new List<string>(Enum.GetNames(typeof(RunningMode)));
        runningModeInput.AddOptions(options);

        var currentRunningMode = _solution.runningMode;
        var defaultValue = options.FindIndex(option => option == currentRunningMode.ToString());

        if (defaultValue >= 0)
        {
            runningModeInput.value = defaultValue;
        }

        runningModeInput.onValueChanged.AddListener(delegate { SwitchRunningMode(); });
    }

    private void InitializeTimeoutMilliseconds()
    {
        timeoutMillisecInput.text = _solution.timeoutMillisec.ToString();
        timeoutMillisecInput.onValueChanged.AddListener(delegate { SetTimeoutMillisec(); });
    }
}