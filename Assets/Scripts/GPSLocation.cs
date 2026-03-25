using System.Collections;
using UnityEngine;
using UnityEngine.Android;

public class GPSLocation : MonoBehaviour
{
    private string logText = "";

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        logText = logString + "\n" + logText;

        // Limit log size
        if (logText.Length > 2000)
        {
            logText = logText.Substring(0, 2000);
        }
    }

    IEnumerator Start()
    {
       
#if UNITY_ANDROID && !UNITY_EDITOR
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Debug.Log("Requesting location permission...");
            Permission.RequestUserPermission(Permission.FineLocation);

            // Wait a moment for user to respond
            yield return new WaitForSeconds(2f);
        }
#endif

        // Check if location services are enabled
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("Location services are disabled by the user.");
            yield break;
        }

        // Start location service
        Input.location.Start(1f, 1f);

        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            Debug.Log("Initializing location service...");
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (maxWait <= 0)
        {
            Debug.Log("Timed out while initializing location services.");
            yield break;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("Failed to get location.");
            yield break;
        }

        Debug.Log("Location service started!");

        // Update loop
        while (true)
        {
            LocationInfo info = Input.location.lastData;

            Debug.Log($"Lat: {info.latitude}");
            Debug.Log($"Lon: {info.longitude}");
            Debug.Log($"Alt: {info.altitude}");
            Debug.Log($"Accuracy: {info.horizontalAccuracy}");
            Debug.Log($"Time: {info.timestamp}");

            yield return new WaitForSeconds(5f);
        }
    }

    void OnDestroy()
    {
        Input.location.Stop();
    }

    void OnGUI()
    {
        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.fontSize = 30;
        style.normal.textColor = Color.white;

        GUI.Label(new Rect(10, 10, Screen.width - 20, Screen.height), logText, style);
    }
}