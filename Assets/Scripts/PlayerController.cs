using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    /// <summary>Player controller for handling inputs and physics</summary>
    public class PlayerController : MonoBehaviour, IEntity
    {
        [Header("Components")]
        [Tooltip("Reference to the AnimatorController attached to this player")]
        [SerializeField]
        private Animator _animator;
        public Animator animator
        {
            get { return _animator; }
        }

        [Tooltip("Reference to the CharacterController attached to this player")]
        [SerializeField]
        private CharacterController _characterController;

        [Tooltip("Reference to the InputController attached to this player")]
        [SerializeField]
        private InputController _inputController;
        public InputController inputController
        {
            get { return _inputController; }
        }
        
        [Tooltip("Bone we attach weapons to")]
        [SerializeField]
        private GameObject _weaponSlot;

        [Header("Physics")]
        [Tooltip("Movement speed")]
        [SerializeField]
        private float _moveSpeed;
        
        /// <summary>Last input movement from user</summary>
        private Vector2 _inputMovement;
        /// <summary>Current velocity</summary>
        private Vector3 _velocity;
        /// <summary>If player is currently moving</summary>
        private bool _isMoving;
        /// <summary>Disable movement when true</summary>
        private bool _canMove = true;
        /// <summary>Equipped weapon</summary>
        private Weapon _weapon;
        /// <summary>Local controller for the player actions</summary>
        private ActionController _actionController;
        public ActionController actionController
        {
            get { return _actionController; }
        }

        public bool isImmune
        {
            get { return false; }
        }

        private void Awake()
        {
            _actionController = new ActionController(this);
        }

        private void Start()
        {
            Equip(EWeaponId.Sword);
        }

        /// <summary>Toggle Moving animation</summary>
        private void Update()
        {
            var oldIsMoving = _isMoving;
            _isMoving = !Mathf.Approximately(_velocity.x + _velocity.z, 0.0f);

            if (oldIsMoving && !_isMoving)
            {
                _animator.SetBool("Moving", false);
            }
            else if (!oldIsMoving && _isMoving)
            {
                _animator.SetBool("Moving", true);
            }

            // Push the main weapon action if ActionController is empty
            if (!_actionController.HasAction && _weapon != null && inputController.IsPressed(EInput.Attack))
            {
                _actionController.Play(_weapon.data.actionId);
            }

            // Update running action
            _actionController.Update();
        }

        /// <summary>Handle movement</summary>
        private void FixedUpdate()
        {
            if (_canMove)
            {
                _velocity = Vector3.right * _inputMovement.x + Vector3.forward * _inputMovement.y;
                _characterController.Move(_velocity * _moveSpeed * Time.fixedDeltaTime);
                transform.LookAt(transform.position + _velocity, Vector3.up);
            }
            else
            {
                _velocity = Vector2.zero;
            }
        }

        #region Inputs
        /// <summary>Called when pressing Move input</summary>
        /// <remarks>See Assets/Controls for a list of inputs</remarks>
        private void OnMove(InputValue value)
        {
            _inputMovement = value.Get<Vector2>();
        }
        #endregion

        /// <summary>Called when the player pickup a weapon</summary>
        public void Equip(EWeaponId weaponId)
        {
            // Check not already held
            if (_weaponSlot == null || (_weapon != null && weaponId == _weapon.id))
            {
                return;
            }

            // Check registered
            WeaponData data = GameInstance.singleton.weaponBank.Get(weaponId);
            if (data == null || data.equipPrefab == null)
            {
                throw new Exception($"No data registered for weapon {weaponId}");
            }

            if (_weapon != null)
            {
                // Destroy old weapon
                Destroy(_weapon.gameObject);
            }

            // Instantiate new weapon
            _weapon = Instantiate(data.equipPrefab, _weaponSlot.transform).GetComponent<Weapon>();
            _weapon.Init(data);
            _weapon.transform.localPosition = Vector3.zero;
            _weapon.transform.localRotation = Quaternion.identity;

            // Override animations
            _animator.runtimeAnimatorController = data.animatorOverride;

            GameEvents.WeaponEquipped(weaponId);
        }

        public virtual void OnHit(int damage)
        {}

        private void OnDrawGizmos()
        {
            if (_actionController != null)
            {
                _actionController.OnDrawGizmos();
            }
        }
    }
}