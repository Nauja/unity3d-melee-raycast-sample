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
    }
}