using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PM
{
    public class InputHandler : MonoBehaviour
    {
        public float horizontal;
        public float vertical;
        public float moveAmount;
        public float mouseX;
        public float mouseY;

        public bool a_Input;
        public bool y_Input;
        public bool b_Input;
        public bool rb_Input;
        public bool rt_Input;
        public bool critical_Attack_Input;
        public bool jump_Input;
        public bool inventory_Input;
        public bool lock_On_Input;
        public bool right_Stick_Right_Input;
        public bool right_Stick_Left_Input;

        public bool d_Pad_Up;
        public bool d_Pad_Down;
        public bool d_Pad_Left;
        public bool d_Pad_Right;


        public bool lockOnFlag;
        public bool twoHandFlag;
        public bool rollFlag;
        public float rollInputTimer;
        public bool sprintFlag;
        public bool comboFlag;
        public bool inventoryFlag;
        public bool bPressed = false;

        public Transform criticalAttackRayCastStartPoint;

        PlayerControls inputActions;
        PlayerAttacker playerAttacker;
        PlayerInventory playerInventory;
        PlayerManager playerManager;
        PlayerStats playerStats;
        WeaponSlotManager weaponSlotManager;
        CameraHandler cameraHandler;
        PlayerAnimatorManager animatorHandler;
        UIManager uiManager;

        Vector2 movementInput;
        Vector2 cameraInput;

        private void Awake()
        {
            playerAttacker = GetComponentInChildren<PlayerAttacker>();
            playerInventory = GetComponent<PlayerInventory>();
            playerManager = GetComponent<PlayerManager>();
            playerStats = GetComponent<PlayerStats>();
            uiManager = FindObjectOfType<UIManager>();
            cameraHandler = FindObjectOfType<CameraHandler>();
            weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
            animatorHandler = GetComponentInChildren<PlayerAnimatorManager>();
        }

        private void OnEnable()
        {
            if (inputActions == null)
            {
                inputActions = new PlayerControls();
                inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
                inputActions.PlayerMovement.Camera.performed += _ => cameraInput = _.ReadValue<Vector2>();
                inputActions.PlayerMovement.LockOnTargetRight.performed += _ => right_Stick_Right_Input = true;
                inputActions.PlayerMovement.LockOnTargetLeft.performed += _ => right_Stick_Left_Input = true;
                inputActions.PlayerActions.Roll.performed += _ => b_Input = true;
                inputActions.PlayerActions.Roll.canceled += _ => b_Input = false;
                //inputActions.PlayerActions.Roll.started += _ => bPressed = true;
                //inputActions.PlayerActions.Roll.started += _ => b_Input = true;
                //inputActions.PlayerActions.Roll.canceled += _ => b_Input = false;
                inputActions.PlayerActions.Interact.performed += _ => a_Input = true;
                inputActions.PlayerActions.Y.performed += _ => y_Input = true;
                inputActions.PlayerActions.Jump.performed += _ => jump_Input = true;
                inputActions.PlayerActions.Inventory.performed += _ => inventory_Input = true;
                inputActions.PlayerActions.RB.performed += _ => rb_Input = true;
                inputActions.PlayerActions.RT.performed += _ => rt_Input = true;
                inputActions.PlayerActions.LockOn.performed += _ => lock_On_Input = true;
                inputActions.PlayerQuickSlots.DPadRight.performed += _ => d_Pad_Right = true;
                inputActions.PlayerQuickSlots.DPadLeft.performed += _ => d_Pad_Left = true;
                inputActions.PlayerActions.CriticalAttack.performed += _ => critical_Attack_Input = true;
            }

            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }

        public void TickInput(float delta)
        {
            HandleMoveInput(delta);
            HandleRollInput(delta);
            HandleAttackInput(delta);
            HandleQuickSlotInput();
            HandleInventoryInput();
            HandleLockOnInput();
            HandleTwoHandInput();
            HandleCriticalAttackInput();
        }

        private void HandleMoveInput(float delta)
        {
            horizontal = movementInput.x;
            vertical = movementInput.y;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
            mouseX = cameraInput.x;
            mouseY = cameraInput.y;
        }

        private void HandleRollInput(float delta)
        {
            //b_Input = inputActions.PlayerActions.Roll.phase == UnityEngine.InputSystem.InputActionPhase.Performed;
            if (b_Input)
            {
                rollInputTimer += delta;

                if (playerStats.currentStamina <= 0)
                {
                    b_Input = false;
                    sprintFlag = false;
                }

                if (moveAmount > 0.5f && playerStats.currentStamina > 0)
                {
                    sprintFlag = true;
                }
            }
            else
            {

                sprintFlag = false;

                if (rollInputTimer > 0 && rollInputTimer < 0.5f)
                {
                    rollFlag = true;
                }

                rollInputTimer = 0;
            }
        }

        private void HandleAttackInput(float delta)
        {

            //rb for right hand weapon
            //if (rb_Input)
            //{
            //    if (playerManager.canDoCombo)
            //    {
            //        comboFlag = true;
            //        playerAttacker.HandleWeaponCombo(playerInventory.rightWeapon);
            //        comboFlag = false;
            //    }
            //    else
            //    {
            //        if (playerManager.isInteracting)
            //            return;
            //        if (playerManager.canDoCombo)
            //            return;

            //        animatorHandler.anim.SetBool("isUsingRightHand", true);
            //        playerAttacker.HandleLightAttack(playerInventory.rightWeapon);
            //    }

            //}
            if (rb_Input)
            {
                playerAttacker.HandleRBAction();
            }


            if (rt_Input)
            {
                if (playerManager.isInteracting)
                    return;
                if (playerManager.canDoCombo)
                    return;

                animatorHandler.anim.SetBool("isUsingRightHand", true);
                playerAttacker.HandleHeavyAttack(playerInventory.rightWeapon);
            }

        }

        private void HandleQuickSlotInput()
        {
            if (d_Pad_Right)
            {
                playerInventory.ChangeRightWeapon();
            }
            if (d_Pad_Left)
            {
                playerInventory.ChangeLeftWeapon();
            }
        }

        //private void HandleJumpInput()
        //{
        //    inputActions.PlayerActions.Jump.performed += i => jump_Input = true;
        //}

        private void HandleInventoryInput()
        {
            if (inventory_Input)
            {
                inventoryFlag = !inventoryFlag;
                if (inventoryFlag)
                {
                    uiManager.openSelectWindow();
                    uiManager.UpdateUI();
                    uiManager.hudWindow.SetActive(false);
                }
                else
                {
                    uiManager.closeSelectWindow();
                    uiManager.hudWindow.SetActive(true);
                    uiManager.closeAllInventoryWindows();
                }
            }
        }

        private void HandleLockOnInput()
        {
            if (lock_On_Input && lockOnFlag == false)
            {
                lock_On_Input = false;
                cameraHandler.HandleLockOn();
                if (cameraHandler.nearestLockOnTarget != null)
                {
                    cameraHandler.currentLockOnTarget = cameraHandler.nearestLockOnTarget;
                    lockOnFlag = true;
                }
            }
            else if (lock_On_Input && lockOnFlag)
            {
                lock_On_Input = false;
                lockOnFlag = false;
                cameraHandler.ClearLockOnTargets();
            }

            if(lockOnFlag && right_Stick_Left_Input)
            {
                
                right_Stick_Left_Input = false;
                cameraHandler.HandleLockOn();
                if(cameraHandler.leftLockTarget != null)
                {
                    cameraHandler.currentLockOnTarget = cameraHandler.leftLockTarget;
                }
            }
            else if (lockOnFlag && right_Stick_Right_Input)
            {
                
                right_Stick_Right_Input = false;
                cameraHandler.HandleLockOn();
                if (cameraHandler.rightLockTarget != null)
                {
                    cameraHandler.currentLockOnTarget = cameraHandler.rightLockTarget;
                }
            }

            cameraHandler.SetCameraHeight();
        }

        private void HandleTwoHandInput()
        {
            if (y_Input)
            {
                y_Input = false;
                twoHandFlag = !twoHandFlag;

                if (twoHandFlag)
                {
                    //enable two handing
                    weaponSlotManager.LoadWeaponOnSlot(playerInventory.rightWeapon, false);
                }
                else
                {
                    //disable two handing
                    weaponSlotManager.LoadWeaponOnSlot(playerInventory.rightWeapon, false);
                    weaponSlotManager.LoadWeaponOnSlot(playerInventory.leftWeapon, true);
                }
            }
        }

        private void HandleCriticalAttackInput()
        {
            if (critical_Attack_Input)
            {
                critical_Attack_Input = false;
                playerAttacker.AttemptBackStabOrRiposte();
            }
        }
    }
}
