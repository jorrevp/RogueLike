using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
    private List<Enemy> enemies = new List<Enemy>();
    public List<Actor> Enemies = new List<Actor>();
    public List<Items.Consumable> Items = new List<Items.Consumable>();

    private Dictionary<Vector2Int, Items.Consumable> itemPositions = new Dictionary<Vector2Int, Items.Consumable>();

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
    public void AddItem(Items.Consumable item, Vector2Int position)
    {
        item.Position = position; // Set the item's position
        itemPositions[position] = item; // Add to dictionary
    }

    // Function to remove an item from the list
    public void RemoveItem(Items.Consumable item)
    {
       
        itemPositions.Remove(item.Position);
    }

    // Function to get the item at a specific location in the list
    public Items.Consumable GetItemAtLocation(Vector2Int location)
    {
        Debug.Log($"Checking for item at location: {location}");
        if (itemPositions.TryGetValue(location, out Items.Consumable item))
        {
            Debug.Log($"Found item at location: {location}");
            return item;
        }
        Debug.Log("No item found at this location.");
        return null;
    }
    public List<Actor> GetNearbyEnemies(Vector3 location)
    {
        List<Actor> nearbyEnemies = new List<Actor>();

        foreach (Enemy enemy in enemies)
        {
            if (Vector3.Distance(enemy.transform.position, location) < 5f)
            {
                nearbyEnemies.Add(enemy.GetComponent<Actor>());
            }
        }

        return nearbyEnemies;
    }
}
