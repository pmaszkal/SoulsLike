using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PM
{
    public class AttackState : State
    {
        public CombatStanceState combatStanceState;
        public EnemyAttackAction[] enemyAttacks;
        public EnemyAttackAction currentAttack;

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            Vector3 targetDir = enemyManager.currentTarget.transform.position - transform.position;
            float viewabelAngle = Vector3.Angle(targetDir, transform.forward);

            if (enemyManager.isPerformingAction)
                return combatStanceState;

            if (currentAttack != null)
            {
                //if we are too close to the enemy to perform current attack, get a new attack
                if (enemyManager.distanceFromTarget < currentAttack.minimumDistanceNeededToAttack)
                {
                    return this;
                }
                //if we are close enough to attack, then let us proceed
                else if (enemyManager.distanceFromTarget < currentAttack.maximumDistanceNeededToAttack)
                {
                    //if our enemy is within our attack viewable angle, we attack
                    if (enemyManager.viewableAngle < currentAttack.maximumAttackAngle
                        && enemyManager.viewableAngle >= currentAttack.minimumAttackAngle)
                    {
                        if (enemyManager.currentRecoveryTime <= 0 && enemyManager.isPerformingAction == false)
                        {
                            enemyAnimatorManager.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                            enemyAnimatorManager.anim.SetFloat("Horizontal", 0, 0.1f, Time.deltaTime);
                            enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
                            enemyManager.isPerformingAction = true;
                            enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
                            currentAttack = null;
                            return combatStanceState;
                        }
                    }
                }
            }
            else
            {
                GetNewAttack(enemyManager);
            }

            return combatStanceState;

            //if (currentAttack == null)
            //{
            //    GetNewAttack(enemyManager);
            //}
            //else
            //{
            //    enemyManager.isPerformingAction = true;
            //    enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
            //    enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
            //    currentAttack = null;
            //}

            //return this;
        }

        public void GetNewAttack(EnemyManager enemyManager)
        {
            Vector3 targertDirection = enemyManager.currentTarget.transform.position - transform.position;
            float viewableAngle = Vector3.Angle(targertDirection, transform.forward);
            enemyManager.distanceFromTarget = Vector3.Distance(
                enemyManager.currentTarget.transform.position, transform.position);

            int maxScore = 0;

            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                EnemyAttackAction enemyAttackAction = enemyAttacks[i];

                if (enemyManager.distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack
                    && enemyManager.distanceFromTarget > enemyAttackAction.minimumDistanceNeededToAttack)
                {
                    if (viewableAngle <= enemyAttackAction.maximumAttackAngle
                        && viewableAngle > enemyAttackAction.minimumAttackAngle)
                    {
                        maxScore += enemyAttackAction.attackScore;
                    }
                }
            }

            int randomValue = Random.Range(0, maxScore);
            int tempScore = 0;

            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                EnemyAttackAction enemyAttackAction = enemyAttacks[i];

                if (enemyManager.distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack
                    && enemyManager.distanceFromTarget > enemyAttackAction.minimumDistanceNeededToAttack)
                {
                    if (viewableAngle <= enemyAttackAction.maximumAttackAngle
                        && viewableAngle > enemyAttackAction.minimumAttackAngle)
                    {
                        if (currentAttack != null)
                            return;
                        tempScore += enemyAttackAction.attackScore;
                        if (tempScore > randomValue)
                        {
                            currentAttack = enemyAttackAction;
                        }
                    }
                }
            }
        }
    }
}