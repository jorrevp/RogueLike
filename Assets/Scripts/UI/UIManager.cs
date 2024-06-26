using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static UIManager Get { get => instance; }

    [Header("Documents")]
    public GameObject HealthBar;
    public GameObject Messages;
    public GameObject InventoryUI;
    public FloorInfoUI floorInfo; // Add this line

    public InventoryUI Inventory { get => InventoryUI.GetComponent<InventoryUI>(); }

    public void UpdateHealth(int current, int max)
    {
        HealthBar.GetComponent<HealthBar>().SetValues(current, max);
    }
    public void UpdateLevel(int level)
    {
        HealthBar.GetComponent<HealthBar>().SetLevel(level);
    }
    public void UpdateXP(int xp)
    {
        Debug.Log($"Updating UI with XP: {xp}");
        HealthBar.GetComponent<HealthBar>().SetXP(xp);
    }
    public void AddMessage(string message, Color color)
    {
        Messages.GetComponent<Messages>().AddMessage(message, color);
    }
    public void UpdateFloor(int floornumber)
    {
        floorInfo.UpdateFloorText(floornumber);
    }
}

