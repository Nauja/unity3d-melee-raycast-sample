using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>Action for basic melee attacks that hit enemies in front of the doer</sumarry>
    public class MeleeAction : Action
    {
        private LayerMask _layerMask;

        public MeleeAction(IEntity parent, ActionData data)
            : base(parent, data)
        {
            _layerMask = LayerMask.GetMask("Default");
        }

        public override bool Update()
        {
            if (!base.Update())
            {
                return false;
            }

            // Check if we are in exec phase
            if (isExecPhase)
            {
                Vector3 position, extents, direction;
                GetHitbox(out position, out extents, out direction);

                RaycastHit[] hits = Physics.BoxCastAll(position, extents, direction, Quaternion.identity, _layerMask);

                foreach (var hit in hits)
                {
                    IEntity entity = hit.collider.gameObject.GetComponent<IEntity>();
                    if (entity == null || entity == parent)
                    {
                        continue;
                    }

                    Debug.Log($"Hit {entity}");
                }
            }

            return true;
        }

        public override void OnDrawGizmos()
        {
            if (isExecPhase)
            {
                Vector3 position, extents, direction;
                GetHitbox(out position, out extents, out direction);

                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(position + direction, extents);
            }
        }

        /// <summary>Compute the hitbox position and size</summary>
        protected virtual void GetHitbox(out Vector3 position, out Vector3 extents, out Vector3 direction)
        {
            position = parent.gameObject.transform.position;
            extents = new Vector3(5.0f, 5.0f, 5.0f);
            direction = parent.gameObject.transform.forward * 10.0f;
        }
    }
}
