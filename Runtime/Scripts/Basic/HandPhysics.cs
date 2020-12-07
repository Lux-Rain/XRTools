using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Com.TeoDiaz.VR
{
    public class HandPhysics : MonoBehaviour
    {
        protected Rigidbody rb;
        protected Vector3 targetPosition;
        protected Quaternion targetRotation;

        [SerializeField]
        protected float smoothingAmmount = 15.0f;
        [SerializeField]
        protected Transform target;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            TeleportToTarget();
        }

        private void Update()
        {
            SetTargetPosition();
            SetTargetRotation();
        }

        private void SetTargetPosition()
        {
            float time = smoothingAmmount * Time.unscaledDeltaTime;
            targetPosition = Vector3.Lerp(targetPosition, target.position, time);
        }

        private void SetTargetRotation()
        {
            float time = smoothingAmmount * Time.unscaledDeltaTime;
            targetRotation = Quaternion.Lerp(targetRotation, target.rotation, time);
        }

        private void FixedUpdate()
        {
            MoveToController();
            RotateToController();
        }

        private void MoveToController()
        {
            Vector3 positionDelta = targetPosition - target.position;
            rb.velocity = Vector3.zero;
            rb.MovePosition(transform.position + positionDelta);
        }

        private void RotateToController()
        {
            rb.angularVelocity = Vector3.zero;
            rb.MoveRotation(targetRotation);
        }

        public void TeleportToTarget()
        {
            targetPosition = target.position;
            targetRotation = target.rotation;

            transform.position = targetPosition;
            transform.rotation = targetRotation;
        }
    }
}