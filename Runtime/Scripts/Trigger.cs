using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Com.TeoDiaz.VR
{
    public class Trigger : MonoBehaviour
    {
        [SerializeField]
        protected bool once;
        [SerializeField]
        protected Collider[] colliders;
        [SerializeField]
        protected string onTriggerEnterMessage;
        [SerializeField]
        protected string onTriggerExitMessage;
        [SerializeField]
        protected UnityEvent TriggerEnter;
        [SerializeField]
        protected UnityEvent TriggerExit;

        private void OnTriggerEnter(Collider other)
        {
            if (once)
            {
                gameObject.SetActive(false);
            }

            if (colliders.Length != 0)
            {
                TriggerEnter.Invoke();
                return;
            }

            other.gameObject.BroadcastMessage(onTriggerEnterMessage);


        }

        private void OnTriggerExit(Collider other)
        {
            if (once)
            {
                gameObject.SetActive(false);
            }

            if (colliders.Length != 0)
            {
                TriggerExit.Invoke();
                return;
            }
            other.gameObject.BroadcastMessage(onTriggerExitMessage);
        }
    }
}
