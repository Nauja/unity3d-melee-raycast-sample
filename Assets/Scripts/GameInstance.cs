using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>Static instance of our game</singlaton>
    public class GameInstance : MonoBehaviour
    {
        private static GameInstance _instance;
        public static GameInstance singleton
        {
            get { return _instance; }
        }

        [Tooltip("Bank containing all actions")]
        public ActionBank actionBank;

        [Tooltip("Bank containing all weapons")]
        public WeaponBank weaponBank;

        private void Awake()
        {
            _instance = this;
        }
    }
}