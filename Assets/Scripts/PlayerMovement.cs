using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour {
    public float walkSpeed = 5;
    public float runMultiplier = 2;
    public float strafeSpeed = 2.5f;
    public float rotateSpeed = 250;
    public float gravity = 20f;


    CollisionFlags _collisionFlags;
    Vector3 _moveDir;
    Transform _transform;
    CharacterController _controller;

    private void Awake()
    {
        _transform = transform;
        _controller = GetComponent<CharacterController>();
        _moveDir = Vector3.zero;
    }
    void Start () {
		
	}

    void Update () {
        if (_controller.isGrounded)
        {
            _moveDir = new Vector3(0f, 0f, Input.GetAxis("Vertical"));
            _moveDir = _transform.TransformDirection(_moveDir).normalized;
            _moveDir *= walkSpeed;
        }
        else
        {
            Debug.Log("Not on gorund");
            if ((_collisionFlags & CollisionFlags.CollidedBelow) == 0)
            {

            }
        }

        _moveDir.y -= gravity * Time.deltaTime;
        _collisionFlags = _controller.Move(_moveDir * Time.deltaTime);
	}
}
