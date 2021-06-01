using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour {

    public float lifetimeInSeconds;
    private MeshRenderer mr;
    private Rigidbody rb;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        mr = GetComponent<MeshRenderer>();
    }

    private void Start() {
        StartCoroutine(BulletLifeTimer());
    }

    private void OnTriggerEnter(Collider other) {
        if (other.transform.GetComponent<Target>() != null) {
            other.transform.GetComponent<Target>().OnHit();
        }
        StartCoroutine(DestroyBullet());
    }

    private IEnumerator DestroyBullet() {
        mr.enabled = false;
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        Destroy(gameObject);

        yield return null;
    }

    private IEnumerator BulletLifeTimer() {
        yield return new WaitForSeconds(lifetimeInSeconds);

        StartCoroutine(DestroyBullet());
    }
}
