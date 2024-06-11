using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HealthBar : MonoBehaviour
{
    private VisualElement root;
    private VisualElement healthBar;
    private Label healthLabel;
    private Label levelLabel;
    private Label xpLabel;
    private void Start()
    {
        // Get the root VisualElement of the UI Document
        var uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;

        // Assign the VisualElements
        healthBar = root.Q<VisualElement>("HealthBar");
        healthLabel = root.Q<Label>("HealthLabel");
        levelLabel = root.Q<Label>("LevelLabel");
        xpLabel = root.Q<Label>("XPLabel");

        // Optionally initialize the health bar values
        SetValues(30, 30); // Example values
        SetLevel(1); // Example level value
        SetXP(0); // Example XP value
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
    public void SetLevel(int level)
    {
        // Update the text of the level label
        levelLabel.text = $"Level: {level}";
    }
    public void SetXP(int xp)
    {
        // Update the text of the XP label
        xpLabel.text = $"XP: {xp}";
    }
}