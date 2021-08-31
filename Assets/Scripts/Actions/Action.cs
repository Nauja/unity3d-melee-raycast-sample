using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>List of all actions supported in the game</summary>
    public enum EActionId
    {
        None,
        SwordAttack1,
        SwordAttack2,
        SwordAttack3,
        AxeAttack1,
        AxeAttack2,
        AxeAttack3,
    }

    /// <summary>List of all Types of Actions. There is a many-to-one mapping of Actions to EActionLogics</summary>
    public enum EActionLogic
    {
        Melee,
        RangedTargeted,
    }

    /// <summary>Action performed by an entity</summary>
    public abstract class Action
    {
        /// <summary>Entity performing the action</summary>
        private IEntity _parent;
        public IEntity parent
        {
            get { return _parent; }
        }

        /// <summary>Data for this action</summary>
        private ActionData _data;
        public ActionData data
        {
            get { return _data; }
        }

        /// <summary>
        /// Time when this Action was started (from Time.time) in seconds. Set by the ActionPlayer or ActionVisualization.
        /// </summary>
        public float timeStarted { get; set; }

        /// <summary>
        /// How long the Action has been running (since its Start was called)--in seconds, measured via Time.time.
        /// </summary>
        public float timeRunning { get { return (Time.time - timeStarted); } }

        /// <summary>Indicate if the action is in exec phase</summary>
        public bool isExecPhase
        {
            get
            {
                var time = timeRunning;
                var delay = data.exec.delay / data.speed;
                var duration = data.exec.duration / data.speed;
                return duration > 0.0f && time >= delay && time < delay + duration;
            }
        }

        /// <summary>Indicate if the action is in combo phase</summary>
        public bool isComboPhase
        {
            get
            {
                var time = timeRunning;
                var delay = data.combo.delay / data.speed;
                var duration = data.combo.duration / data.speed;
                return duration > 0.0f && time >= delay && time < delay + duration;
            }
        }

        public Action(IEntity parent, ActionData data)
        {
            _parent = parent;
            _data = data;
        }

        /// <summary>
        /// Called when the Action starts actually playing (which may be after it is created, because of queueing).
        /// </summary>
        /// <returns>false if the action decided it doesn't want to run after all, true otherwise. </returns>
        public virtual bool Start()
        {
            if (_data.anim != "")
            {
                // Trigger the animation for this action
                Debug.Log($"Trigger animation {_data.anim}");
                _parent.animator.SetTrigger(_data.anim);
                _parent.animator.SetFloat("AttackSpeed", _data.speed);
            }

            return true;
        }

        /// <summary>
        /// Called each frame while the action is running.
        /// </summary>
        /// <returns>true to keep running, false to stop. The Action will stop by default when its duration expires, if it has a duration set. </returns>
        public virtual bool Update()
        {
            // Check for combo directly from the action
            EActionId actionId;
            if (CanCombo(out actionId) && _parent.inputController.IsPressed(EInput.Attack))
            {
                _parent.actionController.Play(actionId);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Called when the Action ends naturally. By default just calls Cancel()
        /// </summary>
        public virtual void End()
        {
            Cancel();
        }

        /// <summary>
        /// This will get called when the Action gets canceled. The Action should clean up any ongoing effects at this point.
        /// (e.g. an Action that involves moving should cancel the current active move).
        /// </summary>
        public virtual void Cancel() { }
        
        public virtual void OnDrawGizmos() { }

        /// <summary>Check if entity can perform a combo during this action</summary>
        /// <param name="actionId">Return the action for possible combo</param>
        /// <returns>If a combo is possible</returns>
        public bool CanCombo(out EActionId actionId)
        {
            actionId = data.combo.actionId;
            return isComboPhase;
        }

        /// <summary>Factory method that creates Actions for requested gameplay logic</summary>
        /// <param name="parent">Entity performing the action</param>
        /// <param name="data">Action data</param>
        /// <returns>The newly created action</returns>
        public static Action Create(IEntity parent, ActionData data)
        {
            switch (data.logic)
            {
                case EActionLogic.Melee: return new MeleeAction(parent, data);
                default: throw new System.NotImplementedException();
            }
        }
    }
}
