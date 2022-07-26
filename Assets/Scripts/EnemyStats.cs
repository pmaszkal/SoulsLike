using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PM
{
    public class EnemyStats : MonoBehaviour
    {
        public int healthLevel = 10;
        public int maxHealth;
        public int currentHealth;

        Animator animator;

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
        }

        private void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
        }

        private int SetMaxHealthFromHealthLevel()
        {
            return maxHealth = healthLevel * 10;
        }

        public void TakeDamage(int damage)
        {
            currentHealth = currentHealth - damage;

            animator.Play("Damage_01");

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                animator.Play("Death_01");
                //Handle player death;
            }
        }
    }
}