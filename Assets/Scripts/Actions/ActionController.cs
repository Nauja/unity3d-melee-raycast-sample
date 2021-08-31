using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>Control actions performed by the player</summary>
    public class ActionController
    {
        /// <summary>Entity owning this action controller</summary>
        private IEntity _parent;

        /// <summary>Current action</summary>
        private List<Action> _queue;

        public ActionController(IEntity parent)
        {
            _parent = parent;
            _queue = new List<Action>();
        }

        /// <summary>Indicate if there is a running action</summary>
        public bool HasAction
        {
            get { return _queue.Count != 0; }
        }

        /// <summary>Play a new action</summary>
        public void Play(EActionId actionId)
        {
            ActionData data = GameInstance.singleton.actionBank.Get(actionId);
            if (data == null)
            {
                throw new Exception($"No registed data for action {actionId}");
            }

            Action action = Action.Create(_parent, data);
            Debug.Log($"Play action {action.data.id}");
            _queue.Add(action);

            if (_queue.Count == 1)
            {
                StartAction();
            }
        }

        /// <summary>
        /// Starts the action at the head of the queue, if any.
        /// </summary>
        private void StartAction()
        {
            if (_queue.Count == 0)
            {
                return;
            }
        
            Action action = _queue[0];
            Debug.Log($"Start action {action.data.id}");
            action.timeStarted = Time.time;
            bool play = action.Start();
            if (!play)
            {
                AdvanceQueue(false);
                return;
            }

            GameEvents.ActionStarted(action);
        }

        /// <summary>Update running action</summary>
        public void Update()
        {
            if (_queue.Count != 0)
            {
                if (!UpdateAction(_queue[0]))
                {
                    AdvanceQueue(true);
                }
            }
        }

        /// <summary>
        /// Optionally end the currently playing action, and advance to the next Action that wants to play.
        /// </summary>
        /// <param name="endRemoved">if true we call End on the removed element.</param>
        private void AdvanceQueue(bool endRemoved)
        {
            if (_queue.Count > 0)
            {
                if (endRemoved)
                {
                    _queue[0].End();
                }
                _queue.RemoveAt(0);
            }

            StartAction();
        }

        /// <summary>
        /// Calls a given Action's Update() and decides if the action is still alive.
        /// </summary>
        /// <returns>true if the action is still active, false if it's dead</returns>
        private bool UpdateAction(Action action)
        {
            bool keepGoing = action.Update();
            bool expirable = action.data.duration > 0f; //non-positive value is a sentinel indicating the duration is indefinite.
            var timeElapsed = Time.time - action.timeStarted;
            bool timeExpired = expirable && timeElapsed >= action.data.duration;
            return keepGoing && !timeExpired;
        }
        
        public void OnDrawGizmos()
        {
            if (_queue.Count != 0)
            {
                _queue[0].OnDrawGizmos();
            }
        }
    }
}