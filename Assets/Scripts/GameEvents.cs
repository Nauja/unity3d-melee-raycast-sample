using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>Helper class for listening to game events</summary>
    public static class GameEvents
    {
        /// <summary>Called when a new weapon has been equipped</summary>
        public delegate void OnWeaponEquippedDelegate(EWeaponId weapon);
        public static event OnWeaponEquippedDelegate OnWeaponEquipped;

        /// <summary>Called when a new action started</summary>
        public delegate void OnActionStartedDelegate(Action action);
        public static event OnActionStartedDelegate OnActionStarted;

        /// <summary>Called when an input has been pressed</summary>
        public delegate void OnInputPressedDelegate(EInput input);
        public static event OnInputPressedDelegate OnInputPressed;

        public static void WeaponEquipped(EWeaponId weapon)
        {
            if (OnWeaponEquipped != null)
            {
                OnWeaponEquipped(weapon);
            }
        }

        public static void ActionStarted(Action action)
        {
            if (OnActionStarted != null)
            {
                OnActionStarted(action);
            }
        }

        public static void InputPressed(EInput input)
        {
            if (OnInputPressed != null)
            {
                OnInputPressed(input);
            }
        }
    }
}