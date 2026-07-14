using System.Collections;
using UnityEngine;
using System.Collections.Generic;
public class ClueManager : MonoBehaviour
{
    private ClueData currentClue;
    public List<ClueData> ClueList;
    public List<ClueData> routeList;
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


        routeList = GenerateClueTrail();

        GenerateNextTarget();

        StartCoroutine(CheckDistanceLoop());
    }

    void OnPlayerIsClose()
    {
        Debug.Log("You're close!");

        // Show popup UI
        UIManager.Instance.ShowClosePopup(() =>
        {
            //UIManager.Instance.ShowQuestion(currentClue);
            UIManager.Instance.ShowPuzzle(currentClue);

        });
    }

    public void SubmitAnswer(string answer)
    {
        if (answer.ToLower() == currentClue.correctAnswer.ToLower())
        {
            Debug.Log("Correct!");
            UIManager.Instance.NextCluePanelShift();
            GenerateNextTarget();
            UIManager.Instance.ShowBadge(currentClue.badgeName);
        }
        else
        {
            Debug.Log("Wrong answer");
        }
    }

    public void CheckCurrentPuzzle()
    {
        switch (currentClue.puzzleType)
        {
            case PuzzleType.TextAnswer:
            CheckTextAnswer();
            break;


            case PuzzleType.DragAndDrop:
            CheckTimelineAnswer();
            break;


            //case PuzzleType.MultipleChoice:
            ////CheckMultipleChoiceAnswer();
            //break;


            //case PuzzleType.PhotoTask:
            ////CheckPhotoAnswer();
            //break;
        }
    }
    public void PuzzleCompleted()
    {
        Debug.Log("Correct!");

        UIManager.Instance.ShowBadge(currentClue.badgeName);

        UIManager.Instance.NextCluePanelShift();

        GenerateNextTarget();
    }

    private void CheckTextAnswer()
    {
        string answer = UIManager.Instance.answerInput.text;


        if (answer.ToLower() == currentClue.correctAnswer.ToLower())
        {
            PuzzleCompleted();
        }
        else
        {
            Debug.Log("Wrong answer");
        }
    }

    private void CheckTimelineAnswer()
    {
        bool correct =
            UIManager.Instance.timelinePuzzle.CheckAnswer();


        if (correct)
        {
            PuzzleCompleted();
        }
        else
        {
            Debug.Log("Wrong timeline");
        }
    }
    private IEnumerator CheckDistanceLoop()
    {
        latitude = 53.8008;
        longitude = -1.5491;

        while (true)
        {
            if (currentClue == null)
            {
                yield break;
            }

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

    private List<ClueData> GenerateClueTrail()
    {
        List<ClueData> remaining = new List<ClueData>(ClueList);
        List<ClueData> route = new List<ClueData>();

        // Start from player position
        double currentLat = latitude;
        double currentLon = longitude;

        while (remaining.Count > 0)
        {
            ClueData closestClue = null;
            float closestDistance = float.MaxValue;

            foreach (ClueData clue in remaining)
            {

                //TODO Check for badge to see if clue has already been implemented 

                float distance = GPSUtils.GetDistance(
                    currentLat,
                    currentLon,
                    clue.latitude,
                    clue.longitude
                );

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestClue = clue;
                }
            }

            // Add closest clue to route
            route.Add(closestClue);

            // Move "current position" to that clue
            currentLat = closestClue.latitude;
            currentLon = closestClue.longitude;

            // Remove from remaining list
            remaining.Remove(closestClue);
        }

        Debug.Log("Generated Trail Order:");
        for (int i = 0; i < route.Count; i++)
        {
            Debug.Log($"{i + 1}: {route[i].clueText}");
        }

        return route;
    }
    public ClueData NextClue()
    {
        if (routeList.Count == 0)
        {
            Debug.Log("Trail complete!");
            return null;
        }

        ClueData clue = routeList[0];
        routeList.RemoveAt(0);

        return clue;
    }
    //public void GenerateNextTarget()
    //{
    //    currentClue = NextClue();
    //    isCloseTriggered = false;
    //    UIManager.Instance.ShowClue(currentClue.clueText);
    //}
    public void GenerateNextTarget()
    {
        currentClue = NextClue();

        if (currentClue == null)
        {
            Debug.Log("Trail Complete!");

            // TODO
            // Show end screen
            // Final anagram
            // Treasure
            return;
        }

        isCloseTriggered = false;

        UIManager.Instance.ShowClue(currentClue.clueText);
    }

    public void DebugLocation()
    {
        longitude = currentClue.longitude;
        latitude = currentClue.latitude;
    }
}