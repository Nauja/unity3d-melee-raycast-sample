using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>Describe a single weapon with its visual and gameplay data</summary>
    [CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObjects/WeaponData", order = 1)]
    public class WeaponData : ScriptableObject
    {
        [Tooltip("Unique id of the weapon")]
        public EWeaponId id;

        [Header("Pickup")]
        [Tooltip("Name displayed when on floor")]
        public string displayName;

        [Tooltip("Prefab for when on floor")]
        public GameObject pickupPrefab;

        [Tooltip("Weapon offset when on floor")]
        public Vector3 pickupOffset;

        [Header("Equip")]
        [Tooltip("Prefab for when equipped on player")]
        public GameObject equipPrefab;

        [Tooltip("Main attack granted by this weapon")]
        public EActionId actionId;

        [Tooltip("Overriden animations")]
        public AnimatorOverrideController animatorOverride;
    }
}