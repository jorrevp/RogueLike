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
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        Enemies = new List<Actor>();
    }

    public static GameManager Get { get => instance; }
    public List<Actor> Enemies { get; private set; }

    public Actor Player { get; set; }
    public void AddEnemy(Actor enemy)
    {
        Enemies.Add(enemy);
    }

    public Actor GetActorAtLocation(Vector3 location)
    {
        if (Player != null && Player.transform.position == location)
        {
            return Player;
        }

        foreach (var enemy in Enemies)
        {
            if (enemy.transform.position == location)
            {
                return enemy;
            }
        }

        return null;
    }

    public GameObject CreateActor(string name, Vector2 position)
    {
        GameObject actor = Instantiate(Resources.Load<GameObject>($"Prefabs/{name}"), new Vector3(position.x + 0.5f, position.y + 0.5f, 0), Quaternion.identity);
        actor.name = name;
        return actor;
    }
    public void StartEnemyTurn()
    {
        foreach (Actor enemy in Enemies)
        {
            Enemy enemyComponent = enemy.GetComponent<Enemy>();
            if (enemyComponent != null)
            {
                enemyComponent.RunAI();
            }
        }
    } 
}

