using System.Collections;
using System.Collections.Generic;
using Mediapipe;
using Mediapipe.Unity;
using UnityEngine;

public class HandTrackingModel : ImageSourceSolution<HandTrackingGraph>
{
    [SerializeField] private MultiHandLandmarkListAnnotationController handLandmarksAnnotationController;

    public HandTrackingGraph.ModelComplexity modelComplexity
    {
        get => graphRunner.modelComplexity;
        set => graphRunner.modelComplexity = value;
    }

    public int maxNumHands
    {
        get => graphRunner.maxNumHands;
        set => graphRunner.maxNumHands = value;
    }

    public float minDetectionConfidence
    {
        get => graphRunner.minDetectionConfidence;
        set => graphRunner.minDetectionConfidence = value;
    }

    public float minTrackingConfidence
    {
        get => graphRunner.minTrackingConfidence;
        set => graphRunner.minTrackingConfidence = value;
    }
    
    protected override void OnStartRun()
    {
        if (!runningMode.IsSynchronous())
        {
            graphRunner.OnHandLandmarksOutput += OnHandLandmarksOutput;
            graphRunner.OnHandednessOutput += OnHandednessOutput;
        }

        var imageSource = ImageSourceProvider.ImageSource;
        SetupAnnotationController(handLandmarksAnnotationController, imageSource, true);
    }

    protected override void AddTextureFrameToInputStream(TextureFrame textureFrame)
    {
        graphRunner.AddTextureFrameToInputStream(textureFrame);
    }

    protected override IEnumerator WaitForNextValue()
    {
        List<NormalizedLandmarkList> handLandmarks = null;
        List<ClassificationList> handedness = null;

        if (runningMode == RunningMode.Sync)
        {
            var _ = graphRunner.TryGetNext(out var _, out var _, out handLandmarks,
                out var _, out var _, out handedness, true);
        }
        else if (runningMode == RunningMode.NonBlockingSync)
        {
            yield return new WaitUntil(() => graphRunner.TryGetNext(out _,
                out _, out handLandmarks, out _,
                out _, out handedness, false));
        }


        handLandmarksAnnotationController.DrawNow(handLandmarks, handedness);
    }

    private void OnHandLandmarksOutput(object stream, OutputEventArgs<List<NormalizedLandmarkList>> eventArgs)
    {
        handLandmarksAnnotationController.DrawLater(eventArgs.value);
    }

    private void OnHandednessOutput(object stream, OutputEventArgs<List<ClassificationList>> eventArgs)
    {
        handLandmarksAnnotationController.DrawLater(eventArgs.value);
    }
}