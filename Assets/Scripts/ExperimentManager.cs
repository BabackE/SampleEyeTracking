using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.IO;
using System.Text;


#if ENABLE_WINMD_SUPPORT || WINDOWS_UWP
using Windows.Stroage;
using System.Threading.Tasks;
#endif




public class ExperimentManager : MonoBehaviour
{
    public struct FittsLawMeasurements
    {
        public float vergenceDistance;
        public float sizeOfObject;
        public FittsLawMeasurements(float vd, float so)
        {
            vergenceDistance = vd;
            sizeOfObject = so;
        }
    }

    [SerializeField]
    private ExtendedEyeGazeDataProvider extendedEyeGazeDataProvider;


    // Targets
    [SerializeField] private Cursor collisionSender;
    [SerializeField] private TextMeshProUGUI resultsText;
    [SerializeField] private TextMeshProUGUI hideButtonText;
    [SerializeField] private Boolean hide = false;
    [SerializeField] private Vector3[] originalPos = new Vector3[3]
    {
        new Vector3(0f, 0f, -0.2f),
        new Vector3(0f, 0f, 0f),
        new Vector3(0f, 0f, 0.2f)
    };
    [SerializeField] private GameObject[] targets = new GameObject[3];

    private float[] targetHitTimes;
    private FittsLawMeasurements[] fittsLawDistanceOverSize;
    private int targetHitCount;
    private float paintStartTime;
    private float RESTART_EXPERIMENT_TIME = 10;


    // Trace Line
    [SerializeField] private bool isTrace = false;
    [SerializeField] private GameObject traceCube;
    [SerializeField] float moveDuration = 10f;
    private ExtendedEyeGazeDataProvider.VergenceReading vergeReading;

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

    // This method toggles the boolean value.
    public void ToggleHide()
    {
        hide = !hide;
        Debug.Log("Hide: " + hide);
        hideButtonText.text = hide ? "Show Cubes" : "Hide Cubes"; 

    }

    // Update is called once per frame
    void OnDisable()
    {
        if (collisionSender != null)
        {
            collisionSender.OnSuccessfulCollision.RemoveListener(HandleSuccessfulCollision);
        }
    }

