using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : Target
{

    public float upTime;

    public Animator anim;
    private MeshRenderer mr;
    private MeshRenderer childMr;
    private Collider col;
    private bool risen;

    private void Awake()
    {
        anim = transform.GetComponent<Animator>();
        mr = transform.GetComponent<MeshRenderer>();
        childMr = transform.GetChild(0).GetComponent<MeshRenderer>();
        col = transform.GetComponent<Collider>();

        EnableDog();
    }

    
    public override void OnHit()
    {
        mr.enabled = false;
        childMr.enabled = true;
        GameManager.instance.GameOver();
        StartCoroutine(DestroyObject());
    }
    

        
    protected override IEnumerator DestroyObject()
    {
        ToggleDog(false);
        yield return null;
    }
    

    public void EnableDog()
    {
        if (risen)
        {
            mr.enabled = true;
            col.enabled = true;
        }
        else
        {
            mr.enabled = false;
            col.enabled = false;
        }

        childMr.enabled = false;
    }

    private void ToggleDog(bool toggle)
    {
        risen = toggle;
        anim.SetBool("Show Dog", toggle);
    }

    public IEnumerator ShowDog()
    {
        ToggleDog(true);

        yield return new WaitForSeconds(upTime);

        ToggleDog(false);
    }
}
