using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    private int width, height;
    private int maxRoomSize, minRoomSize;
    private int maxRooms;
    private int maxEnemies;
    private int maxItems;
    private int currentFloor;

    List<Room> rooms = new List<Room>();
    private string[] enemyNamesInOrderOfStrength = { "Scorpion", "Taurus", "Wasp", "Spider" ,"Viking", "Wolf", "Wizard", "Lizard", "Boss" /* Add more enemies as needed */ };
    public void SetSize(int width, int height)
    {
        this.width = width;
        this.height = height;
    }

    public void SetRoomSize(int min, int max)
    {
        minRoomSize = min;
        maxRoomSize = max;
    }

    public void SetMaxRooms(int max)
    {
        maxRooms = max;
    }

    public void SetMaxEnemies(int max)
    {
        maxEnemies = max;
    }
    public void SetMaxItems(int max)
    {
        maxItems = max;
    }
    public void SetCurrentFloor(int floor)
    {
        currentFloor = floor;
    }


    public void Generate()
    {
        rooms.Clear();
        GameManager.Get.ClearLadders();

        for (int roomNum = 0; roomNum < maxRooms; roomNum++)
        {
            int roomWidth = Random.Range(minRoomSize, maxRoomSize);
            int roomHeight = Random.Range(minRoomSize, maxRoomSize);

            int roomX = Random.Range(0, width - roomWidth - 1);
            int roomY = Random.Range(0, height - roomHeight - 1);

            var room = new Room(roomX, roomY, roomWidth, roomHeight);

            // if the room overlaps with another room, discard it
            if (room.Overlaps(rooms))
            {
                continue;
            }

            // add tiles make the room visible on the tilemap
            for (int x = roomX; x < roomX + roomWidth; x++)
            {
                for (int y = roomY; y < roomY + roomHeight; y++)
                {
                    if (x == roomX
                        || x == roomX + roomWidth - 1
                        || y == roomY
                        || y == roomY + roomHeight - 1)
                    {
                        if (!TrySetWallTile(new Vector3Int(x, y)))
                        {
                            continue;
                        }
                    }
                    else
                    {
                        SetFloorTile(new Vector3Int(x, y, 0));
                    }

                }
            }

            // create a coridor between rooms
            if (rooms.Count != 0)
            {
                TunnelBetween(rooms[rooms.Count - 1], room);
            }
            PlaceEnemies(room, maxEnemies);
            PlaceItems(room, maxItems);

            rooms.Add(room);
        }
        // Plaats de ladder naar beneden in het midden van de laatste kamer
        var lastRoom = rooms[rooms.Count - 1];
        GameManager.Get.CreateGameObject("LadderDown", lastRoom.Center());

        GameObject player = GameObject.Find("Player");

        if (player != null)
        {
            // Verplaats de speler naar het midden van de eerste kamer
            Vector2Int playerPosition = rooms[0].Center();
            player.transform.position = new Vector3(playerPosition.x + 0.5f, playerPosition.y + 0.5f, 0);
        }
        else
        {
            // Creëer de speler in het midden van de eerste kamer
            Vector2Int playerPosition = rooms[0].Center();
            player = GameManager.Get.CreateGameObject("Player", new Vector3(playerPosition.x  , playerPosition.y  , 0));
        }
    }

    private bool TrySetWallTile(Vector3Int pos)
    {
        // if this is a floor, it should not be a wall
        if (MapManager.Get.FloorMap.GetTile(pos))
        {
            return false;
        }
        else
        {
            // if not, it can be a wall
            MapManager.Get.ObstacleMap.SetTile(pos, MapManager.Get.WallTile);
            return true;
        }
    }

    private void SetFloorTile(Vector3Int pos)
    {
        // this tile should be walkable, so remove every obstacle
        if (MapManager.Get.ObstacleMap.GetTile(pos))
        {
            MapManager.Get.ObstacleMap.SetTile(pos, null);
        }
        // set the floor tile
        MapManager.Get.FloorMap.SetTile(pos, MapManager.Get.FloorTile);
    }

    private void TunnelBetween(Room oldRoom, Room newRoom)
    {
        Vector2Int oldRoomCenter = oldRoom.Center();
        Vector2Int newRoomCenter = newRoom.Center();
        Vector2Int tunnelCorner;

        if (Random.value < 0.5f)
        {
            // move horizontally, then vertically
            tunnelCorner = new Vector2Int(newRoomCenter.x, oldRoomCenter.y);
        }
        else
        {
            // move vertically, then horizontally
            tunnelCorner = new Vector2Int(oldRoomCenter.x, newRoomCenter.y);
        }

        // Generate the coordinates for this tunnel
        List<Vector2Int> tunnelCoords = new List<Vector2Int>();
        BresenhamLine.Compute(oldRoomCenter, tunnelCorner, tunnelCoords);
        BresenhamLine.Compute(tunnelCorner, newRoomCenter, tunnelCoords);

        // Set the tiles for this tunnel
        for (int i = 0; i < tunnelCoords.Count; i++)
        {
            SetFloorTile(new Vector3Int(tunnelCoords[i].x, tunnelCoords[i].y));

            for (int x = tunnelCoords[i].x - 1; x <= tunnelCoords[i].x + 1; x++)
            {
                for (int y = tunnelCoords[i].y - 1; y <= tunnelCoords[i].y + 1; y++)
                {
                    if (!TrySetWallTile(new Vector3Int(x, y, 0)))
                    {
                        continue;
                    }
                }
            }
        }
    }

    private void PlaceEnemies(Room room, int maxEnemies)
    {
        // the number of enemies we want
        int maxIndex = Mathf.Min(currentFloor, enemyNamesInOrderOfStrength.Length - 1);

        Debug.Log("Current Floor: " + currentFloor);
        Debug.Log("Max Index: " + maxIndex);

        for (int counter = 0; counter < maxEnemies; counter++)
        {
            // The borders of the room are walls, so add and substract by 1
            int x = Random.Range(room.X + 1, room.X + room.Width - 1);
            int y = Random.Range(room.Y + 1, room.Y + room.Height - 1);

            // Pick an enemy name based on strength order
            string enemyName = GetEnemyNameBasedOnStrength(maxIndex);

            // Create the enemy GameObject
            GameManager.Get.CreateGameObject(enemyName, new Vector2(x, y));
        }
    }
    private void PlaceItems(Room room, int maxItems)
    {
        // the number of items we want
        int numItems = UnityEngine.Random.Range(0, maxItems + 1);

        for (int counter = 0; counter < numItems; counter++)
        {
            // The borders of the room are walls, so add and subtract by 1
            int x = UnityEngine.Random.Range(room.X + 1, room.X + room.Width - 1);
            int y = UnityEngine.Random.Range(room.Y + 1, room.Y + room.Height - 1);

            // Create and add consumable items to the room
            Vector2 position = new Vector2(x, y);
            GameObject itemObject = GameManager.Get.CreateGameObject("HealthPotion", position);
            Items.Consumable item = itemObject.GetComponent<Items.Consumable>();

            Vector2Int gridPosition = new Vector2Int(x, y);
            GameManager.Get.AddItem(item, gridPosition); // Correctly pass the item and its position
        }
    }
    // Method to get the enemy name based on strength order
    private string GetEnemyNameBasedOnStrength(int maxIndex)
    {
        // You can implement your logic here to choose the enemy name
        // For now, let's pick a random name from the list
        return enemyNamesInOrderOfStrength[Random.Range(0, maxIndex + 1)];
    }
}