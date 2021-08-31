using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>Handle interaction with pickeable objects</summary>
    public class Pickup : MonoBehaviour
    {
        /// <summary>Root game object to hide/show</summary>
        [SerializeField]
        private GameObject _root;
        /// <summary>Weapon to pickup</summary>
        [SerializeField]
        private EWeaponId _weaponId;
        /// <summary>Pickup height</summary>
        [SerializeField]
        private float _height = 2.0f;
        /// <summary>Pickup tilt</summary>
        [SerializeField]
        private float _tilt = 25.0f;
        /// <summary>Pickup rotation speed</summary>
        [SerializeField]
        private float _rotationSpeed;
        /// <summary>TextMesh for pickup name</summary>
        [SerializeField]
        private TextMesh _textMesh;
        private float _rotation;

        private void Awake()
        {
            // Hide the pickup if already equipped
            GameEvents.OnWeaponEquipped += (EWeaponId weaponId) => {
                _root.SetActive(weaponId != _weaponId);
            };
        }

        private void Start()
        {
            // Spawn the pickup
            var weapon = GameInstance.singleton.weaponBank.Get(_weaponId);
            if (weapon != null && weapon.pickupPrefab != null)
            {
                var o = Instantiate(weapon.pickupPrefab, _root.transform);
                o.transform.localPosition = Vector3.up * _height + weapon.pickupOffset;
                o.transform.localRotation = Quaternion.Euler(_tilt, 0, 0);
                _textMesh.text = weapon.displayName;
            }
        }

        private void Update()
        {
            _rotation += _rotationSpeed * Time.deltaTime;
            _root.transform.localRotation = Quaternion.Euler(0, _rotation, 0);
        }

        private void OnTriggerEnter(Collider other)
        {
            var player = other.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.Equip(_weaponId);
            }
        }
    }
}