    private void HandleSuccessfulCollision(GameObject target, float collisionTime, float vergenceDistance, float sizeOfObject)
    {
        if (isTrace) // Trace Experiment
        {
            if (target == traceCube && traceCube.GetComponent<Renderer>().material.color == Color.red)
            {
                // Start experiment
                StartCoroutine(StartTraceExperiment());

            }
        }
        else // Target Experiment
        {
            Debug.Log("Received CollisionEvent, checking against " + targets.Length + " against " + target.name);
            // Loop through the targets array to find the matching target.
            for (int i = 0; i < targets.Length; i++)
            {
                Debug.Log("Checking " + targets[i] + " against " + target.name);
                if (targets[i] == target)
                {
                    // Record the hit time if this target hasn't been hit before.
                    if (targetHitTimes[i] < 0)
                    {
                        targetHitTimes[i] = (paintStartTime == 0) ? 0 : Time.time - this.paintStartTime;
                        fittsLawDistanceOverSize[i].sizeOfObject = sizeOfObject;
                        fittsLawDistanceOverSize[i].vergenceDistance = vergenceDistance;
                        target.GetComponent<Renderer>().enabled = hide;
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
                    results += $"{targets[i].name}: {targetHitTimes[i]:F2} s, d={fittsLawDistanceOverSize[i].vergenceDistance:F2}, s={fittsLawDistanceOverSize[i].sizeOfObject:F2}, f={2 * fittsLawDistanceOverSize[i].vergenceDistance / fittsLawDistanceOverSize[i].sizeOfObject:F2}\n";
                }
                resultsText.text = results;
                Debug.Log("All targets have been hit. Results updated.");
            }
        }
    }

    public void SetupTraceExperiment()
    {
        // Hide all the target cubes
        for (int i = 0; i < targets.Length; i++)
        {
            targets[i].SetActive(false);
        }

        // Show trace line cube
        traceCube.SetActive(true);

        isTrace = true;
    }

    IEnumerator StartTraceExperiment()
    { 
        Debug.Log("Trace Experiment Started");

        Vector3 startPosition = traceCube.transform.localPosition;
        Vector3 endPosition = startPosition + new Vector3(0, 0, 1);

        List<string[]> vergencePositions = new List<string[]>();

        // Interporlate the tracecube position
        float timeElapsed = 0f;

        // While time has not reached the duration, move the cube
        while (timeElapsed < moveDuration)
        {
            // Lerp between the start and end positions based on the time elapsed
            traceCube.transform.localPosition = Vector3.Lerp(startPosition, endPosition, timeElapsed / moveDuration);

            // Increment time elapsed
            timeElapsed += Time.deltaTime;

            // Record eyegaze position
            vergeReading = extendedEyeGazeDataProvider.GetWorldSpaceBinocularVergence();
            if (vergeReading.IsValid)
            {
                //VergenceObject.transform.position = vergeReading.EyePosition + 1.2f * vergeReading.GazeDirection;
                //VergenceObject.transform.position = vergeReading.EyePosition + vergeReading.FocusPoint;
                if (vergeReading.FocusPoint.magnitude > 0.0f)
                {
                    Vector3 pos = vergeReading.FocusPoint;
                    string[] row = { timeElapsed.ToString(), pos.x.ToString(), pos.y.ToString(), pos.z.ToString() };
                    vergencePositions.Add(row);
                }
            }
            else
            {
                // Record null
            }

            // Wait for the next frame
            yield return null;
        }


        // Write to csv
        StringBuilder csvContent = new StringBuilder();
        string[] header = { "time", "x", "y", "z" };
        // Write the header row

        csvContent.AppendLine(string.Join(",", header));
        // Write each row of data
        foreach (var row in vergencePositions)
        {
            csvContent.AppendLine(string.Join(",", row));  // Join each element with a comma
        }


        // Get the current timestamp
        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");

        // Create the filename with the timestamp
        string fileName = $"vergencePositions_{timestamp}.csv";
        string filePath = SaveCSV(csvContent.ToString(), fileName);

        Debug.Log($"Writing to {filePath}");

        resultsText.text += $"\n Saved CSV to: {filePath}";

        // Reset traceCube
        traceCube.transform.localPosition = startPosition;
        traceCube.GetComponent<Renderer>().material.color = Color.white;
        // Wait for the next frame
        yield return null;
    }
#if ENABLE_WINMD_SUPPORT || WINDOWS_UWP
    public async Task<string> SaveCSV(string csvContent, string filename)
    {
        return await SaveFileUWP(csvContent, filename);
    }
#else
    public string SaveCSV(string csvContent, string filename)
    { 
        return SaveFileEditor(csvContent, filename);

    }
#endif
#if ENABLE_WINMD_SUPPORT || WINDOWS_UWP
    private async Task<string> SaveFileUWP(string csvContent, string filename)
    {
        try 
        {
            StorageFolder documentFolder = KnownFolders.DocumentsLibrary;
            StorageFile csvFile = await documentFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(csvFile, csvContent);
            return csvFile.Path;
        }
        catch(Exception ex)
        {
            return ex.Message;
        }
    }
#else
    private string SaveFileEditor(string csvContent, string filename)
    {
        string path = Path.Combine(Application.persistentDataPath, filename);
        File.WriteAllText(path, csvContent);
        return path;
    }
#endif

    public void StartExperiment()
    {
        traceCube.SetActive(false);
        isTrace = false;

        for (int i = 0; i < targets.Length; i++)
        {
            targets[i].SetActive(true);
            targets[i].GetComponent<Renderer>().enabled = true;
            targets[i].GetComponent<Renderer>().material.color = Color.white;
            targets[i].transform.localScale = Vector3.one * 0.05f;
            targets[i].transform.localPosition = originalPos[i];
        }
        
        resultsText.text = "NEW EXPERIMENT!";

        // Initialize your hit times array based on the number of targets
        if (targetHitTimes == null)
        {
            targetHitTimes = new float[targets.Length];
            fittsLawDistanceOverSize = new FittsLawMeasurements[targets.Length];
        }

        for (int i = 0; i < targetHitTimes.Length; i++)
        {
            targetHitTimes[i] = -1f; // -1 indicates the target hasn't been hit
            fittsLawDistanceOverSize[i] = new FittsLawMeasurements(0f,0f);
        }
        this.targetHitCount = 0;
        this.paintStartTime = 0.0f;
    }
}
