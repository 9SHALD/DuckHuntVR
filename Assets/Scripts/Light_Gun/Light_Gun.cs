using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light_Gun : Usable
{
    public GameObject bulletPrefab;
    public Transform barrelEnd;
    public float bulletSpeed;
    private bool once = false;
    private ControllerHelper ch;

    override public void TriggerDown()
    {
        if (!once) {
            once = true;
            GameObject newBullet = Instantiate(bulletPrefab, barrelEnd.position, barrelEnd.rotation);
            Rigidbody bulletRB = newBullet.GetComponent<Rigidbody>();
            bulletRB.velocity = bulletRB.transform.right * bulletSpeed;
        }
    }

    override public void TriggerUp() {
        if (once) {
            once = false;
        }
    }

    public override void OnPickup() {
        ch = grabbedBy.gameObject.GetComponentInChildren<ControllerHelper>();
        ch.EnableMesh("Disabled");
    }

    public override void OnLetGo() {
        if (ch != null) ch.EnableMesh("Enabled");
        ch = null;
    }
}
