using System.Collections.Generic;
using UnityEngine;

public class WarriorPlacement : MonoBehaviour
{
    public GameObject warriorPrefab;  // Assign your warrior prefab here
    public List<GameObject> tiles;    // Assign the tiles (Grass) in the Inspector

    void Start()
    {
        PlaceWarriors();
    }

    void PlaceWarriors()
    {
        // Ensure the tiles list is populated
        if (tiles == null || tiles.Count == 0)
        {
            Debug.LogError("No tiles assigned!");
            return;
        }

        // Ensure the grid has exactly 3 tiles
        if (tiles.Count != 3)
        {
            Debug.LogError("The grid should have exactly 3 tiles.");
            return;
        }

        // Place warriors only on the first and last tile
        for (int i = 0; i < tiles.Count; i++)
        {
            GameObject tile = tiles[i];

            // Only instantiate warriors on the first and last tiles
            if (i == 0 || i == tiles.Count - 1)
            {
                // Ensure the tile is unoccupied by checking its child count
                if (tile.transform.childCount == 0)
                {
                    // Calculate the correct spawn position for the warrior on top of the tile
                    // If tile's Y scale is 2, position the warrior at Y = 0.5 (half the tile's height)
                    float tileHeightOffset = tile.transform.localScale.y / 2 - 0.5f; // Adjust for warrior height
                    Vector3 spawnPosition = tile.transform.position + new Vector3(0, tileHeightOffset, 0);

                    // Instantiate the warrior at the correct position
                    GameObject newWarrior = Instantiate(warriorPrefab, spawnPosition, Quaternion.identity);

                    // Parent the warrior to the tile for better organization
                    newWarrior.transform.SetParent(tile.transform);

                    // Make sure the warrior's position is correctly centered on the tile
                    newWarrior.transform.localPosition = new Vector3(0, tileHeightOffset, 0);
                }
                else
                {
                    Debug.Log("Tile already occupied: " + tile.name);
                }
            }
        }
    }
}
