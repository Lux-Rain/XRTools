using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Playables;
using UnityEngine.XR;

namespace Com.TeoDiaz.VR
{
    public class VRPlayerController : MonoBehaviour
    {
        protected Rigidbody rb;
        protected InputDevice device;
        protected Vector2 velocity;
        protected Vector3 velocityRef = Vector3.zero;
        [Header("Devices")]
        [SerializeField]
        protected InputDeviceCharacteristics deviceCharacteristics;
        [SerializeField]
        protected Camera playerCamera;
        [SerializeField]
        protected Transform direction;

        [Header("Movement")]
        [SerializeField]
        protected float speed;
        [SerializeField, Range(0, 0.3f)]
        protected float smoothTime = 0.05f;
        [SerializeField]
        protected new CapsuleCollider collider;
        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        protected void Start()
        {
            List<InputDevice> devices = new List<InputDevice>();
            InputDevices.GetDevicesWithCharacteristics(deviceCharacteristics, devices);
            if (devices.Count != 0)
            {
                device = devices[0];
            }
        }

        private void Update()
        {
            CheckDirection();
            Input();
        }

        private void CheckDirection()
        {
            direction.rotation = Quaternion.Euler(0, playerCamera.transform.rotation.eulerAngles.y, 0);
            collider.center = new Vector3(playerCamera.transform.localPosition.x, 0, playerCamera.transform.localPosition.z);
        }

        protected void Input()
        {
            device.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 velocity);
            this.velocity = velocity;
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            Vector3 targetVelocity = (direction.forward * velocity.y + direction.right * velocity.x) * speed + Vector3.up * rb.velocity.y;
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocityRef, smoothTime);
        }
    }
}