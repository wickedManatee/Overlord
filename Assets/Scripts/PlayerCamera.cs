using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public GameObject player;
    public Transform followTarget;

    public float camDistance;
    public float height;
    public float xSpeed = 250;
    public float ySpeed = 120;
    public float heightDamping = 2f;
    public float rotationDamping = 3f;

    Transform _myTrans;
    float x;
    float y;
    bool _aimButtonPressed;

    private void Awake()
    {
        _myTrans = transform;
        _aimButtonPressed = false;
    }

    // Use this for initialization
    void Start()
    {
        if (followTarget == null || player == null)
            Debug.LogError("Gameobject not fully formed - " + this.name);
        else
        {
            _myTrans.position = new Vector3(followTarget.position.x, followTarget.position.y + height, followTarget.position.z - camDistance);
            _myTrans.LookAt(followTarget);
        }

    }

    // Update is called once per frame
    void LateUpdate()
    {
        #region Code to rotate the camera with the player movement
        /* 
        {
            float wantedRotationAngle = target.eulerAngles.y;
            float wantedHeight = target.position.y + height;

            float currentRotationAngle = _myTrans.eulerAngles.y;
            float currentHeight = _myTrans.position.y;

            currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);
            currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

            Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

            _myTrans.position = target.position;
            _myTrans.position -= currentRotation * Vector3.forward * walkDistance;
            _myTrans.position = new Vector3(_myTrans.position.x, currentHeight, _myTrans.position.z);

            _myTrans.LookAt(target);
        }*/
    #endregion

        x += Input.GetAxis("Mouse X") * xSpeed * .02f;
        y -= Input.GetAxis("Mouse Y") * ySpeed * .02f;

        Quaternion rotation = Quaternion.Euler(y, x, 0);
        Vector3 position; 

        if (_aimButtonPressed)
            position = rotation * new Vector3(0.0f, 0.0f, - (camDistance / 4)) + followTarget.position;
        else
            position = rotation * new Vector3(0.0f, 0.0f, - camDistance) + followTarget.position;

        _myTrans.rotation = rotation;
        _myTrans.position = position;
    }

    public void SetAimming(bool state)
    {
        _aimButtonPressed = state;
    }
    
}
