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

        public ItemType Type
        {
            get { return type; }
        }
        private void Start()
        {
            GameManager gameManager = GameManager.Get;
            gameManager.AddItem(this);
        }
    }
}

