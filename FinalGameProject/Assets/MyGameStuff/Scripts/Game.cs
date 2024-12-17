using System.Collections.Generic;
using UnityEngine;

public class WarriorMovement : MonoBehaviour
{
    public GameObject friendlyWarriorPrefab;  // Assignable in the Inspector
    public GameObject enemyWarriorPrefab;     // Assignable in the Inspector
    public List<GameObject> tiles;            // Assign all tiles in the scene to this list in the Inspector
    private GameObject selectedWarrior;       // To store the currently selected warrior
    private int selectedWarriorIndex = -1;    // Index of the currently selected warrior

    void Start()
    {
        if (friendlyWarriorPrefab == null || enemyWarriorPrefab == null || tiles == null)
        {
            Debug.LogError("One or more references are not assigned in the Inspector.");
            return;
        }

        PlaceWarriors();
    }

    void Update()
    {
        if (TurnManager.Instance == null)
        {
            Debug.LogError("TurnManager instance is not set.");
            return;
        }

        if (TurnManager.Instance.CurrentTurn == TurnManager.Turn.Player)
        {
            HandleMouseInput();
            HandleKeyboardInput();
        }
    }

    void PlaceWarriors()
    {
        if (tiles == null || tiles.Count == 0)
        {
            Debug.LogError("No tiles assigned!");
            return;
        }

        // Place warriors only on the first and last tiles
        for (int i = 0; i < tiles.Count; i++)
        {
            if (i == 0 || i == tiles.Count - 1) // First and last tiles
            {
                Vector3 tilePosition = tiles[i].transform.position;

                // Tile top is at Y = tilePosition.y + half of tile height
                float tileTopY = tilePosition.y + tiles[i].transform.localScale.y / 2;

                // Warrior's bottom Y should be at tileTopY
                float warriorHeight = friendlyWarriorPrefab.transform.localScale.y;
                float spawnY = tileTopY + warriorHeight / 2;

                Vector3 spawnPosition = new Vector3(tilePosition.x, spawnY, tilePosition.z);

                GameObject newWarrior;
                if (i == 0)
                {
                    newWarrior = Instantiate(friendlyWarriorPrefab, spawnPosition, Quaternion.identity);
                }
                else
                {
                    newWarrior = Instantiate(enemyWarriorPrefab, spawnPosition, Quaternion.identity);
                    newWarrior.transform.Rotate(0, 180, 0); // Rotate the enemy warrior to face the middle of the grid
                }

                newWarrior.transform.SetParent(tiles[i].transform);

                Debug.Log($"Warrior placed on tile: {tiles[i].name} with tag: {newWarrior.tag}");
            }
        }
    }

    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0)) // Detect left mouse button click
        {
            Debug.Log("Mouse button clicked");

            // Perform a raycast to detect what was clicked
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Define layer mask for "Warrior" and "Tile" layers
            int layerMask = LayerMask.GetMask("Warrior");

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                Debug.Log($"Raycast hit: {hit.collider.name}");

                // Check if a friendly warrior was clicked
                if (hit.collider.CompareTag("FriendlyWarrior"))
                {
                    selectedWarrior = hit.collider.gameObject; // Store the clicked warrior
                    Debug.Log("Friendly warrior selected: " + selectedWarrior.name);
                }
                else
                {
                    Debug.Log("Raycast hit an unrecognized object or no friendly warrior selected");
                }
            }
            else
            {
                Debug.Log("Raycast did not hit any object");
            }
        }
    }

    void HandleKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (selectedWarrior == null)
            {
                SelectNextWarrior();
            }
            else
            {
                selectedWarrior = null;
                Debug.Log("Warrior deselected");
            }
        }

        if (selectedWarrior != null)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                MoveWarrior(Vector3.forward * 4); // Move along the Z-axis
                TurnManager.Instance.EndTurn();
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                MoveWarrior(Vector3.back * 4); // Move along the Z-axis
                TurnManager.Instance.EndTurn();
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                MoveWarrior(Vector3.left * 4); // Move along the X-axis
                TurnManager.Instance.EndTurn();
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                MoveWarrior(Vector3.right * 4); // Move along the X-axis
                TurnManager.Instance.EndTurn();
            }
        }
    }

    void SelectNextWarrior()
    {
        GameObject[] warriors = GameObject.FindGameObjectsWithTag("FriendlyWarrior");
        if (warriors.Length == 0) return;

        selectedWarriorIndex = (selectedWarriorIndex + 1) % warriors.Length;
        selectedWarrior = warriors[selectedWarriorIndex];
        Debug.Log("Friendly warrior selected: " + selectedWarrior.name);
    }

    void MoveWarrior(Vector3 direction)
    {
        if (selectedWarrior == null)
        {
            Debug.LogError("No warrior selected to move.");
            return;
        }

        Vector3 currentPosition = selectedWarrior.transform.position;
        Vector3 newPosition = currentPosition + direction;

        // Lock the Y position to ensure movement only on the X and Z axes
        newPosition.y = currentPosition.y;

        Debug.Log($"Current position: {currentPosition}, New position: {newPosition}");

        // Move the warrior to the new position
        selectedWarrior.transform.position = newPosition;

        Debug.Log($"Warrior moved to new position: {newPosition}");
    }
}



