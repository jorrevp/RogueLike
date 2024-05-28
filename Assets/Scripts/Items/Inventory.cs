using Items;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Consumable> Items = new List<Consumable>();
    public int MaxItems = 10; // Default maximum items

    // Function to add an item to the inventory
    public bool AddItem(Consumable item)
    {
        if (Items.Count < MaxItems)
        {
            Items.Add(item);
            return true; // Item added successfully
        }
        else
        {
            Debug.LogWarning("Inventory is full. Cannot add item.");
            return false; // Failed to add item due to inventory full
        }
    }

    // Function to drop an item from the inventory
    public void DropItem(Consumable item)
    {
        if (Items.Contains(item))
        {
            Items.Remove(item);
            Debug.Log("Item dropped from inventory: " + item.gameObject.name);
        }
        else
        {
            Debug.LogWarning("Item not found in inventory: " + item.gameObject.name);
        }
    }
}

