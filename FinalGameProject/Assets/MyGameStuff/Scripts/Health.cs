//using UnityEngine;

//public class WarriorHealth : MonoBehaviour
//{
//    public int maxHP = 2;
//    private int currentHP;

//    void Start()
//    {
//        currentHP = maxHP;
//    }

//    public void TakeDamage(int damage)
//    {
//        currentHP -= damage;
//        if (currentHP <= 0)
//        {
//            Die();
//        }
//    }

//    private void Die()
//    {
//        // Notify the WarriorCombat script to handle the death
//        WarriorCombat combat = GetComponent<WarriorCombat>();
//        if (combat != null)
//        {
//            combat.HandleDeath();
//        }
//    }
//}

