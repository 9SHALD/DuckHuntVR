using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[System.Serializable]
public class DelayedEvent {
    public float delay;
    public UnityEvent unityEvent;
}

public class StartButtonTemp : Target
{


    [Header("Events")]
    public UnityEvent onInteract;

    public override void OnHit() {
        onInteract.Invoke();
        gameObject.SetActive(false);
    }

    protected override IEnumerator DestroyObject() => throw new System.NotImplementedException();
}
