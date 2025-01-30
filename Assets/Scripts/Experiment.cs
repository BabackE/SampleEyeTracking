using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static ExtendedEyeGazeDataProvider;

public class Experiment : MonoBehaviour
{

    public ExtendedEyeGazeDataProvider extendedEyeGazeDataProvider;
    public Canvas canvas;
    public TextMeshProUGUI leftText;
    public TextMeshProUGUI rightText;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        leftText.text = "Left: " + extendedEyeGazeDataProvider.GetWorldSpaceGazeReading(ExtendedEyeGazeDataProvider.GazeType.Left).GazeDirection.ToString();
        rightText.text = "Right: " + extendedEyeGazeDataProvider.GetWorldSpaceGazeReading(ExtendedEyeGazeDataProvider.GazeType.Right).GazeDirection.ToString();

        Debug.Log(leftText.text);
        Debug.Log(rightText.text);
    }
}
