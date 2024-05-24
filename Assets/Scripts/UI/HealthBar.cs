using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HealthBar : MonoBehaviour
{
    private VisualElement root;
    private VisualElement healthBar;
    private Label healthLabel;
    private void Start()
    {
        // Get the root VisualElement of the UI Document
        var uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;

        // Assign the VisualElements
        healthBar = root.Q<VisualElement>("HealthBar");
        healthLabel = root.Q<Label>("HealthLabel");

        // Optionally initialize the health bar values
        SetValues(30, 30); // Example values
    }

    public void SetValues(int currentHitPoints, int maxHitPoints)
    {
        // Calculate the percentage of health remaining
        float percent = (float)currentHitPoints / maxHitPoints * 100;

        // Update the width of the health bar
        healthBar.style.width = Length.Percent(percent);

        // Update the text of the health label
        healthLabel.text = $"{currentHitPoints}/{maxHitPoints} HP";
    }
}
