using System.Collections;
using UnityEngine;
using TMPro; // Required for modern Unity UI text

public class CountdownTimer : MonoBehaviour
{
    // Drag your UI Text element here in the Inspector
    public TextMeshProUGUI countdownText;

    // You can change this to 3 or 10 in the Inspector if you want
    public int startSeconds = 5;

    void Start()
    {
        // Start the countdown as soon as the object loads
        StartCoroutine(BeginCountdown());
    }

    private IEnumerator BeginCountdown()
    {
        int currentTime = startSeconds;

        // Loop as long as currentTime is greater than 0
        while (currentTime > 0)
        {
            // Update the text on screen
            countdownText.text = currentTime.ToString();

            // Wait exactly 1 second
            yield return new WaitForSeconds(1f);

            // Subtract 1 from the timer
            currentTime--;
        }

        // When the loop finishes (hits 0), show GO!
        countdownText.text = "GO!";

        // Optional: Wait 1 more second, then hide the text completely
        yield return new WaitForSeconds(1f);
        countdownText.gameObject.SetActive(false);

        // ---> YOU CAN ADD YOUR GAME START LOGIC HERE <---
    }
}