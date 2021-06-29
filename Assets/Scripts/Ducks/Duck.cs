using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Duck : Target {

    public float duckLifeTime;

    internal Launcher launchedBy;
    private Rigidbody rb;
    private bool broken;
    private bool superDuck;

    private void Start() {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        StartCoroutine(DuckLifeTimer());
    }

    private void Update() {
        if (!broken) {
            transform.rotation = Quaternion.LookRotation(rb.velocity);
        }
    }

    public override void OnHit() {
        if (superDuck) {
            GameManager.instance.Hit(true);
        } else {
            GameManager.instance.Hit();
        }
        StartCoroutine(DestroyObject());
    }

    protected override IEnumerator DestroyObject() {
        launchedBy.launchedTargets.Remove(this);
        rb.velocity = Vector3.zero;
        broken = true;
        GameManager.instance.destroyedThisLevel++;
        Destroy(gameObject);
        yield return null;
    }

    public IEnumerator DuckLifeTimer() {
        yield return new WaitForSeconds(duckLifeTime);
        StartCoroutine(DestroyObject());
    }

    public void MakeSuperDuck() {
        superDuck = true;
    }
}
