using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    private AdamMilVisibility algorithm;
    public List<Vector3Int> FieldOfView = new List<Vector3Int>();
    public int FieldOfViewRange = 8;

    [Header("Powers")]
    [SerializeField] private int maxHitPoints;
    [SerializeField] private int hitPoints;
    [SerializeField] private int defense;
    [SerializeField] private int power;

    public int MaxHitPoints { get { return maxHitPoints; } }
    public int HitPoints { get { return hitPoints; } }
    public int Defense { get { return defense; } }
    public int Power { get { return power; } }

    private void Start()
    {
        algorithm = new AdamMilVisibility();
        UpdateFieldOfView();
        if (this.GetComponent<Player>() != null)
        {
            UIManager.Instance.UpdateHealth(hitPoints, maxHitPoints);
        }
    }

    public void Move(Vector3 direction)
    {
        if (MapManager.Get.IsWalkable(transform.position + direction))
        {
            transform.position += direction;
        }
    }

    public void UpdateFieldOfView()
    {
        var pos = MapManager.Get.FloorMap.WorldToCell(transform.position);

        FieldOfView.Clear();
        algorithm.Compute(pos, FieldOfViewRange, FieldOfView);

        if (GetComponent<Player>())
        {
            MapManager.Get.UpdateFogMap(FieldOfView);
        }
    }
    private void Die()
    {
        string message = "";

        if (this.GetComponent<Player>())
        {
            message = "You died!";
            UIManager.Instance.AddMessage(message, Color.red);
        }
        else
        {
            message = $"{gameObject.name} is dead!";
            UIManager.Instance.AddMessage(message, Color.green);
        }

        GameManager.Get.CreateActor("Dead", transform.position);

        if (this.GetComponent<Enemy>())
        {
            GameManager.Get.RemoveEnemy(this);
        }

        Destroy(gameObject);
    }
    public void DoDamage(int hp)
    {
        hitPoints -= hp;

        if (hitPoints < 0)
        {
            hitPoints = 0;
        }

        if (this.GetComponent<Player>())
        {
            UIManager.Instance.UpdateHealth(hitPoints, maxHitPoints);
        }

        if (hitPoints == 0)
        {
            Die();
        }
    }
}
