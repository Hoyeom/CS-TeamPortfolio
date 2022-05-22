using System;
using UnityEngine;

namespace Runtime.Contents
{
    public class Platform : MonoBehaviour
    {
        public Define.Dir Dir { get; set; } = Define.Dir.Left;
        public Vector3 Pos { get; set; } = Vector3.zero;

        private void OnEnable()
        {
            Pos = transform.position;
        }
        
    }
}