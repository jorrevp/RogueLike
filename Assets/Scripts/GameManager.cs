using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public static GameManager Get { get => instance; }

    public Actor Player;
    public List<Actor> Enemies = new List<Actor>();
    public List<Items.Consumable> Items = new List<Items.Consumable>();

    public GameObject CreateGameObject(string name, Vector2 position)
    {
        GameObject actor = Instantiate(Resources.Load<GameObject>($"Prefabs/{name}"), new Vector3(position.x + 0.5f, position.y + 0.5f, 0), Quaternion.identity);
        actor.name = name;
        return actor;
    }

    public void AddEnemy(Actor enemy)
    {
        Enemies.Add(enemy);
    }

    public void RemoveEnemy(Actor enemy)
    {
        if (Enemies.Contains(enemy))
        {
            Enemies.Remove(enemy);
        }
    }

    public void StartEnemyTurn()
    {
        foreach (var enemy in Enemies)
        {
            enemy.GetComponent<Enemy>().RunAI();
        }
    }

    public Actor GetActorAtLocation(Vector3 location)
    {
        if (Player.transform.position == location)
        {
            return Player;
        }
        else
        {
            foreach (Actor enemy in Enemies)
            {
                if (enemy.transform.position == location)
                {
                    return enemy;
                }
            }
        }
        return null;
    }
    // Function to add an item to the list
    public void AddItem(Items.Consumable item)
    {
        Items.Add(item);
    }

    // Function to remove an item from the list
    public void RemoveItem(Items.Consumable item)
    {
        Items.Remove(item);
    }

    // Function to get the item at a specific location in the list
    public Items.Consumable GetItemAtLocation(int index)
    {
        if (index >= 0 && index < Items.Count)
        {
            return Items[index];
        }
        else
        {
            Debug.LogError("Index out of range.");
            return null;
        }
    }
}
