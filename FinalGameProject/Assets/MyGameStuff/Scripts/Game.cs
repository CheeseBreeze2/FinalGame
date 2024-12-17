using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WarriorMovement : MonoBehaviour
{
    public GameObject friendlyWarriorPrefab;
    public GameObject enemyWarriorPrefab;
    public List<GameObject> tiles;
    public Material highlightMaterial;

    private Material originalMaterial;
    private GameObject selectedWarrior;
    private int selectedWarriorIndex = -1;

    void Start()
    {
        if (friendlyWarriorPrefab == null || enemyWarriorPrefab == null || tiles == null || highlightMaterial == null)
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
            HandleKeyboardInput();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    void PlaceWarriors()
    {
        if (tiles == null || tiles.Count == 0)
        {
            Debug.LogError("No tiles assigned!");
            return;
        }

        for (int i = 0; i < tiles.Count; i++)
        {
            if (i == 0 || i == tiles.Count - 1)
            {
                Vector3 tilePosition = tiles[i].transform.position;
                float tileTopY = tilePosition.y + tiles[i].transform.localScale.y / 2;
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
                    newWarrior.transform.Rotate(0, 180, 0);
                }

                newWarrior.transform.SetParent(tiles[i].transform);
                Debug.Log($"Warrior placed on tile: {tiles[i].name} with tag: {newWarrior.tag}");
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
                DeselectWarrior();
                Debug.Log("Warrior deselected");
            }
        }

        if (selectedWarrior != null)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                MoveWarrior(Vector3.forward * 4);
                TurnManager.Instance.EndTurn();
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                MoveWarrior(Vector3.back * 4);
                TurnManager.Instance.EndTurn();
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                MoveWarrior(Vector3.left * 4);
                TurnManager.Instance.EndTurn();
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                MoveWarrior(Vector3.right * 4);
                TurnManager.Instance.EndTurn();
            }
        }
    }

    void SelectNextWarrior()
    {
        GameObject[] warriors = GameObject.FindGameObjectsWithTag("FriendlyWarrior");
        if (warriors.Length == 0) return;

        selectedWarriorIndex = (selectedWarriorIndex + 1) % warriors.Length;
        SelectWarrior(warriors[selectedWarriorIndex]);
        Debug.Log("Friendly warrior selected: " + selectedWarrior.name);
    }

    void SelectWarrior(GameObject warrior)
    {
        if (selectedWarrior != null)
        {
            DeselectWarrior();
        }

        selectedWarrior = warrior;
        Renderer renderer = selectedWarrior.GetComponent<Renderer>();
        if (renderer != null)
        {
            originalMaterial = renderer.material;
            renderer.material = highlightMaterial;
        }
    }

    void DeselectWarrior()
    {
        if (selectedWarrior != null)
        {
            Renderer renderer = selectedWarrior.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = originalMaterial;
            }
            selectedWarrior = null;
        }
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
        newPosition.y = currentPosition.y;

        selectedWarrior.transform.position = newPosition;
        Debug.Log($"Warrior moved to new position: {newPosition}");
    }
}
