using UnityEngine;
using UnityEngine.SceneManagement;

public class WarriorCombat : MonoBehaviour
{
    private static bool collisionProcessed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (collisionProcessed) return;

        if (other.CompareTag("FriendlyWarrior") && gameObject.CompareTag("EnemyWarrior"))
        {
            // Enemy warrior hits friendly warrior
            DestroyWarrior(other.gameObject); // Destroy the friendly warrior
            collisionProcessed = true;
            Debug.Log("Enemy warrior destroyed friendly warrior");

            // Check if all friendly warriors are defeated after a short delay to ensure the warrior is destroyed
            Invoke("CheckForRemainingFriendlyWarriors", 0.1f);
        }
        else if (other.CompareTag("EnemyWarrior") && gameObject.CompareTag("FriendlyWarrior"))
        {
            // Friendly warrior hits enemy warrior
            DestroyWarrior(other.gameObject); // Destroy the enemy warrior
            collisionProcessed = true;
            Debug.Log("Friendly warrior destroyed enemy warrior");

            // Check if all enemies are defeated after a short delay to ensure the enemy is destroyed
            Invoke("CheckForRemainingEnemies", 0.1f);
        }
    }

    private void DestroyWarrior(GameObject warrior)
    {
        // Disable the collider to prevent further collisions
        Collider collider = warrior.GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = false;
        }

        // Destroy the warrior
        Destroy(warrior);
    }

    private void CheckForRemainingEnemies()
    {
        // Check if all enemies are defeated
        int remainingEnemies = GameObject.FindGameObjectsWithTag("EnemyWarrior").Length;
        Debug.Log("Remaining enemies: " + remainingEnemies);
        if (remainingEnemies == 0)
        {
            Debug.Log("All enemies defeated. Loading YouWin scene.");
            SceneManager.LoadScene("YouWin");
        }

        // Reset the flag after checking for remaining enemies
        collisionProcessed = false;
    }

    private void CheckForRemainingFriendlyWarriors()
    {
        // Check if all friendly warriors are defeated
        int remainingFriendlyWarriors = GameObject.FindGameObjectsWithTag("FriendlyWarrior").Length;
        Debug.Log("Remaining friendly warriors: " + remainingFriendlyWarriors);
        if (remainingFriendlyWarriors == 0)
        {
            Debug.Log("All friendly warriors defeated. Loading YouLose scene.");
            SceneManager.LoadScene("YouLose");
        }

        // Reset the flag after checking for remaining friendly warriors
        collisionProcessed = false;
    }

    private void OnDestroy()
    {
        // Reset the flag when the object is destroyed
        collisionProcessed = false;
    }
}

