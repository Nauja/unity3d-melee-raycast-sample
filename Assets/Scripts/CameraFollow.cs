using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class CameraFollow : MonoBehaviour
    {
        public GameObject target;
        public float height = 8.0f;
        public float distance = -11.0f;
        public float right = 0.0f;
        public float angle = 21.0f;
        public float smoothTime = 0.0f;
        private Vector3 _velocity = Vector3.zero;
        
        void Update()
        {
            var targetPosition = target.transform.position + new Vector3(right, height, distance);    
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, smoothTime);
            transform.rotation = Quaternion.Euler(angle, 0, 0);            
        }
    }
}
