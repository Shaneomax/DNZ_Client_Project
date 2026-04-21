using UnityEngine;
using UnityEngine.UI;     // Required for UI components
using DrawMesh;           // Your namespace

public class InkUI : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("Drag your UI Slider here")]
    public Slider inkSlider;

    private void Start()
    {
        // Auto-grab the slider if it's attached to the same object
        if (inkSlider == null)
        {
            inkSlider = GetComponent<Slider>();
        }

        // Ensure the slider goes from 0 to 1
        if (inkSlider != null)
        {
            inkSlider.minValue = 0f;
            inkSlider.maxValue = 1f;
        }
    }

    private void Update()
    {
        // Make sure the InkSystem instance exists before trying to read from it
        if (InkSystem.Instance != null && inkSlider != null)
        {
            // Because InkPercent represents the amount of ink USED (0 to 1), 
            // we subtract it from 1 to get the REMAINING ink percentage.
            inkSlider.value = 1f - InkSystem.Instance.InkPercent;
        }
    }
}