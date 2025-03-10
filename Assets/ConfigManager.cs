using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.OpenXR.Input;

public class ConfigManager : MonoBehaviour
{
    //initial values for targets
    //TODO: populate Vec3s
    [SerializeField]
    private Vector3[] pos = { };
    [SerializeField]
    private Vector3[] scale = { };
    private int numTargets = 3;
    private sceneConfig currConfig;
    private struct sceneConfig
    {
        public Vector3[] targetPos;
        public Vector3[] targetScale;
        public int numTargets;
        public sceneConfig(Vector3[] pos, Vector3[] scale, int n)
        {
            targetPos = pos;
            targetScale = scale;
            numTargets = n;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        currConfig = new sceneConfig(pos, scale, numTargets);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnBtn0()
    {
        Debug.Log("Button 0 Pressed! Reset!");
        // Implement individual actions
        Vector3[] pos = new Vector3[3]
        {
            new Vector3(0f, 0f, -0.2f),
            new Vector3(0f, 0f, 0f),
            new Vector3(0f, 0f, 0.2f)
        }; 
        Vector3[] scale = new Vector3[3]
        {
            new Vector3(0.05f, 0.05f, 0.05f),
            new Vector3(0.05f, 0.05f, 0.05f),
            new Vector3(0.05f, 0.05f, 0.05f)
        };
        int numTargets = 3;

        currConfig = new sceneConfig(pos, scale, numTargets);

        Transform[] ts = GetComponentsInChildren<Transform>();

        for (int i = 0; i < ts.Length; i++)
        {
            ts[i + 1].localScale = currConfig.targetScale[i];
            ts[i + 1].localPosition = currConfig.targetPos[i];
        }
    }

    public void OnBtn1()
    {
        Debug.Log("Button 1 Pressed!");
        // Implement individual actions
        Vector3[] pos = new Vector3[3]
        {
            new Vector3(0f, 0f, -0.2f),
            new Vector3(0f, 0f, 0f),
            new Vector3(0f, 0f, 0.2f)
        };
        Vector3[] scale = new Vector3[3]
        {
            new Vector3(0.05f, 0.05f, 0.05f),
            new Vector3(0.1f, 0.1f, 0.1f),
            new Vector3(0.15f, 0.15f, 0.15f)
        };
        int numTargets = 3;

        currConfig = new sceneConfig(pos, scale, numTargets);

        Transform[] ts = GetComponentsInChildren<Transform>();

        for (int i = 0; i < ts.Length; i++)
        {
            ts[i + 1].localScale = currConfig.targetScale[i];
            ts[i + 1].localPosition = currConfig.targetPos[i];
        }
    }

    public void OnBtn2()
    {
        Debug.Log("Button 2 Pressed!");
        // Implement individual actions
        Vector3[] pos = { };
        Vector3[] scale = { };
        int numTargets = 3;

        currConfig = new sceneConfig(pos, scale, numTargets);

        Transform[] ts = GetComponentsInChildren<Transform>();

        for (int i = 0; i < ts.Length; i++)
        {
            ts[i + 1].localScale = currConfig.targetScale[i];
            ts[i + 1].localPosition = currConfig.targetPos[i];
        }
    }
}
