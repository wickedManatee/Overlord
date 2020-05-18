using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class HandState : MonoBehaviour {
    public Hand hand;
    public Mesh rest;
    public Mesh primed;
    private Color originalColor;

	// Use this for initialization
	void Start () {
        originalColor = GetComponent<Renderer>().material.color;
       // hand.GetStandardGripButtonDown += GripGrabStart; 

    }
	
    void GripGrabStart()
    {
        GetComponent<Renderer>().material.color = Color.red;
    }

    void GripGrabEnd()
    {
        GetComponent<Renderer>().material.color = originalColor;
    }

    void GripTouchStart()
    {
        GetComponent<MeshFilter>().mesh = primed;
    }

    void GripTouchEnd()
    {
        //if (!gripPoint.HoldingSomething())
        {
            GetComponent<MeshFilter>().mesh = rest;
        }
    }
}
