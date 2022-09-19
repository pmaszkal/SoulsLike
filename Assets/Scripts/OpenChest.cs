using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PM
{
    public class OpenChest : Interactable
    {
        Animator animator;
        OpenChest openChest;

        public Transform playerStandingPosition;
        public GameObject itemSpawner;
        public WeaponItem itemInChest;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            openChest = GetComponent<OpenChest>();
        }

        public override void Interact(PlayerManager playerManager)
        {
            playerManager.OpenChestInteraction(playerStandingPosition);

            //rotate our player towards the chest
            Vector3 rotationDirection = transform.position - playerManager.transform.position;
            rotationDirection.y = 0;
            rotationDirection.Normalize();

            Quaternion tr = Quaternion.LookRotation(rotationDirection);
            Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 1000 * Time.deltaTime);
            playerManager.transform.rotation = targetRotation;

            animator.Play("Open Chest");
            StartCoroutine("SpawnItemInChest");

            WeaponPickUp weaponPickUp = itemSpawner.GetComponent<WeaponPickUp>();

            if (weaponPickUp != null)
            {
                weaponPickUp.weapon = itemInChest;
            }
            //lock his tranform to a certain point infront of the chest
            //open the chest lid and animate the player
            //spawn an item inside the chest the player can pick up
        }

        private IEnumerator SpawnItemInChest()
        {
            yield return new WaitForSeconds(1f);
            Instantiate(itemSpawner, transform);
            Destroy(openChest);
            tag = "Untagged";
        }
    }
}