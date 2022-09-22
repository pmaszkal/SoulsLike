using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PM
{
    public class CharacterManager : MonoBehaviour
    {
        [Header("Lock on Transform")]
        public Transform lockOnTransform;

        [Header("Combat Colliders")]
        public CriticalDamageCollider backStabCollider;
        public CriticalDamageCollider riposteCollider;

        [Header("Combat Flags")]
        public bool canBeRiposted;
        public bool canBeParried;
        public bool isBlocking;
        public bool isParrying;

        //damage will be inflicted rugin an animation event
        //used in backstab or riposte animations
        public int pendingCriticalDamage;
    }
}