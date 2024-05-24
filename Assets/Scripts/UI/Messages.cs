using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Messages : MonoBehaviour
{
    private Label[] labels = new Label[5];
    private VisualElement root;
    void Start()
    {
        // Assign values to variables
        root = GetComponent<VisualElement>();

        // Clear labels
        Clear();

        // Add initial message
        AddMessage("Welcome to the dungeon, Adventurer!", Color.white);
    }
    public void Clear()
    {
        foreach (Label label in labels)
        {
            label.text = "";
        }
    }
    public void MoveUp()
    {
        for (int i = labels.Length - 1; i > 0; i--)
        {
            labels[i].text = labels[i - 1].text;
            labels[i].style.color = labels[i - 1].style.color;
        }
    }
    public void AddMessage(string content, Color color)
    {
        MoveUp();
        labels[0].text = content;
        labels[0].style.color = color;
    }
}

