using Com.TeoDiaz.Math;
using UnityEngine;
using UnityEngine.Events;

namespace Com.TeoDiaz.VR
{
    public class Interactible : MonoBehaviour
    {
        protected Joint joint;
        protected HandPresence currentHand;

        [Header("TakePosition")]
        [SerializeField]
        protected Vector3 customRotation;
        [SerializeField]
        protected Vector3 customPosition;

        [Header("Hand Position")]
        [SerializeField, Range(0, 1)]
        protected float pinky;
        [SerializeField, Range(0, 1)]
        protected float ring;
        [SerializeField, Range(0, 1)]
        protected float middle;
        [SerializeField, Range(0, 1)]
        protected float index;
        [SerializeField, Range(0, 1)]
        protected float thumb;

        [Header("Debug")]
        [SerializeField]
        protected bool debug;
        [SerializeField]
        protected HandAnimator debugCurrentHand;

        [Header("Event")]
        public UnityEvent OnTake;
        public UnityEvent OnDrop;
        public UnityEvent OnStartUsing;
        public UnityEvent OnStopUsing;
        public UnityEvent OnUse;

        public HandPresence CurrentHand => currentHand;

        public Interactible Take(HandPresence hand)
        {
            if (currentHand)
            {
                return null;
            }
            currentHand = hand;
            transform.parent = currentHand.TakePosition;
            transform.localRotation = Quaternion.Euler(customRotation);
            transform.localPosition = customPosition;
            transform.localScale = MathPlus.MultiplyVector3D(transform.localScale, transform.parent.localScale);
            joint = gameObject.AddComponent<FixedJoint>();
            joint.connectedBody = currentHand.Rb;
            OnTake.Invoke();

            HandAnimator handAnim = currentHand.GetComponentInChildren<HandAnimator>();
            handAnim.animate = false;
            handAnim.AnimateFinger("Pinky", pinky);
            handAnim.AnimateFinger("Ring", ring);
            handAnim.AnimateFinger("Middle", middle);
            handAnim.AnimateFinger("Index", index);
            handAnim.AnimateFinger("Thumb", thumb);
            return this;
        }

        public void StartUsing()
        {
            OnStartUsing.Invoke();
        }

        public void StopUsing()
        {
            OnStopUsing.Invoke();
        }

        public void Use()
        {
            OnUse.Invoke();
        }

        public void Drop()
        {
            HandAnimator handAnim = currentHand.GetComponentInChildren<HandAnimator>();
            handAnim.animate = true;
            transform.parent = null;
            currentHand.Interactible = null;
            currentHand = null;
            Destroy(joint);
            OnDrop.Invoke();
        }


        private void OnValidate()
        {
            if (debug)
            {

                transform.localPosition = customPosition;
                transform.localRotation = Quaternion.Euler(customRotation);

                if (debugCurrentHand)
                {
                    debugCurrentHand.DebugAnimateFinger("Pinky", pinky);
                    debugCurrentHand.DebugAnimateFinger("Ring", ring);
                    debugCurrentHand.DebugAnimateFinger("Middle", middle);
                    debugCurrentHand.DebugAnimateFinger("Index", index);
                    debugCurrentHand.DebugAnimateFinger("Thumb", thumb);
                }
            }
        }

    }
}
