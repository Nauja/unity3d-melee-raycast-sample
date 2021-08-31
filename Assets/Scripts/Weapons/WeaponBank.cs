using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    /// <summary>Contains all weapons available in the game</summary>
    [CreateAssetMenu(fileName = "WeaponBank", menuName = "ScriptableObjects/WeaponBank", order = 1)]
    public class WeaponBank : ScriptableObject
    {
        /// <summary>All possible weapons</summary>
        public WeaponData[] weapons;

        /// <summary>Get a weapon by id</summary>
        public WeaponData Get(EWeaponId id)
        {
            return weapons.Where(_ => _.id == id).FirstOrDefault();
        }
    }
}