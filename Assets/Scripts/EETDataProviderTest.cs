// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;
using TMPro;
using UnityEngine;

public class EETDataProviderTest : MonoBehaviour
{
    [SerializeField]
    private GameObject LeftGazeObject;
    [SerializeField]
    private GameObject RightGazeObject;
    [SerializeField]
    private GameObject CombinedGazeObject;
    [SerializeField]
    private GameObject CameraRelativeCombinedGazeObject;
    [SerializeField]
    private GameObject VergenceObject;
    [SerializeField]
    private ExtendedEyeGazeDataProvider extendedEyeGazeDataProvider;
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private TextMeshProUGUI leftText;
    [SerializeField]
    private TextMeshProUGUI rightText;
    [SerializeField]
    private TextMeshProUGUI combinedText;

    private DateTime timestamp;
    private ExtendedEyeGazeDataProvider.GazeReading gazeReading;
    private ExtendedEyeGazeDataProvider.VergenceReading vergeReading;

    void Update()
    {
        timestamp = DateTime.Now;

        // positioning for left gaze object
        gazeReading = extendedEyeGazeDataProvider.GetWorldSpaceGazeReading(ExtendedEyeGazeDataProvider.GazeType.Left, timestamp);
        if (gazeReading.IsValid)
        {
            // position gaze object 1.5 meters out from the gaze origin along the gaze direction
            LeftGazeObject.transform.position = gazeReading.EyePosition + 1.5f * gazeReading.GazeDirection;
            leftText.text = "Left: " + gazeReading.GazeDirection.ToString("F6");
            LeftGazeObject.SetActive(true);
        }
        else
        {
            LeftGazeObject.SetActive(false);
        }

        // positioning for right gaze object
        gazeReading = extendedEyeGazeDataProvider.GetWorldSpaceGazeReading(ExtendedEyeGazeDataProvider.GazeType.Right, timestamp);
        if (gazeReading.IsValid)
        {
            // position gaze object 1.5 meters out from the gaze origin along the gaze direction
            RightGazeObject.transform.position = gazeReading.EyePosition + 1.5f * gazeReading.GazeDirection;
            rightText.text = "Right: " + gazeReading.GazeDirection.ToString("F6");
            RightGazeObject.SetActive(true);
        }
        else
        {
            RightGazeObject.SetActive(false);
        }

        //get vergence reading and pass to vergence ball;
        //vergeReading = extendedEyeGazeDataProvider.GetWorldSpaceCyclopVergence();
        vergeReading = extendedEyeGazeDataProvider.GetWorldSpaceBinocularVergence();
        if (vergeReading.IsValid)
        {
            //VergenceObject.transform.position = vergeReading.EyePosition + 1.2f * vergeReading.GazeDirection;
            //VergenceObject.transform.position = vergeReading.EyePosition + vergeReading.FocusPoint;
            VergenceObject.transform.position = vergeReading.FocusPoint;
            VergenceObject.SetActive(true);
        }
        else
        {
            VergenceObject.SetActive(false);
        }

        // positioning for combined gaze object
        gazeReading = extendedEyeGazeDataProvider.GetWorldSpaceGazeReading(ExtendedEyeGazeDataProvider.GazeType.Combined, timestamp);
        if (gazeReading.IsValid)
        {
            // position gaze object 1.5 meters out from the gaze origin along the gaze direction
            CombinedGazeObject.transform.position = gazeReading.EyePosition + 1.5f * gazeReading.GazeDirection;
            combinedText.text = "Combined: " + gazeReading.GazeDirection.ToString("F6");
            CombinedGazeObject.SetActive(true);
        }
        else
        {
            CombinedGazeObject.SetActive(false);
        }

        // positioning for camera relative gaze cube
        gazeReading = extendedEyeGazeDataProvider.GetCameraSpaceGazeReading(ExtendedEyeGazeDataProvider.GazeType.Combined, timestamp);
        if (gazeReading.IsValid)
        {
            // position gaze object 1.5 meters out from the gaze origin along the gaze direction
            CameraRelativeCombinedGazeObject.transform.localPosition = gazeReading.EyePosition + 1.5f * gazeReading.GazeDirection;
            canvas.transform.localPosition = gazeReading.EyePosition + 1.5f * Vector3.forward;
            CameraRelativeCombinedGazeObject.SetActive(true);
        }
        else
        {
            CameraRelativeCombinedGazeObject.SetActive(false);
        }

    }
}
