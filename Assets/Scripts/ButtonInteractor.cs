using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[System.Serializable]
public class DelayedEvent {
    public float delay;
    public UnityEvent unityEvent;
}

public class ButtonInteractor : Target
{


    [Header("Events")]
    public UnityEvent onInteract;

    public override void OnHit() {
        onInteract.Invoke();
    }

    protected override IEnumerator DestroyObject() => throw new System.NotImplementedException();
}
