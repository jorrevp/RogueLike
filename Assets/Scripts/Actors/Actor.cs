using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Actor : MonoBehaviour
{
    private AdamMilVisibility algorithm;

    [Header("FieldOfView")]
    public List<Vector3Int> FieldOfView = new List<Vector3Int>();
    public int FieldOfViewRange = 8;

    [Header("Powers")]
    [SerializeField] private int maxHitPoints;
    [SerializeField] private int hitPoints;
    [SerializeField] private int defense;
    [SerializeField] private int power;
    [SerializeField] private int level = 1;
    [SerializeField] private int xp = 0;
    [SerializeField] private int xpToNextLevel = 100;

    public int MaxHitPoints { get => maxHitPoints; }
    public int HitPoints { get => hitPoints; }
    public int Defense { get => defense; }
    public int Power { get => power; }
    public int Level { get => level; }
    public int XP { get => xp; }
    public int XPToNextLevel { get => xpToNextLevel; }

    private void Start()
    {
        algorithm = new AdamMilVisibility();
        UpdateFieldOfView();

        if (GetComponent<Player>())
        {
            UIManager.Get.UpdateHealth(HitPoints, MaxHitPoints);
            UIManager.Get.UpdateLevel(Level);
            UIManager.Get.UpdateXP(XP);
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

    public void DoDamage(int hp, Actor attacker)
    {
        hitPoints -= hp;

        if (hitPoints < 0) hitPoints = 0;

        if (GetComponent<Player>())
        {
            UIManager.Get.UpdateHealth(hitPoints, MaxHitPoints);
        }
        if (hitPoints == 0)
        {
            if (attacker != null && attacker.GetComponent<Player>())
            {
                attacker.AddXp(xp);
            }
            Die();
        }
    }
    public void Heal(int hp)
    {
        int effectiveHealing = Mathf.Min(maxHitPoints - hitPoints, hp);
        hitPoints += effectiveHealing;

        if (GetComponent<Player>())
        {
            UIManager.Get.UpdateHealth(hitPoints, MaxHitPoints);
            UIManager.Get.AddMessage($"You were healed for {effectiveHealing}!", Color.green); //Green
        }
    }
    public void AddXp(int amount)
    {
        xp += amount;
        Debug.Log($"XP added: {amount}, Current XP: {xp}");

        while (xp >= xpToNextLevel)
        {
            xp -= xpToNextLevel;
            level++;
            xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * 1.5f); // Exponentially increase XP required for next level
            maxHitPoints += 10; // Example increase, adjust as needed
            defense += 2; // Example increase, adjust as needed
            power += 3; // Example increase, adjust as needed

            if (GetComponent<Player>())
            {
                UIManager.Get.UpdateHealth(hitPoints, MaxHitPoints);
                UIManager.Get.UpdateLevel(level);
                UIManager.Get.AddMessage("You have leveled up!", Color.yellow); // Yellow
                Debug.Log($"Level up! New Level: {level}");
            }
        }

        if (GetComponent<Player>())
        {
            UIManager.Get.UpdateXP(xp);
            Debug.Log($"UI updated with new XP: {xp}");
        }
    }
    private void Die()
    {
        if (GetComponent<Player>())
        {
            UIManager.Get.AddMessage("You died!", Color.red); //Red
        }
        else
        {
            UIManager.Get.AddMessage($"{name} is dead!", Color.green); //Light Orange
        }
        GameManager.Get.CreateGameObject("Dead", transform.position).name = $"Remains of {name}";
        GameManager.Get.RemoveEnemy(this);
        Destroy(gameObject);
    }
}
