using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PM
{
    public class CameraHandler : MonoBehaviour
    {
        InputHandler inputHandler;
        PlayerManager playerManager;

        public Transform targetTransform;
        public Transform cameraTransform;
        public Transform cameraPivotTransfrom;
        private Transform myTransform;
        private Vector3 cameraTransformPosition;
        public LayerMask ignoreLayers;
        public LayerMask enviromentLayer;
        private Vector3 cameraFollowVelocity = Vector3.zero;

        public static CameraHandler singleton;

        public float lookSpeed = 0.1f;
        public float followSpeed = 0.1f;
        public float pivotSpeed = 0.03f;

        private float targetPosisiton;
        private float defaultPosition;
        private float lookAngle;
        private float pivotAngle;
        public float minimumPivot = -35;
        public float maximumPivot = 35;

        public float cameraSphereRadius = 0.2f;
        public float cameraCollisionOffset = 0.2f;
        public float minimumCollisionOffset = 0.2f;
        public float lockedPivotPosistion = 2.25f;
        public float unlockedPivotPosition = 1.65f;

        public Transform currentLockOnTarget;

        List<CharacterManager> availableTargets = new List<CharacterManager>();
        public Transform nearestLockOnTarget;
        public Transform leftLockTarget;
        public Transform rightLockTarget;
        public float maximumLockOnDistance = 30f;

        private void Awake()
        {
            singleton = this;
            myTransform = transform;
            defaultPosition = cameraTransform.localPosition.z;
            ignoreLayers = ~(1 << 7 | 1 << 9 | 1 << 10 | 1 << 13);
            targetTransform = FindObjectOfType<PlayerManager>().transform;
            inputHandler = FindObjectOfType<InputHandler>();
            playerManager = FindObjectOfType<PlayerManager>();
        }

        private void Start()
        {
            enviromentLayer = LayerMask.NameToLayer("Enviroment");
        }

        public void FollowTarget(float delta)
        {
            Vector3 targetPosition = Vector3.SmoothDamp
                (myTransform.position, targetTransform.position, ref cameraFollowVelocity, delta / followSpeed);
            myTransform.position = targetPosition;

            HandleCameraCollisions(delta);
        }

        public void HandlerCameraRotation(float delta, float mouseXInput, float mouseYInput)
        {
            if (inputHandler.lockOnFlag == false && currentLockOnTarget == null)
            {
                lookAngle += (mouseXInput * lookSpeed) / delta;
                pivotAngle -= (mouseYInput * pivotSpeed) / delta;
                pivotAngle = Mathf.Clamp(pivotAngle, minimumPivot, maximumPivot);

                Vector3 rotation = Vector3.zero;
                rotation.y = lookAngle;
                Quaternion targetRotation = Quaternion.Euler(rotation);
                myTransform.rotation = targetRotation;

                rotation = Vector3.zero;
                rotation.x = pivotAngle;

                targetRotation = Quaternion.Euler(rotation);
                cameraPivotTransfrom.localRotation = targetRotation;
            }
            else
            {
                //float velocity = 0;
                Vector3 dir = currentLockOnTarget.position - transform.position;
                dir.Normalize();
                dir.y = 0;

                Quaternion targetRotation = Quaternion.LookRotation(dir);
                transform.rotation = targetRotation;

                dir = currentLockOnTarget.position - cameraPivotTransfrom.position;
                dir.Normalize();

                targetRotation = Quaternion.LookRotation(dir);
                Vector3 eulerAngle = targetRotation.eulerAngles;
                eulerAngle.y = 0;
                cameraPivotTransfrom.localEulerAngles = eulerAngle;
            }
        }

        private void HandleCameraCollisions(float delta)
        {
            targetPosisiton = defaultPosition;
            RaycastHit hit;
            Vector3 direction = cameraTransform.position - cameraPivotTransfrom.position;
            direction.Normalize();

            if (Physics.SphereCast(cameraPivotTransfrom.position, cameraSphereRadius, direction, out hit
                , Mathf.Abs(targetPosisiton), ignoreLayers))
            {
                float dis = Vector3.Distance(cameraPivotTransfrom.position, hit.point);
                targetPosisiton = -(dis - cameraCollisionOffset);
            }

            if (Mathf.Abs(targetPosisiton) < minimumCollisionOffset)
            {
                targetPosisiton = -minimumCollisionOffset;
            }

            cameraTransformPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosisiton, delta / 0.2f);
            cameraTransform.localPosition = cameraTransformPosition;
        }

        public void HandleLockOn()
        {
            availableTargets.Clear();

            float shortestDistance = Mathf.Infinity;
            float shortestDistanceOfLeftTarget = Mathf.Infinity;
            float shortestDistanceOfRightTarget = Mathf.Infinity;

            Collider[] colliders = Physics.OverlapSphere(targetTransform.position, 26);

            if (colliders.Length > 0)
            {
                for (int i = 0; i < colliders.Length; i++)
                {
                    CharacterManager character = colliders[i].GetComponent<CharacterManager>();

                    if (character != null)
                    {
                        Vector3 lockTargetDirection = character.transform.position - targetTransform.position;
                        float distanceFromTarget = Vector3.Distance(targetTransform.position, character.transform.position);
                        float viewableAngle = Vector3.Angle(lockTargetDirection, cameraTransform.forward);
                        RaycastHit hit;

                        if (character.transform.root != targetTransform.transform.root
                            && viewableAngle > -50 && viewableAngle < 50
                            && distanceFromTarget <= maximumLockOnDistance)
                        {
                            if (Physics.Linecast(playerManager.lockOnTransform.position, character.lockOnTransform.position, out hit))
                            {
                                Debug.DrawLine(playerManager.lockOnTransform.position, character.lockOnTransform.position);

                                if (hit.transform.gameObject.layer == enviromentLayer)
                                {
                                    //cannot lock on
                                }
                                else
                                {
                                    availableTargets.Add(character);
                                }
                            }
                        }
                    }
                }

                for (int k = 0; k < availableTargets.Count; k++)
                {
                    float distanceFromTarget = Vector3.Distance(targetTransform.position, availableTargets[k].transform.position);
                    if (distanceFromTarget < shortestDistance)
                    {
                        shortestDistance = distanceFromTarget;
                        nearestLockOnTarget = availableTargets[k].lockOnTransform;
                    }

                    if (inputHandler.lockOnFlag)
                    {
                        Vector3 relativeEnemyPosition =
                            currentLockOnTarget.InverseTransformPoint(availableTargets[k].transform.position);
                        var distanceFromLeftTarget = currentLockOnTarget.transform.position.x
                            - availableTargets[k].transform.position.x;
                        var distanceFromRightTarget = currentLockOnTarget.transform.position.x
                            + availableTargets[k].transform.position.x;

                        if (relativeEnemyPosition.x > 0.00 && distanceFromLeftTarget < shortestDistanceOfLeftTarget)
                        {
                            shortestDistanceOfLeftTarget = distanceFromLeftTarget;
                            leftLockTarget = availableTargets[k].lockOnTransform;
                        }
                        if (relativeEnemyPosition.x < 0.00 && distanceFromRightTarget < shortestDistanceOfRightTarget)
                        {
                            shortestDistanceOfRightTarget = distanceFromRightTarget;
                            rightLockTarget = availableTargets[k].lockOnTransform;
                        }
                    }
                }
            }
        }
        public void ClearLockOnTargets()
        {
            availableTargets.Clear();
            nearestLockOnTarget = null;
            currentLockOnTarget = null;
        }

        public void SetCameraHeight()
        {
            Vector3 velocity = Vector3.zero;
            Vector3 newLockedPosition = new Vector3(0, lockedPivotPosistion);
            Vector3 newUnlockedPosition = new Vector3(0, unlockedPivotPosition);

            if (currentLockOnTarget != null)
            {
                cameraPivotTransfrom.transform.localPosition = Vector3.SmoothDamp(cameraPivotTransfrom.transform.localPosition, newLockedPosition, ref velocity, Time.deltaTime);
            }
            else
            {
                cameraPivotTransfrom.transform.localPosition = Vector3.SmoothDamp(cameraPivotTransfrom.transform.localPosition, newUnlockedPosition, ref velocity, Time.deltaTime);
            }
        }
    }
}