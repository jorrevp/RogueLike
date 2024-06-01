using Items;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Consumable> Items = new List<Consumable>();
    public int MaxItems = 10;

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
        }
        else
        {
            Debug.LogWarning("Item not found in inventory.");
        }
    }
    public bool IsFull()
    {
        return Items.Count >= MaxItems;
    }
    public Consumable GetItem(int index)
    {
        if (index >= 0 && index < Items.Count)
        {
            return Items[index];
        }
        else
        {
            Debug.LogWarning("Index out of range.");
            return null; // Ongeldige index
        }
    }

    // Functie om alle items in de inventaris op te halen
    public List<Consumable> GetItems()
    {
        return new List<Consumable>(Items);
    }
    public void RemoveItem(Consumable item)
    {
        if (Items.Contains(item))
        {
            Items.Remove(item);
        }
        else
        {
            Debug.LogWarning("Item not found in inventory.");
        }
    }
    public void UseItem(Consumable item)
    {
        switch (item.Type)
        {
            case Consumable.ItemType.HealthPotion:
                // Voer hier de logica uit voor het gebruik van een Health Potion
                Debug.Log("Using Health Potion. Restoring health...");
                // Hier kun je code toevoegen om de gezondheid van de speler te herstellen
                break;
            case Consumable.ItemType.Fireball:
                // Voer hier de logica uit voor het gebruik van een Fireball
                Debug.Log("Using Fireball. Casting spell...");
                // Hier kun je code toevoegen om een vuurbal te lanceren
                break;
            case Consumable.ItemType.ScrollOfConfusion:
                // Voer hier de logica uit voor het gebruik van een Scroll of Confusion
                Debug.Log("Using Scroll of Confusion. Confusing enemies...");
                // Hier kun je code toevoegen om vijanden te verwarren
                break;
            default:
                Debug.LogWarning("Unknown consumable type.");
                break;
        }
    }
}

