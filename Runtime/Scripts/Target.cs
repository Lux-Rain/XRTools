using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Target : MonoBehaviour
{
    [SerializeField]
    protected UnityEvent OnHit;

    virtual public void Hit()
    {
        OnHit.Invoke();
    }
}
