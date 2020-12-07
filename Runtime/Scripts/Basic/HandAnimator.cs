using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

namespace Com.TeoDiaz.VR
{
    public class HandAnimator : MonoBehaviour
    {
        protected Animator animator;
        public bool animate;
        protected readonly List<Finger> gripFingers = new List<Finger>()
    {
        new Finger(FingerType.Middle),
        new Finger(FingerType.Ring),
        new Finger(FingerType.Pinky)
    };

        protected readonly List<Finger> pointFingers = new List<Finger>()
    {
        new Finger(FingerType.Index),
        new Finger(FingerType.Thumb),
    };

        [SerializeField]
        protected float speed = 5;

        [SerializeField]
        protected XRController controller = null;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (animate)
            {
                //Store Input
                CheckGrip();
                CheckPointer();
                //Smooth Input Value
                SmoothFinger(pointFingers);
                SmoothFinger(gripFingers);
                //Apply smoothed Values
                AnimateFinger(pointFingers);
                AnimateFinger(gripFingers);
            }

        }

        private void CheckGrip()
        {
            if (controller.inputDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
            {
                SetFingerTargets(gripFingers, gripValue);
            }
        }

        private void CheckPointer()
        {
            if (controller.inputDevice.TryGetFeatureValue(CommonUsages.trigger, out float pointerValue))
            {
                SetFingerTargets(pointFingers, pointerValue);
            }
        }

        private void SetFingerTargets(List<Finger> fingers, float value)
        {
            foreach (Finger finger in fingers)
            {
                finger.target = value;
            }
        }

        private void SmoothFinger(List<Finger> fingers)
        {
            foreach (Finger finger in fingers)
            {
                float time = speed * Time.unscaledDeltaTime;
                finger.current = Mathf.MoveTowards(finger.current, finger.target, time);
            }
        }

        protected void AnimateFinger(List<Finger> fingers)
        {
            foreach (Finger finger in fingers)
            {
                AnimateFinger(finger.type.ToString(), finger.current);
            }
        }

        public void AnimateFinger(string finger, float blend)
        {
            animator.SetFloat(finger, blend);
        }

        public void DebugAnimateFinger(string finger, float blend)
        {
            Animator debugAnimator = GetComponent<Animator>();
            debugAnimator.SetFloat(finger, blend);
        }
    }
}