using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrbit : MonoBehaviour {
    public Transform target;
    public float walkDistance;
    public float runDistance;
    public float height;

    Transform _myTransform;

    void Start () {
        if (target == null)
            Debug.LogWarning("Target not set on " + transform.name);

        _myTransform = transform;
        
	}
	
	void Update () {
		
	}

    void LateUpdate()
    {
        _myTransform.position = new Vector3(target.position.x, target.position.y + height, target.position.z - walkDistance);    
    }
}
