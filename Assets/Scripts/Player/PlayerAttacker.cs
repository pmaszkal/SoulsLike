using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PM
{
    public class PlayerAttacker : MonoBehaviour
    {
        AnimatorHandler animatorHandler;
        InputHandler inputHandler;
        public string lastAttack;

        WeaponSlotManager weaponSlotManager;

        private void Awake()
        {
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
            inputHandler = GetComponent<InputHandler>();
            weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
        }

        public void HandleWeaponCombo(WeaponItem weapon)
        {
            if (inputHandler.comboFlag)
            {
                animatorHandler.anim.SetBool("canDoCombo", false);
                if (lastAttack == weapon.OH_Light_Attack_01)
                {
                    animatorHandler.PlayTargetAnimation(weapon.OH_Light_Attack_02, true);
                }
                else if (lastAttack == weapon.TH_Light_Attack_01)
                {
                    animatorHandler.PlayTargetAnimation(weapon.TH_Light_Attack_02, true);
                }
            }
        }

        public void HandleLightAttack(WeaponItem weapon)
        {
            if (weapon.OH_Light_Attack_01.Length > 0)
            {
                weaponSlotManager.attackingWeapon = weapon;
                if (inputHandler.twoHandFlag)
                {
                    animatorHandler.PlayTargetAnimation(weapon.TH_Light_Attack_01, true);
                    lastAttack = weapon.TH_Light_Attack_01;
                }
                else
                {
                    animatorHandler.PlayTargetAnimation(weapon.OH_Light_Attack_01, true);
                    lastAttack = weapon.OH_Light_Attack_01;
                }
            }
        }

        public void HandleHeavyAttack(WeaponItem weapon)
        {
            //tak samo jak dla light attack TODO
            weaponSlotManager.attackingWeapon = weapon;
            animatorHandler.PlayTargetAnimation(weapon.OH_Heavy_Attack_01, true);
            lastAttack = weapon.OH_Heavy_Attack_01;
        }
    }
}