using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Actor), typeof(AStar))]
public class Enemy : MonoBehaviour
{
    public Actor Target { get; private set; }
    public bool IsFighting { get; private set; } = false;
    public AStar Algorithm { get; private set; }

    private void Start()
    {
        Algorithm = GetComponent<AStar>();
    }

    public void MoveAlongPath(Vector3Int targetPosition)
    {
        Vector3Int gridPosition = MapManager.Get.FloorMap.WorldToCell(transform.position);
        Vector2 direction = Algorithm.Compute((Vector2Int)gridPosition, (Vector2Int)targetPosition);
        Action.MoveOrHit(GetComponent<Actor>(), direction); 
    }

    public void RunAI()
    {
        // If target is null, set target to player (from GameManager)
        if (Target == null)
        {
            Target = GameManager.Get.Player;
        }

        // Convert the position of the target to a gridPosition
        Vector3Int gridPosition = MapManager.Get.FloorMap.WorldToCell(Target.transform.position);

        // First check if already fighting, because the FieldOfView check costs more CPU
        if (IsFighting || GetComponent<Actor>().FieldOfView.Contains(gridPosition))
        {
            // If the enemy was not fighting, it should be fighting now
            if (!IsFighting)
            {
                IsFighting = true;
            }

            // Call MoveAlongPath with the gridPosition
            MoveAlongPath(gridPosition);
        }
    }
}

