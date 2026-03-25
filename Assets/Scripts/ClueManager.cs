using System.Collections;
using UnityEngine;

public class ClueManager : MonoBehaviour
{
    public ClueData currentClue;

    private bool isCloseTriggered = false;
    public double latitude;
    public double longitude;
    public void StartTrail()
    {
        Debug.Log("Trail Started!");

        StartCoroutine(StartLocationAndGame());
    }

    private IEnumerator StartLocationAndGame()
    {
        yield return null; // ? ensures coroutine always yields at least once

#if !UNITY_EDITOR
    Input.location.Start();

    int maxWait = 10;

    while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
    {
        Debug.Log("Initializing GPS...");
        yield return new WaitForSeconds(1);
        maxWait--;
    }
#endif

        if (Input.location.status != LocationServiceStatus.Running)
        {
            Debug.LogWarning("GPS not running - using fallback (editor/spoofed location)");
        }
        else
        {
            Debug.Log("GPS Started");
        }

        Debug.Log("Clue: " + currentClue.clueText);
        UIManager.Instance.ShowClue(currentClue.clueText);

        StartCoroutine(CheckDistanceLoop());
    }

    void OnPlayerIsClose()
    {
        Debug.Log("You're close!");

        // Show popup UI
        UIManager.Instance.ShowClosePopup(() =>
        {
            UIManager.Instance.ShowQuestion(currentClue);
        });
    }

    public void SubmitAnswer(string answer)
    {
        if (answer.ToLower() == currentClue.correctAnswer.ToLower())
        {
            Debug.Log("Correct!");

            UIManager.Instance.ShowBadge(currentClue.badgeName);
        }
        else
        {
            Debug.Log("Wrong answer");
        }
    }
    private IEnumerator CheckDistanceLoop()
    {
        latitude = 53.8008;   
         longitude = -1.5491;

        while (true)
        {
        #if UNITY_EDITOR
            
      

        #else
        // ?? REAL GPS ON DEVICE
        var data = Input.location.lastData;
        latitude = data.latitude;
        longitude = data.longitude;
        #endif

            float distance = GPSUtils.GetDistance(
                latitude,
                longitude,
                currentClue.latitude,
                currentClue.longitude
            );

            Debug.Log($"Distance: {distance} | Lat: {latitude} | Lon: {longitude}");

            if (!isCloseTriggered && distance <= currentClue.triggerDistanceMetres)
            {
                isCloseTriggered = true;
                OnPlayerIsClose();
            }

            yield return new WaitForSeconds(2f);
        }
    }
}