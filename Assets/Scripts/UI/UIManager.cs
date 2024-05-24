using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;

    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("UIManager");
                _instance = go.AddComponent<UIManager>();
            }
            return _instance;
        }
    }

    private GameObject healthBar;
    private GameObject messages;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }

        // Find and assign HealthBar and Messages GameObjects
        healthBar = GameObject.Find("HealthBar");
        messages = GameObject.Find("Messages");
    }
    public void UpdateHealth(int current, int max)
    {
        if (healthBar != null)
        {
            VisualElement healthVisualElement = healthBar.GetComponent<VisualElement>();
            if (healthVisualElement != null)
            {
                Label healthLabel = healthVisualElement.Q<Label>("healthLabel");
                if (healthLabel != null)
                {
                    healthLabel.text = "Health: " + current + " / " + max;
                }
                else
                {
                    Debug.LogError("Text element not found within HealthBar GameObject!");
                }
            }
            else
            {
                Debug.LogError("VisualElement component not found on HealthBar GameObject!");
            }
        }
        else
        {
            Debug.LogError("HealthBar GameObject not found!");
        }
    }

    public void AddMessage(string message, Color color)
    {
        if (messages != null)
        {
            Messages messagesComponent = messages.GetComponent<Messages>();
            if (messagesComponent != null)
            {
                messagesComponent.AddMessage(message, color);
            }
            else
            {
                Debug.LogError("Messages component not found!");
            }
        }
        else
        {
            Debug.LogError("Messages GameObject not found!");
        }
    }
}

