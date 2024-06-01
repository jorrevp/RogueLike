using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryUI : MonoBehaviour
{
    public Label[] labels = new Label[8];
    private VisualElement root;
    private int selected;
    private int numItems;
    public int Selected => selected;
    private void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        for (int i = 0; i < 8; i++)
        {
            labels[i] = root.Q<Label>($"Item{i + 1}");
        }
        Clear();
        root.style.display = DisplayStyle.None;
    }

    public void Clear()
    {
        foreach (var label in labels)
        {
            label.text = string.Empty;
        }
    }

    private void UpdateSelected()
    {
        for (int i = 0; i < labels.Length; i++)
        {
            if (i == selected)
            {
                labels[i].style.backgroundColor = Color.green;
            }
            else
            {
                labels[i].style.backgroundColor = Color.clear;
            }
        }
    }

    public void SelectNextItem()
    {
        if (selected < numItems - 1)
        {
            selected++;
        }
        else
        {
            selected = 0; // Wrap around to the first item
        }
        UpdateSelected();
    }

    public void SelectPreviousItem()
    {
        if (selected > 0)
        {
            selected--;
        }
        else
        {
            selected = numItems - 1; // Wrap around to the last item
        }
        UpdateSelected();
    }

    public void Show(List<Items.Consumable> list)
    {
        selected = 0;
        numItems = list.Count;
        Clear();

        for (int i = 0; i < numItems && i < labels.Length; i++)
        {
            labels[i].text = list[i].Type.ToString(); // Assuming Type has a meaningful name
        }

        UpdateSelected();
        root.style.display = DisplayStyle.Flex;
    }

    public void Hide()
    {
        root.style.display = DisplayStyle.None;
    }
}
