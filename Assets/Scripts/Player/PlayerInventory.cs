using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PM
{
    public class PlayerInventory : MonoBehaviour
    {
        WeaponSlotManager WeaponSlotManager;

        public WeaponItem rightWeapon;
        public WeaponItem leftWeapon;

        public WeaponItem unarmedWeapon;

        public WeaponItem[] weaponsInRightHandSlots = new WeaponItem[2];
        public WeaponItem[] weaponsInLeftHandSlots = new WeaponItem[2];

        public int currentRightWeaponIndex = -1;
        public int currentLeftWeaponIndex = -1;

        public List<WeaponItem> weaponsInventory;

        public void Awake()
        {
            WeaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
        }

        private void Start()
        {
            rightWeapon = unarmedWeapon;
            leftWeapon = unarmedWeapon;
            currentRightWeaponIndex = -1;
            currentLeftWeaponIndex = -1;
            //rightWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
            //leftWeapon = weaponsInLeftHandSlots[currentLeftWeaponIndex];
            //WeaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
            //WeaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);
        }

        public void ChangeRightWeapon()
        {
            //currentRightWeaponIndex = currentRightWeaponIndex + 1;

            //if (currentRightWeaponIndex == 0 && weaponsInRightHandSlots[0] != null)
            //{
            //    rightWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
            //    WeaponSlotManager.LoadWeaponOnSlot(weaponsInRightHandSlots[currentRightWeaponIndex], false);
            //}
            //else if (currentRightWeaponIndex == 0 && weaponsInRightHandSlots[0] == null)
            //{
            //    currentRightWeaponIndex = currentRightWeaponIndex + 1;
            //}

            //else if (currentRightWeaponIndex == 1 && weaponsInRightHandSlots[1] != null)
            //{
            //    rightWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
            //    WeaponSlotManager.LoadWeaponOnSlot(weaponsInRightHandSlots[currentRightWeaponIndex], false);
            //}
            //else if (currentRightWeaponIndex == 1 && weaponsInRightHandSlots[1] == null)
            //{
            //    currentRightWeaponIndex = currentRightWeaponIndex + 1;
            //}

            //if (currentRightWeaponIndex > weaponsInRightHandSlots.Length)
            //{
            //    currentRightWeaponIndex = -1;
            //    rightWeapon = unarmedWeapon;
            //    WeaponSlotManager.LoadWeaponOnSlot(unarmedWeapon, false);
            //}

            currentRightWeaponIndex = currentRightWeaponIndex + 1;

            if (currentRightWeaponIndex > weaponsInRightHandSlots.Length - 1)
            {
                currentRightWeaponIndex = -1;
                rightWeapon = unarmedWeapon;
                WeaponSlotManager.LoadWeaponOnSlot(unarmedWeapon, false);

            }
            else if (weaponsInRightHandSlots[currentRightWeaponIndex] != null)
            {
                rightWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
                WeaponSlotManager.LoadWeaponOnSlot(weaponsInRightHandSlots[currentRightWeaponIndex], false);
            }
            else
            {
                currentRightWeaponIndex = currentRightWeaponIndex + 1;
                ChangeRightWeapon();
            }
        }

        public void ChangeLeftWeapon()
        {
            //currentLeftWeaponIndex = currentLeftWeaponIndex + 1;

            //if (currentLeftWeaponIndex == 0 && weaponsInLeftHandSlots[0] != null)
            //{
            //    leftWeapon = weaponsInLeftHandSlots[currentLeftWeaponIndex];
            //    WeaponSlotManager.LoadWeaponOnSlot(weaponsInLeftHandSlots[currentLeftWeaponIndex], true);
            //}
            //else if (currentLeftWeaponIndex == 0 && weaponsInLeftHandSlots[0] == null)
            //{
            //    currentLeftWeaponIndex = currentLeftWeaponIndex + 1;
            //}

            //else if (currentLeftWeaponIndex == 1 && weaponsInLeftHandSlots[1] != null)
            //{
            //    leftWeapon = weaponsInLeftHandSlots[currentLeftWeaponIndex];
            //    WeaponSlotManager.LoadWeaponOnSlot(weaponsInLeftHandSlots[currentLeftWeaponIndex], true);
            //}
            //else if (currentLeftWeaponIndex == 1 && weaponsInLeftHandSlots[1] == null)
            //{
            //    currentLeftWeaponIndex = currentLeftWeaponIndex + 1;
            //}

            //if (currentLeftWeaponIndex > weaponsInLeftHandSlots.Length)
            //{
            //    currentLeftWeaponIndex = -1;
            //    leftWeapon = unarmedWeapon;
            //    WeaponSlotManager.LoadWeaponOnSlot(unarmedWeapon, true);
            //}


            currentLeftWeaponIndex = currentLeftWeaponIndex + 1;

            if (currentLeftWeaponIndex > weaponsInLeftHandSlots.Length - 1)
            {
                currentLeftWeaponIndex = -1;
                leftWeapon = unarmedWeapon;
                WeaponSlotManager.LoadWeaponOnSlot(unarmedWeapon, true);

            }
            else if (weaponsInLeftHandSlots[currentLeftWeaponIndex] != null)
            {
                leftWeapon = weaponsInLeftHandSlots[currentLeftWeaponIndex];
                WeaponSlotManager.LoadWeaponOnSlot(weaponsInLeftHandSlots[currentLeftWeaponIndex], true);
            }
            else
            {
                currentLeftWeaponIndex = currentLeftWeaponIndex + 1;
                ChangeLeftWeapon();
            }
        }
    }
}