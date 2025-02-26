using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ExperimentManager : MonoBehaviour
{
    // Targets
    [SerializeField] private Cursor collisionSender;
    [SerializeField] private TextMeshProUGUI resultsText;
    private float[] targetHitTimes;
    private GameObject[] targets;
    private int targetHitCount;
    private float paintStartTime;
    private float RESTART_EXPERIMENT_TIME = 10;

    void Awake()
    {
        StartExperiment();
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        if (collisionSender != null)
        {
            collisionSender.OnSuccessfulCollision.AddListener(HandleSuccessfulCollision);
            Debug.Log("ExperimentManager OnEnable");
        }
        
    }

    // Update is called once per frame
    void OnDisable()
    {
        if (collisionSender != null)
        {
            collisionSender.OnSuccessfulCollision.RemoveListener(HandleSuccessfulCollision);
        }
    }

    private void HandleSuccessfulCollision(GameObject target, float collisionTime)
    {
        Debug.Log("Received CollisionEvent, checking against " + targets.Length + " against " + target.name);
        // Loop through the targets array to find the matching target.
        for (int i = 0; i < targets.Length; i++)
        {
            Debug.Log("Checking " + targets[i]+ " against " + target.name);
            if (targets[i]==target)
            {
                // Record the hit time if this target hasn't been hit before.
                if (targetHitTimes[i] < 0)
                {
                    targetHitTimes[i] = (paintStartTime == 0) ? 0 : Time.time - this.paintStartTime;
                    Debug.Log($"Recorded collision for target {target.name}: Time = {targetHitTimes[i]}");
                }
                else
                {
                    Debug.Log($"Target {target.name} was already hit with collision time {targetHitTimes[i]}");
                }
                break;
            }
        }

        // Check if all targets have been hit.
        int allTargetsHit = 0;
        for (int i = 0; i < targetHitTimes.Length; i++)
        {
            if (targetHitTimes[i] >= 0)
            {
                allTargetsHit++;
            }
        }
        if ((this.targetHitCount == 0) && (allTargetsHit == 1))
        {
            this.paintStartTime = Time.time;
            Debug.Log("Experiment starting at time " + this.paintStartTime);
        }
        this.targetHitCount = allTargetsHit;

        // If all targets are hit, update the UI element with their collision times.
        if (this.targetHitCount == targets.Length)
        {
            string results = "Collision Times:\n";
            for (int i = 0; i < targetHitTimes.Length; i++)
            {
                results += $"{targets[i].name}: {targetHitTimes[i]:F2} seconds\n";
            }
            resultsText.text = results;
            Debug.Log("All targets have been hit. Results updated.");
            Invoke("StartExperiment", RESTART_EXPERIMENT_TIME);
        }
    }

    void StartExperiment()
    {
        // Automatically populate the targets array with all objects tagged "Target"
        this.targets = GameObject.FindGameObjectsWithTag("Target");
        Debug.Log("Found " + targets.Length + " targets");

        for (int i = 0; i < targets.Length; i++)
            targets[i].GetComponent<Renderer>().material.color = Color.white;
        
        resultsText.text = "NEW EXPERIMENT!";

        // Initialize your hit times array based on the number of targets
        if (targetHitTimes == null)
            targetHitTimes = new float[targets.Length];

        for (int i = 0; i < targetHitTimes.Length; i++)
        {
            targetHitTimes[i] = -1f; // -1 indicates the target hasn't been hit
        }
        this.targetHitCount = 0;
        this.paintStartTime = 0.0f;
    }
}
