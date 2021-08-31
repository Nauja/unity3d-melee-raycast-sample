using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    /// <summary>All possible inputs for InputController</summary>
    public enum EInput
    {
        None,
        Attack
    }

    /// <summary>
    /// The role of this controller is to receive player inputs and
    /// queue them in a buffer for a short period of time so they
    /// can be consumed even if not pressed at the right frame.
    /// </summary>
    public class InputController : MonoBehaviour
    {
        /// <summary>Save an input and the time it was pressed</summary>
        private struct InputTime
        {
            /// <summary>Which input</summary>
            public EInput input;

            /// <summary>Time it was pressed</summary>
            public float time;
        }

        [Tooltip("Allowed lifetime for buffered inputs")]
        [SerializeField]
        private float _inputLifetime;
        public float inputLifetime
        {
            get { return _inputLifetime; }
        }

        /// <summary>All pressed inputs</Summary>
        private List<InputTime> _inputs;

        private void Awake()
        {
            _inputs = new List<InputTime>();
        }

        /// <summary>Called when pressing Attack input</summary>
        /// <remarks>See Assets/Controls for a list of inputs</remarks>
        private void OnAttack(InputValue value)
        {
            _inputs.Add(new InputTime {input=EInput.Attack, time=Time.time});

            GameEvents.InputPressed(EInput.Attack);
        }

        private void Update()
        {
            // Remove expired inputs
            var size = _inputs.Count;
            for (var i = 0; i < size; ++i)
            {
                if (_inputs[i].time + _inputLifetime < Time.time)
                {
                    _inputs.RemoveAt(i);
                    --size;
                }
            }
        }

        /// <summary>Check if an input has been pressed and is not expired</summary>
        public bool IsPressed(EInput input)
        {
            foreach (var _ in _inputs)
            {
                if (_.input == input)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
