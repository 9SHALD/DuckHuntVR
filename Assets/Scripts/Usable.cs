using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Usable : OVRGrabbable
{

    protected OVRInput.Controller m_controller;

    private void Update() {

        if (grabbedBy != null) {
            m_controller = grabbedBy.Controller;
            OnPickup();
        } else {
            m_controller = OVRInput.Controller.None;
            OnLetGo();
        }
        if (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, m_controller) > .1 && m_controller != OVRInput.Controller.None) {
            TriggerDown();
        } else {
            TriggerUp();
        }
        
    }

    virtual public void TriggerDown() { }

    virtual public void TriggerUp() { }

    virtual public void OnPickup() { }

    virtual public void OnLetGo() { }


}
