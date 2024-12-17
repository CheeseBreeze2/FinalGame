using UnityEngine;

public class WarriorCombat : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FriendlyWarrior") && gameObject.CompareTag("EnemyWarrior"))
        {
            // Enemy warrior hits friendly warrior
            DestroyWarrior(other.gameObject); // Destroy the friendly warrior
            Debug.Log("Enemy warrior destroyed friendly warrior");
        }
        else if (other.CompareTag("EnemyWarrior") && gameObject.CompareTag("FriendlyWarrior"))
        {
            // Friendly warrior hits enemy warrior
            DestroyWarrior(other.gameObject); // Destroy the enemy warrior
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
}

