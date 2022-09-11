using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PM
{
    public class EnemyManager : CharacterManager
    {
        EnemyLocomotionManager enemyLocomotionManager;
        bool isPerformingAction;

        [Header("AI Settings")]
        public float detectionRadius = 20;
        public float maximumDetectionAngle = 50;
        public float minimumDetectionAngle = - 50;

        private void Awake()
        {
            enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();   
        }

        private void Update()
        {
            HandleCurrentAction();
        }

        private void HandleCurrentAction()
        {
            if (enemyLocomotionManager.currentTarget == null)
            {
                enemyLocomotionManager.HandleDetection();
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red; //replace red with whatever color you prefer
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
            Vector3 fovLine1 = Quaternion.AngleAxis(maximumDetectionAngle, transform.up) * transform.forward * detectionRadius;

            Vector3 fovLine2 = Quaternion.AngleAxis(minimumDetectionAngle, transform.up) * transform.forward * detectionRadius;

            Gizmos.color = Color.blue;

            Gizmos.DrawRay(transform.position, fovLine1);

            Gizmos.DrawRay(transform.position, fovLine2);
        }
    }
}