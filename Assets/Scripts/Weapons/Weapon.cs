using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>List of all weapons supported in the game</summary>
    public enum EWeaponId
    {
        None,
        Sword,
        Axe
    }

    /// <summary>Action held by the player</summary>
    public class Weapon : MonoBehaviour
    {
        public EWeaponId id
        {
            get { return _data.id; }
        }
        
        /// <summary>Data for this weapon</summary>
        private WeaponData _data;
        public WeaponData data
        {
            get { return _data; }
        }

        /// <summary>Init this weapon from data</summary>
        public void Init(WeaponData data)
        {
            _data = data;
        }
    }
}