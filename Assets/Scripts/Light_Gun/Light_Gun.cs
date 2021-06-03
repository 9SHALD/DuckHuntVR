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
    private int shotsLeft = 3;

    override public void TriggerDown()
    {
        if (!once && shotsLeft > 0) {
            once = true;
            GameObject newBullet = Instantiate(bulletPrefab, barrelEnd.position, barrelEnd.rotation);
            Rigidbody bulletRB = newBullet.GetComponent<Rigidbody>();
            bulletRB.velocity = bulletRB.transform.right * bulletSpeed;
            shotsLeft -= 1;
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

    public void Reload() {
        shotsLeft = 3;
    }
}
