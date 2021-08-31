using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    /// <summary>Contains all actions available in the game</summary>
    [CreateAssetMenu(fileName = "ActionBank", menuName = "ScriptableObjects/ActionBank", order = 1)]
    public class ActionBank : ScriptableObject
    {
        /// <summary>All possible actions</summary>
        public ActionData[] actions;

        /// <summary>Get an action by id</summary>
        public ActionData Get(EActionId id)
        {
            return actions.Where(_ => _.id == id).FirstOrDefault();
        }
    }
}