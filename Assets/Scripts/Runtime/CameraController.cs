using System;
using UnityEngine;

namespace Runtime
{
    public class CameraController : MonoBehaviour
    {
        private Transform _target = null;
        [SerializeField] private float dampSpeed = 4;
        [SerializeField] private Vector3 camOffset = new Vector3(0,3,0);

        
        private void Start()
        {
            _target = Managers.Game.Player.transform;
        }

        private void LateUpdate()
        {
            Vector3 targetPos = _target.transform.position + camOffset;
            targetPos.z = transform.position.z;
            transform.position =
                Vector3.Lerp(
                    transform.position,
                    targetPos,
                    dampSpeed *Time.deltaTime);
        }
    }
}