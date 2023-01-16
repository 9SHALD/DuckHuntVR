using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LaserEmitter : MonoBehaviour
{
	public float rayLength;

	private RaycastHit hit;
	private LineRenderer ln;

	private void Awake()
	{
		ln = GetComponent<LineRenderer>();
		ln.enabled = false;
	}

	private void Update()
	{

		if (Physics.Raycast(transform.position, transform.forward, out hit, rayLength))
		{
			ln.SetPosition(1 , new Vector3(0, 0, hit.transform.position.z));
		} else {
			ln.SetPosition(1, new Vector3(0,0, 31));
        }
	}

	public void EnableLaser(bool enabled) {
		ln.enabled = enabled;
    }
}