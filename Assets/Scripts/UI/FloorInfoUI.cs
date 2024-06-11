using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FloorInfoUI : MonoBehaviour
{
    private VisualElement root;
    private Label enemiesLabel;
    private Label floorLabel;

    private void Start()
    {
        // Get the root VisualElement of the UI Document
        var uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;

        // Assign the VisualElements
        enemiesLabel = root.Q<Label>("Enemies");
        floorLabel = root.Q<Label>("Floor");

        // Optionally initialize the labels with default values
        UpdateEnemiesText(0); // Example default value
        UpdateFloorText(1); // Example default floor value
    }
    public void UpdateEnemiesText(int enemiesCount)
    {
        enemiesLabel.text = $"{enemiesCount} enemies left";
    }
    public void UpdateFloorText(int floorNumber)
    {
        floorLabel.text = $"Floor {floorNumber}";
    }
}
