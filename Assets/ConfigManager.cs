using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.OpenXR.Input;

public class ConfigManager : MonoBehaviour
{
    //initial values for targets
    //TODO: populate Vec3s
    private Vector3[] pos = { };
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
        Transform[] ts = GetComponentsInChildren<Transform>();

        for (int i = 0; i < currConfig.numTargets; i++) 
        {
            ts[i].localScale = currConfig.targetScale[i];
            ts[i].position = currConfig.targetPos[i];
        }
    }

    public void OnBtn0()
    {
        Debug.Log("Button 0 Pressed!");
        // Implement individual actions
        Vector3[] pos = { };
        Vector3[] scale = { };
        int numTargets = 3;

        currConfig = new sceneConfig(pos, scale, numTargets);
    }
    public void OnBtn1()
    {
        Debug.Log("Button 0 Pressed!");
        // Implement individual actions
        Vector3[] pos = { };
        Vector3[] scale = { };
        int numTargets = 3;

        currConfig = new sceneConfig(pos, scale, numTargets);
    }
    public void OnBtn2()
    {
        Debug.Log("Button 0 Pressed!");
        // Implement individual actions
        Vector3[] pos = { };
        Vector3[] scale = { };
        int numTargets = 3;

        currConfig = new sceneConfig(pos, scale, numTargets);
    }
}
