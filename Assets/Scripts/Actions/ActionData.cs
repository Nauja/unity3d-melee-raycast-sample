using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>Informations about the execution phase of an action</summary>
    [Serializable]
    public struct ExecPhase
    {
        [Tooltip("Delay in seconds before the execution phase starts")]
        public float delay;

        [Tooltip("Time in seconds the execution phase lasts")]
        public float duration;
    }

    /// <summary>Informations about a combo action that can be triggered during an action</summary>
    [Serializable]
    public struct ComboPhase
    {
        [Tooltip("Delay in seconds before combo becomes available")]
        public float delay;

        [Tooltip("Time in seconds the combo is available")]
        public float duration;

        [Tooltip("Action that can be triggered")]
        public EActionId actionId;
    }

    /// <summary>Describe a single action with its visual and gameplay data</summary>
    [CreateAssetMenu(fileName = "ActionData", menuName = "ScriptableObjects/ActionData", order = 1)]
    public class ActionData : ScriptableObject
    {
        [Tooltip("Unique id of the action")]
        public EActionId id;

        [Header("Visual")]
        [Tooltip("Trigger of the animation to play")]
        public string anim;

        [Tooltip("Name displayed in UI")]
        public string displayName;

        [Header("Gameplay")]
        [Tooltip("Which gameplay logic to apply")]
        public EActionLogic logic;

        [Tooltip("Duration in seconds that this Action takes to play")]
        public float duration;

        [Tooltip("Speed multiplier for animation and Action")]
        public float speed;

        [Tooltip("Damages caused by this Action to other entities")]
        public int damage;

        [Header("Phases")]
        [Tooltip("When is the execution phase")]
        public ExecPhase exec;

        [Tooltip("Possible action that can be triggered during this action")]
        public ComboPhase combo;
    }
}