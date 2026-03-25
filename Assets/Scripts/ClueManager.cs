using System.Collections;
using UnityEngine;

public class ClueManager : MonoBehaviour
{
    public ClueData currentClue;

    private bool isCloseTriggered = false;

    public void StartTrail()
    {
        Debug.Log("Trail Started!");

        StartCoroutine(StartLocationAndGame());
    }

    private IEnumerator StartLocationAndGame()
    {
        Input.location.Start();

        while (Input.location.status == LocationServiceStatus.Initializing)
            yield return null;

        if (Input.location.status != LocationServiceStatus.Running)
        {
            Debug.Log("GPS failed");
            yield break;
        }

        Debug.Log("GPS Started");

        // Show first clue
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
        while (true)
        {
            var data = Input.location.lastData;

            float distance = GPSUtils.GetDistance(
                data.latitude,
                data.longitude,
                currentClue.latitude,
                currentClue.longitude
            );

            Debug.Log("Distance: " + distance);

            if (!isCloseTriggered && distance <= currentClue.triggerDistanceMetres)
            {
                isCloseTriggered = true;
                OnPlayerIsClose();
            }

            yield return new WaitForSeconds(2f);
        }
    }
}