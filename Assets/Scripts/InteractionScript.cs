using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class InteractionScript : MonoBehaviour
{
    public  UnityEvent Event;

    private void Awake()
    {
        if (Event == null) Event = new UnityEvent();
    }
    
    public void DoAction() {
        Event.Invoke();
    }
}
