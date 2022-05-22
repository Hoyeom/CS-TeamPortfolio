using UnityEngine;

namespace Runtime.Contents
{
    public abstract class ItemBase : MonoBehaviour
    {
        public abstract void PickUp();
        
        protected void OnTriggerEnter2D(Collider2D col)
        {
            PickUp();
        }
    }
}