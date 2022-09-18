using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PM
{
    [CreateAssetMenu(menuName = "Spells/Healing Spell")]

    public class HealingSpell : SpellItem
    {
        public int healAmount;

        public override void AttemptToCastSpell(PlayerAnimatorManager animatorHandler, PlayerStats playerStats)
        {
            base.AttemptToCastSpell(animatorHandler, playerStats);
            GameObject instantiatedWarmUpSpellFX = Instantiate(spellWarmUpFX, animatorHandler.transform);
            instantiatedWarmUpSpellFX.transform.position = animatorHandler.transform.position;
            instantiatedWarmUpSpellFX.transform.rotation = animatorHandler.transform.rotation;
            animatorHandler.PlayTargetAnimation(spellAnimation, true);
            Destroy(instantiatedWarmUpSpellFX, 1f);
            Debug.Log("attempt to cast spell");
        }

        public override void SuccessfullyCastSpell(PlayerAnimatorManager animatorHandler, PlayerStats playerStats)
        {
            base.SuccessfullyCastSpell(animatorHandler, playerStats);
            GameObject instantiatedSpellFX = Instantiate(spellCastFX, animatorHandler.transform);
            instantiatedSpellFX.transform.position = animatorHandler.transform.position;
            instantiatedSpellFX.transform.rotation = animatorHandler.transform.rotation;
            playerStats.HealPlayer(healAmount);
            Destroy(instantiatedSpellFX, 1f);
            Debug.Log("spell cast successful");
        }
    }
}