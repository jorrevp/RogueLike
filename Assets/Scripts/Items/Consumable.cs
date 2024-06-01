using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public class Consumable : MonoBehaviour
    {
        public enum ItemType
        {
            HealthPotion,
            Fireball,
            ScrollOfConfusion
        }

        [SerializeField]
        private ItemType type;
        public Vector2Int Position { get; set; }
        public ItemType Type
        {
            get { return type; }
        }
        private void Start()
        {
            GameManager gameManager = GameManager.Get;
            Vector2Int itemPosition = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
            Position = itemPosition; // Set the position property
            gameManager.AddItem(this, Position);
        }
        public class HealthPotion : Consumable
        {
            public int HealAmount = 50; // Stel de hoeveelheid healing in die deze potion geeft
        }
        public class Fireball : Consumable
        {
            public int Damage = 30; // Stel de hoeveelheid schade in die deze fireball doet
        }
    }
}

