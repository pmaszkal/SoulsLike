using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PM
{
    public class PlayerStats : CharacterStats
    {
        PlayerManager playerManager;
        public HealthBar healthBar;
        public StaminaBar staminaBar;
        public FocusPointBar focusPointBar;
        PlayerAnimatorManager animatorHandler;

        public float staminaRegenerationAmount = 30f;
        public float staminaRegenerationTimer = 0;

        private void Awake()
        {
            animatorHandler = GetComponentInChildren<PlayerAnimatorManager>();
            playerManager = GetComponent<PlayerManager>();

            healthBar = FindObjectOfType<HealthBar>();
            staminaBar = FindObjectOfType<StaminaBar>();
            focusPointBar = FindObjectOfType<FocusPointBar>();
        }

        private void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);
            healthBar.SetCurrentHealth(currentHealth);

            maxStamina = SetMaxStaminaFromStaminaLevel();
            currentStamina = maxStamina;
            staminaBar.SetMaxStamina(maxStamina);
            staminaBar.SetCurrentStamina(currentStamina);

            maxFocusPoints = SetMaxFocusPointsFromFocusLevel();
            currentFocusPoints = maxFocusPoints;
            focusPointBar.SetMaxFocusPoints(maxFocusPoints);
            focusPointBar.SetCurrentFocusPoints(currentFocusPoints);
        }

        private int SetMaxHealthFromHealthLevel()
        {
            return maxHealth = healthLevel * 10;
        }

        private float SetMaxStaminaFromStaminaLevel()
        {
            return maxStamina = staminaLevel * 10f;
        }

        private float SetMaxFocusPointsFromFocusLevel()
        {
            return maxFocusPoints = focusLevel * 10;
        }

        public void TakeDamage(int damage)
        {
            if (playerManager.isInvulnerable)
                return;
            if (isDead)
                return;

            currentHealth = currentHealth - damage;
            healthBar.SetCurrentHealth(currentHealth);

            animatorHandler.PlayTargetAnimation("Damage_01", true);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                animatorHandler.PlayTargetAnimation("Death_01", true);
                isDead = true;
                //Handle player death;
            }
        }

        public void TakeDamageNoAnimation(int damage)
        {
            currentHealth = currentHealth - damage;
            healthBar.SetCurrentHealth(currentHealth);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                isDead = true;
            }
        }

        public void TakeStaminaDamage(int damage)
        {
            currentStamina = currentStamina - damage;
            staminaBar.SetCurrentStamina(currentStamina);
        }

        public void RegenerateStamina()
        {
            if (playerManager.isInteracting)
            {
                staminaRegenerationTimer = 0;
            }
            else
            {
                staminaRegenerationTimer += Time.deltaTime;
                if (currentStamina < maxStamina && staminaRegenerationTimer > 1f)
                {
                    currentStamina += staminaRegenerationAmount * Time.deltaTime;
                    staminaBar.SetCurrentStamina(Mathf.RoundToInt(currentStamina));
                }
            }
        }

        public void HealPlayer(int amount)
        {
            currentHealth = currentHealth + amount;

            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }

            healthBar.SetCurrentHealth(currentHealth);
        }

        public void DeductFocusPoints(int focusPoints)
        {
            currentFocusPoints = currentFocusPoints - focusPoints;

            if (currentFocusPoints < 0)
            {
                currentFocusPoints = 0;
            }

            focusPointBar.SetCurrentFocusPoints(currentFocusPoints);
        }

        public void AddSouls(int souls)
        {
            soulCount += souls;
        }
    }
}