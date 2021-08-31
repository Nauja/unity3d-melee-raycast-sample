using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class DummyController : MonoBehaviour, IEntity
    {
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
    }
}