using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Light_Gun : Usable
{
    public GameObject bulletPrefab;
    public Transform barrelEnd;
    public float bulletSpeed;
    private bool once = false;
    private ControllerHelper ch;
    private int shotsLeft = 3;
    [SerializeField] TextMeshPro shotLeftText;
    [SerializeField] AudioClip shot;

    [SerializeField] private Transform respawnPoint;
    [SerializeField] private LaserEmitter le;

   

    override public void TriggerDown()
    {
        if (!once && shotsLeft > 0) {
            SoundManager.Instance.Play(shot, .2f);
            once = true;
            GameObject newBullet = Instantiate(bulletPrefab, barrelEnd.position, barrelEnd.rotation);
            Rigidbody bulletRB = newBullet.GetComponent<Rigidbody>();
            bulletRB.velocity = bulletRB.transform.right * bulletSpeed;
            shotsLeft -= 1;
            shotLeftText.text = shotsLeft.ToString();
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
        le.EnableLaser(true);
    }

    public override void OnLetGo() {
        if (ch != null) ch.EnableMesh("Enabled");
        ch = null;

        le.EnableLaser(false);
    }

    public void Reload() {
        shotsLeft = 3;
        shotLeftText.text = shotsLeft.ToString();
    }
    private void LateUpdate() {
        if (!grabbed) {
            if (transform.position.y <= .2f) {
                transform.position = respawnPoint.position;
                transform.rotation = respawnPoint.rotation;
            }
        }
    }
}
