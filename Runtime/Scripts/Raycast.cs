using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Raycast : MonoBehaviour
{
    protected float t;
    protected bool raycastActive;

    [Header("Raycast")]
    [SerializeField]
    protected Transform raycastStart;
    [SerializeField]
    protected float raycastLenght;
    [SerializeField]
    protected LayerMask raycastLayer;

    [Header("Delay")]
    [SerializeField]
    protected float raycastDelay;

    [Header("Event")]
    [SerializeField]
    protected string broadcastMessage;
    [SerializeField]
    protected UnityEvent OnShootRaycast;
    [SerializeField]
    protected UnityEvent OnHitTarget;

    /// <summary>
    /// Send one Raycast
    /// </summary>
    public void SendRaycast()
    {
        OnShootRaycast.Invoke();
        if(Physics.Raycast(raycastStart.position, raycastStart.forward, out RaycastHit hit, raycastLenght, raycastLayer))
        {
            if(broadcastMessage != "")
            {
                hit.transform.gameObject.BroadcastMessage(broadcastMessage);
            }
            OnHitTarget.Invoke();
        }
    }

    /// <summary>
    /// Send Multiple Raycast
    /// </summary>
    public void ActivateRaycast()
    {
        raycastActive = true;
        t = 0;
    }

    public void DeactivateRaycast()
    {
        raycastActive = false;
    }

    private void Update()
    {
        if (raycastActive)
        {
            t += Time.deltaTime;
            if(t >= raycastDelay)
            {
                t = 0;
                SendRaycast();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(raycastStart.position, raycastStart.forward * raycastLenght);
    }
}
