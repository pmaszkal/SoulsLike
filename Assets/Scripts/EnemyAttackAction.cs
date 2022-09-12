using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PM
{
    [CreateAssetMenu(menuName ="AI/Enemy Actions/Attack Action")]
    public class EnemyAttackAction : EnemyAction
    {
        public int attackScore = 3;
        public float recoveryTime = 2;

        public float maximumAttackAngle = 35;
        public float minimumAttackAngle = -35;

        public float minimumDistanceToAttack = 0;
        public float maximumDistanceNeededToAttack = 3;
    }
}