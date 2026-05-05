using System.Collections;
using UnityEngine;
using TMPro; // Required for modern Unity UI text

public class CountdownTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private int startSeconds = 5;

    void Start()
    {
        StartCoroutine(BeginCountdown());
    }

    private IEnumerator BeginCountdown()
    {
        int currentTime = startSeconds;

        while (currentTime > 0)
        {
            countdownText.text = currentTime.ToString();
            yield return new WaitForSeconds(1f);
            currentTime--;
        }

        countdownText.text = "GO!";
        yield return new WaitForSeconds(1f);
        countdownText.gameObject.SetActive(false);


    }
}