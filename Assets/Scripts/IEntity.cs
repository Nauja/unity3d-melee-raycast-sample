using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public interface IEntity
    {
        GameObject gameObject
        {
            get;
        }

        Animator animator
        {
            get;
        }

        InputController inputController
        {
            get;
        }

        ActionController actionController
        {
            get;
        }

        /// <summary>Return true if immune to attacks</summary>
        bool isImmune
        {
            get;
        }

        /// <summary>Called when hit by an attack</summary>
        void OnHit(int damage);
    }
}