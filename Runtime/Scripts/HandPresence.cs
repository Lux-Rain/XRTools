using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

namespace Com.TeoDiaz.VR
{
    public class HandPresence : MonoBehaviour
    {
        protected Interactible interactible;
        protected bool isUsing = false;
        protected bool OnUsing = false;
        protected InputDevice device;
        protected Rigidbody rb;
        protected Coroutine grabDistanceCoroutine;

        [Header("Devices")]
        [SerializeField]
        protected InputDeviceCharacteristics deviceCharacteristics;
        [SerializeField]
        protected XRController controller;

        [Header("Grab")]
        [SerializeField]
        protected Transform takePosition;
        [SerializeField]
        protected float takeRadius;
        [SerializeField]
        protected LayerMask takeLayer;
        [Header("Grab Distance")]
        [SerializeField]
        protected float grabDistance;
        [SerializeField]
        protected LayerMask detectionDistanceLayer;
        [SerializeField]
        protected LineRenderer grabDistanceLine;

        public Rigidbody Rb => rb;
        public Transform TakePosition => takePosition;
        public Interactible Interactible { get => interactible; set => interactible = value; }

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void Start()
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
            Input();
        }


        private void Input()
        {
            device.TryGetFeatureValue(CommonUsages.gripButton, out bool gripbool);
            if (gripbool)
            {
                //Grab
                if (!isUsing)
                {
                    isUsing = true;
                    Take();
                }

                //GrabDistance
                if (!interactible)
                {
                    grabDistanceLine.gameObject.SetActive(true);
                    if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, grabDistance, detectionDistanceLayer))
                    {
                        if (hit.transform.TryGetComponent(out Interactible interactible))
                        {
                            if (grabDistanceCoroutine == null)
                            {
                                grabDistanceCoroutine = StartCoroutine(MoveInteractible(interactible));
                            }
                        }
                        else
                        {
                            grabDistanceLine.SetPosition(0, transform.position);
                            grabDistanceLine.SetPosition(1, hit.point);
                        }
                    }
                    else
                    {
                        grabDistanceLine.SetPosition(0, transform.position);
                        grabDistanceLine.SetPosition(1, transform.position + transform.forward * grabDistance);
                    }
                }
            }
            else
            {
                //Drop
                if (isUsing)
                {
                    isUsing = false;
                    Drop();

                    //Stop GrabDistance;
                    if (grabDistanceCoroutine != null)
                    {
                        StopCoroutine(grabDistanceCoroutine);
                        grabDistanceCoroutine = null;
                    }
                    grabDistanceLine.gameObject.SetActive(false);
                }

            }

            device.TryGetFeatureValue(CommonUsages.triggerButton, out bool triggerBool);
            if (triggerBool)
            {
                if (!OnUsing)
                {
                    OnUsing = true;
                    if (interactible)
                    {
                        interactible.StartUsing();
                    }
                }


            }
            else
            {
                if (OnUsing)
                {
                    OnUsing = false;
                    if (interactible)
                    {
                        interactible.StopUsing();

                    }
                }
            }

        }

        /// <summary>
        /// Move Interactible To Hand
        /// </summary>
        /// <param name="interactible"></param>
        /// <returns></returns>
        public IEnumerator MoveInteractible(Interactible interactible)
        {
            while (Vector3.Distance(transform.position, interactible.transform.position) > 1)
            {
                grabDistanceLine.SetPosition(0, transform.position);
                grabDistanceLine.SetPosition(1, interactible.transform.position);
                interactible.transform.position = Vector3.Slerp(interactible.transform.position, transform.position, 10 * Time.deltaTime);
                yield return null;
            }
            interactible.transform.position = transform.position;
            Take(interactible);
            grabDistanceLine.gameObject.SetActive(false);
            grabDistanceCoroutine = null;
        }

        /// <summary>
        /// Lache l'interactible
        /// </summary>
        public void Drop()
        {
            Debug.Log("Try Drop");
            if (interactible)
            {
                interactible.Drop();
            }
        }

        /// <summary>
        /// Prend l'interactible le plus proche
        /// </summary>
        protected bool Take()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, takeRadius, takeLayer);
            List<Interactible> interactibles = new List<Interactible>();
            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent(out Interactible interactible))
                {
                    interactibles.Add(interactible);
                }
                else if (collider.transform.parent != null)
                {
                    if (collider.transform.parent.TryGetComponent(out Interactible interactibleParent))
                    {
                        interactibles.Add(interactibleParent);
                    }

                }
            }
            Debug.Log("Try Take");
            if (interactibles.Count != 0)
            {
                interactibles = interactibles.OrderBy(x => Vector3.Distance(takePosition.position, x.transform.position)).ToList();
                Take(interactibles[0]);
                return true;
            }
            return false;

        }
        /// <summary>
        /// Prend un interactible definie
        /// </summary>
        /// <param name="interactible"></param>
        public void Take(Interactible interactible)
        {
            Debug.Log("Take");
            if (interactible.CurrentHand)
            {
                interactible.CurrentHand.Drop();
            }
            this.interactible = interactible.Take(this);
        }

        /// <summary>
        /// Active une vibration d'amplitude 0.5 et de durée 0.1
        /// </summary>
        public void Haptic()
        {
            if (device.TryGetHapticCapabilities(out HapticCapabilities capabilities))
            {
                if (capabilities.supportsImpulse)
                {
                    uint channel = 0;
                    float amplitude = 0.5f;
                    float duration = 0.1f;
                    device.SendHapticImpulse(channel, amplitude, duration);
                    return;
                }
            }
        }
        /// <summary>
        /// Active une vibration de durée 0.1
        /// </summary>
        /// <param name="amplitude"></param>
        public void Haptic(float amplitude)
        {
            if (device.TryGetHapticCapabilities(out HapticCapabilities capabilities))
            {
                if (capabilities.supportsImpulse)
                {
                    uint channel = 0;
                    float duration = 0.1f;
                    device.SendHapticImpulse(channel, amplitude, duration);
                    Debug.Log("HapticFeedback");
                    return;
                }

                Debug.LogWarning("Unsupported");
            }
        }

        /// <summary>
        /// Active une vibration
        /// </summary>
        /// <param name="amplitude"></param>
        /// <param name="duration"></param>
        public void Haptic(float amplitude, float duration)
        {
            if (device.TryGetHapticCapabilities(out HapticCapabilities capabilities))
            {
                if (capabilities.supportsImpulse)
                {
                    uint channel = 0;
                    device.SendHapticImpulse(channel, amplitude, duration);
                    Debug.Log("HapticFeedback");
                    return;
                }

                Debug.LogWarning("Unsupported");
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, takeRadius);
        }
    }
}
