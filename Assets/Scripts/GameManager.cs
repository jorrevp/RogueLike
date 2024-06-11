using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;


    private void Awake()
    {
        floorInfo = FindObjectOfType<FloorInfoUI>();

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
    public List<Ladder> Ladders = new List<Ladder>();
    public FloorInfoUI floorInfo;
    private List<Tombstone> tombStones = new List<Tombstone>();

    private Dictionary<Vector2Int, Items.Consumable> itemPositions = new Dictionary<Vector2Int, Items.Consumable>();
    

    private int enemiesCount;

    public GameObject CreateGameObject(string name, Vector2 position)
    {
        GameObject actor = Instantiate(Resources.Load<GameObject>($"Prefabs/{name}"), new Vector3(position.x + 0.5f, position.y + 0.5f, 0), Quaternion.identity);
        actor.name = name;
        return actor;
    }

    public void AddEnemy(Actor enemy)
    {
        Enemies.Add(enemy);
        /*UIManager.Get.floorInfo.UpdateEnemiesText(Enemies.Count);*/

    }

    public void RemoveEnemy(Actor enemy)
    {
        if (Enemies.Contains(enemy))
        {
            Enemies.Remove(enemy);
            /*UIManager.Get.floorInfo.UpdateEnemiesText(Enemies.Count);*/
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
    public void AddLadder(Ladder ladder)
    {
        Ladders.Add(ladder);
    }

    public void ClearLadders()
    {
        Ladders.Clear();
    }
    public Ladder GetLadderAtLocation(Vector3 location)
    {
        foreach(var ladder in Ladders)
        {
            if (Vector3.Distance(ladder.transform.position, location) < 0.1f)
            {
                return ladder;
            }           
        }
        return null;
    }
    // Method to spawn a ladder
    public void SpawnLadder(Vector3 position, bool isUp)
    {
        string ladderPrefabName = isUp ? "LadderUp" : "LadderDown";
        GameObject ladderObject = CreateGameObject(ladderPrefabName, position);
        Ladder ladder = ladderObject.GetComponent<Ladder>();
        ladder.Up = isUp;
        AddLadder(ladder);
    }

    // Function to add a TombStone to the list
    public void AddTombStone(Tombstone stone)
    {
        tombStones.Add(stone);
    }
    public void ClearFloor()
    {
        // Destroy all enemies
        foreach (Actor enemy in Enemies)
        {
            Destroy(enemy.gameObject);
        }
        Enemies.Clear();

        // Destroy all items
        foreach (var item in itemPositions.Values)
        {
            Destroy(item.gameObject);
        }
        itemPositions.Clear();

        // Destroy all ladders
        foreach (var ladder in Ladders)
        {
            Destroy(ladder.gameObject);
        }
        Ladders.Clear();

        // Destroy all tombstones
        foreach (var tombstone in tombStones)
        {
            Destroy(tombstone.gameObject);
        }
        tombStones.Clear();
    }

    private int maxHitPoints;
    private int hitPoints;
    private int defense;
    private int power;
    private int level;
    private int xp;
    private int xpToNextLevel;
    private void Start()
    {
        // Load player properties if save data exists
        LoadPlayerData();
    }

    private void OnApplicationQuit()
    {
        // Save player data when the application quits
        SavePlayerData();
    }
    

    private void SavePlayerData()
    {
        // Save player properties to PlayerPrefs
        PlayerPrefs.SetInt("MaxHitPoints", maxHitPoints);
        PlayerPrefs.SetInt("HitPoints", hitPoints);
        PlayerPrefs.SetInt("Defense", defense);
        PlayerPrefs.SetInt("Power", power);
        PlayerPrefs.SetInt("Level", level);
        PlayerPrefs.SetInt("XP", xp);
        PlayerPrefs.SetInt("XpToNextLevel", xpToNextLevel);

        // Save PlayerPrefs data
        PlayerPrefs.Save();
    }

    private void LoadPlayerData()
    {
        // Load player properties from PlayerPrefs
        maxHitPoints = PlayerPrefs.GetInt("MaxHitPoints", maxHitPoints);
        hitPoints = PlayerPrefs.GetInt("HitPoints", hitPoints);
        defense = PlayerPrefs.GetInt("Defense", defense);
        power = PlayerPrefs.GetInt("Power", power);
        level = PlayerPrefs.GetInt("Level", level);
        xp = PlayerPrefs.GetInt("XP", xp);
        xpToNextLevel = PlayerPrefs.GetInt("XpToNextLevel", xpToNextLevel);
    }

    public void PlayerDied()
    {
        // Remove save game when player dies
        PlayerPrefs.DeleteAll();
    }

}
