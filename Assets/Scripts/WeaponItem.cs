using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PM
{
    [CreateAssetMenu(menuName = "item/Weapon Item")]
    public class WeaponItem : Item
    {
        public GameObject modelPrefab;
        public bool isUnarmed;
    }
}