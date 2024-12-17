using UnityEngine;

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
        }
        else if (other.CompareTag("EnemyWarrior") && gameObject.CompareTag("FriendlyWarrior"))
        {
            // Friendly warrior hits enemy warrior
            DestroyWarrior(other.gameObject); // Destroy the enemy warrior
            collisionProcessed = true;
            Debug.Log("Friendly warrior destroyed enemy warrior");
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

    private void OnDestroy()
    {
        // Reset the flag when the object is destroyed
        collisionProcessed = false;
    }
}

