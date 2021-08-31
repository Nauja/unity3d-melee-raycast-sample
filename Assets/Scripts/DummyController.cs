using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class DummyController : MonoBehaviour, IEntity
    {
        [Header("Components")]
        [Tooltip("Reference to the FlashController attached to this entity")]
        [SerializeField]
        private FlashController _flashController;

        [Header("Gameplay")]
        [Tooltip("Time in seconds this entity is immune to attacks after being hit")]
        [SerializeField]
        private float _immuneDuration;

        private Animator _animator;
        public Animator animator
        {
            get { return _animator; }
        }

        private InputController _inputController;
        public InputController inputController
        {
            get { return _inputController; }
        }

        private ActionController _actionController;
        public ActionController actionController
        {
            get { return _actionController; }
        }

        /// <summary>Time in seconds this entity began immune to attacks</summary>
        private float _immuneTime;

        private bool _isImmune;
        public bool isImmune
        {
            get { return _isImmune; }
        }

        private void Update()
        {
            if (_isImmune && (_immuneTime + _immuneDuration) <= Time.time)
            {
                _isImmune = false;
            }
        }

        public virtual void OnHit(int damage)
        {
            if (!isImmune)
            {
                _isImmune = true;
                _immuneTime = Time.time;
                _flashController.Flash();
            }
        }
    }
}