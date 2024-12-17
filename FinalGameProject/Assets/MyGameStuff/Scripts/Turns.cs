using UnityEngine;
using System.Collections;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }

    public enum Turn
    {
        Player,
        Enemy
    }

    public Turn CurrentTurn { get; private set; } = Turn.Player;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void EndTurn()
    {
        if (CurrentTurn == Turn.Player)
        {
            CurrentTurn = Turn.Enemy;
            Debug.Log("Enemy's turn");
            StartCoroutine(EnemyMoveWithDelay(2.0f)); // Add a 2-second delay before the enemy moves
        }
        else
        {
            CurrentTurn = Turn.Player;
            Debug.Log("Player's turn");
        }
    }

    private IEnumerator EnemyMoveWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        EnemyMove();
    }

    private void EnemyMove()
    {
        // Find all enemy warriors
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("EnemyWarrior");

        // Move each enemy forward
        foreach (GameObject enemy in enemies)
        {
            Vector3 currentPosition = enemy.transform.position;
            Vector3 newPosition = currentPosition + enemy.transform.forward * 4; // Use local forward direction

            // Lock the Y position to ensure movement only on the X and Z axes
            newPosition.y = currentPosition.y;

            enemy.transform.position = newPosition;
            Debug.Log($"Enemy moved to new position: {newPosition}");
        }

        // End enemy turn and switch back to player turn
        EndTurn();
    }
}


