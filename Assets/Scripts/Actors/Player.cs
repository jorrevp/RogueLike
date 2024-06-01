using Items;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Actor))]
public class Player : MonoBehaviour, Controls.IPlayerActions
{
    private Controls controls;
    private Actor playerActor;
    private Inventory inventory;
    private UIManager uiManager;

    private bool inventoryIsOpen = false;
    private bool droppingItem = false;
    private bool usingItem = false;


    private void Awake()
    {
        controls = new Controls();
        playerActor = GetComponent<Actor>();
        inventory = new Inventory();
    }

    private void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -5);
        GameManager.Get.Player = GetComponent<Actor>();
    }

    private void OnEnable()
    {
        if (controls == null)
        {
            Debug.LogError("Controls not initialized. Ensure the PlayerControls is properly instantiated in Awake.");   
            return;
        }

        controls.Player.SetCallbacks(this);
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Player.SetCallbacks(null);
        controls.Disable();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (inventoryIsOpen)
            {
                Vector2 direction = controls.Player.Movement.ReadValue<Vector2>();
                if (direction.y > 0)
                {
                    uiManager.InventoryUIComponent.SelectPreviousItem();
                }
                else if (direction.y < 0)
                {
                    uiManager.InventoryUIComponent.SelectNextItem();
                }
            }
            else
            {
                Move();
            }
        }
    }

    public void OnExit(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (inventoryIsOpen)
            {
                uiManager.InventoryUIComponent.Hide();
                inventoryIsOpen = false;
                droppingItem = false;
                usingItem = false;
            }
        }
    }

    private void Move()
    {
        Vector2 direction = controls.Player.Movement.ReadValue<Vector2>();
        Vector2 roundedDirection = new Vector2(Mathf.Round(direction.x), Mathf.Round(direction.y));
        Action.MoveOrHit(GetComponent<Actor>(), roundedDirection);
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -5);
    }

    public void OnGrab(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // Get the player's position
            Vector3 playerPosition = transform.position;

            // Convert the player's position to a Vector2Int by rounding its x and y components
            Vector2Int playerGridPosition = new Vector2Int(Mathf.RoundToInt(playerPosition.x), Mathf.RoundToInt(playerPosition.y));

            // Retrieve the item at the player's grid position
            Items.Consumable item = GameManager.Get.GetItemAtLocation(playerGridPosition);

            // Handle the three scenarios
            if (item == null)
            {
                Debug.Log("No item found at this location.");
            }
            else if (inventory.IsFull())
            {
                Debug.Log("Inventory is full. Cannot add item.");
            }
            else
            {
                // Attempt to add the item to the player's inventory
                if (inventory.AddItem(item))
                {
                    // If the item is successfully added to the inventory, remove it from the game world
                    GameManager.Get.RemoveItem(item);

                    // Hide the item by deactivating its GameObject
                    item.gameObject.SetActive(false);

                    Debug.Log("Item picked up and added to inventory.");
                }
            }
        }
    }

    public void OnDrop(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!inventoryIsOpen)
            {
                // Toon de inventory
                uiManager.InventoryUIComponent.Show(inventory.GetItems());
                inventoryIsOpen = true;
                droppingItem = true;
            }
            else if (droppingItem)
            {
                // Drop het geselecteerde item
                Items.Consumable selectedItem = inventory.GetItem(uiManager.InventoryUIComponent.Selected);
                if (selectedItem != null)
                {
                    selectedItem.transform.position = transform.position;
                    GameManager.Get.AddItem(selectedItem, new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y)));
                    selectedItem.gameObject.SetActive(true);
                    inventory.RemoveItem(selectedItem);
                }
                uiManager.InventoryUIComponent.Hide();
                inventoryIsOpen = false;
                droppingItem = false;
            }
        }
    }

    public void OnSelect(InputAction.CallbackContext context)
    {
        
    }

    public void OnUse(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!inventoryIsOpen)
            {
                // Toon de inventory
                uiManager.InventoryUIComponent.Show(inventory.GetItems());
                inventoryIsOpen = true;
                usingItem = true;
            }
            else if (usingItem)
            {
                // Gebruik het geselecteerde item
                Items.Consumable selectedItem = inventory.GetItem(uiManager.InventoryUIComponent.Selected);
                if (selectedItem != null)
                {
                    UseItem(selectedItem);
                    Destroy(selectedItem.gameObject);
                    inventory.RemoveItem(selectedItem);
                }
                uiManager.InventoryUIComponent.Hide();
                inventoryIsOpen = false;
                usingItem = false;
            }
        }
    }

    private void UseItem(Consumable selectedItem)
    {
        if (selectedItem is HealthPotion)
        {
            // Gebruik HealthPotion
            HealthPotion potion = (HealthPotion)selectedItem;
            playerActor.Heal(potion.HealAmount);
            UIManager.Get.AddMessage($"You have been healed by {potion.HealAmount} points!", Color.green);
        }
        else if (selectedItem is Fireball)
        {
            // Gebruik Fireball
            Fireball fireball = (Fireball)selectedItem;
            List<Actor> nearbyEnemies = GameManager.Get.GetNearbyEnemies(transform.position);
            foreach (Actor enemy in nearbyEnemies)
            {
                enemy.DoDamage(fireball.Damage);
            }
            UIManager.Get.AddMessage("You used a Fireball!", Color.red);
        }
        else if (selectedItem is ScrollOfConfusion)
        {
            // Gebruik ScrollOfConfusion
            ScrollOfConfusion scroll = (ScrollOfConfusion)selectedItem;
            List<Actor> nearbyEnemies = GameManager.Get.GetNearbyEnemies(transform.position);
            foreach (Actor enemy in nearbyEnemies)
            {
                Enemy enemyComponent = enemy.GetComponent<Enemy>();
                if (enemyComponent != null)
                {
                    enemyComponent.Confuse();
                }
            }
            UIManager.Get.AddMessage("You used a Scroll of Confusion!", Color.yellow);
        }
    }

}