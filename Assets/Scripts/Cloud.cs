using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{

    private float speed;
    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(0.3f, 0.6f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition -= new Vector3(speed * Time.deltaTime,0,0);
        if(transform.localPosition.x <= -25) {
            transform.localPosition = new Vector3(25, transform.localPosition.y, transform.localPosition.z);
        }
    }
}
