using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Usable : OVRGrabbable
{

    protected OVRInput.Controller m_controller;
    protected bool grabbed;

    private void Update() {

        if (grabbedBy != null && !grabbed) { 
            m_controller = grabbedBy.Controller;
            grabbed = true;
            OnPickup();
        } else  if (grabbedBy == null && grabbed){
            m_controller = OVRInput.Controller.None;
            grabbed = false;
            OnLetGo();
        }
        if (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, m_controller) > .1 && m_controller != OVRInput.Controller.None) {
            TriggerDown();
        } else if (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, m_controller) < .1 && m_controller != OVRInput.Controller.None) {
            TriggerUp();
        }
    }

    virtual public void TriggerDown() { }

    virtual public void TriggerUp() { }

    virtual public void OnPickup() { }

    virtual public void OnLetGo() { }


}
