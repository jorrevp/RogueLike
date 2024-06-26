using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Actor), typeof(AStar))]
public class Enemy : MonoBehaviour
{
    public Actor Target;
    public bool IsFighting = false;
    public int confused = 0; // Toegevoegde variabele
    private AStar algorithm;

    private void Start()
    {
        GameManager.Get.AddEnemy(GetComponent<Actor>());
        algorithm = GetComponent<AStar>();
    }

    public void MoveAlongPath(Vector3Int targetPosition)
    {
        Vector3Int gridPosition = MapManager.Get.FloorMap.WorldToCell(transform.position);
        Vector2 direction = algorithm.Compute((Vector2Int)gridPosition, (Vector2Int)targetPosition);
        Action.Move(GetComponent<Actor>(), direction);
    }

    public void RunAI()
    {
        // If target is null, set target to player (from gameManager)
        if (Target == null)
        {
            Target = GameManager.Get.Player;
        }

        // convert the position of the target to a gridPosition
        var gridPosition = MapManager.Get.FloorMap.WorldToCell(Target.transform.position);

        // First check if already fighting, because the FieldOfView check costs more cpu
        if (IsFighting || GetComponent<Actor>().FieldOfView.Contains(gridPosition))
        {
            // If the enemy was not fighting, is should be fighting now
            if (!IsFighting)
            {
                IsFighting = true;
            }

            // See how far away the player is
            float targetDistance = Vector3.Distance(transform.position, Target.transform.position);

            // if close ...
            if (targetDistance <= 1.5f)
            {
                // ... hit!
                Action.Hit(GetComponent<Actor>(), Target);
            }
            else
            {
                // call MoveAlongPath with the gridPosition
                MoveAlongPath(gridPosition);
            }
        }
    }
    public void Confuse()
    {
        confused = 8;
    }
} 

