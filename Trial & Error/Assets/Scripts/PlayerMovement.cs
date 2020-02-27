﻿using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Transform _mainCameraRotation;
    private Vector3 startingPosition;
    private int collectiblesCollected = 0;
    public bool unlocked = false;

    [SerializeField]
    private float playerSpeed = 1f;
    [SerializeField, Tooltip("Max speed the player can move at")]
    private float maxPlayerSpeed = 8f;
    private Vector2 _movementDir = Vector2.zero;
    
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _mainCameraRotation = FindObjectOfType<CinemachineBrain>().transform;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        startingPosition = transform.position;
    }

    private void FixedUpdate()
    {
        MovePlayer();
        BackToStart();
    }

    void MovePlayer()
    {
        Vector3 velocity = _rigidbody.velocity;
        var camRot = _mainCameraRotation.rotation;
        camRot.eulerAngles = new Vector3(0, camRot.eulerAngles.y, 0);
        velocity += camRot * new Vector3(_movementDir.x, 0, _movementDir.y) * playerSpeed;
        if (!unlocked)
        {
            velocity = Vector3.ClampMagnitude(velocity, maxPlayerSpeed);
        }
        _rigidbody.velocity = velocity;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _movementDir = context.performed ? context.ReadValue<Vector2>() : Vector2.zero;
    }

    void BackToStart()
    {
        if (Input.GetKey(KeyCode.Backspace))
        {
            gameObject.transform.position = startingPosition;
        }
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Collectible")
        {
            collectiblesCollected++;
            Debug.Log("Got it!");
            GameObject collectionSphere = other.gameObject;
            collectionSphere.SetActive(false);
        }
    }
}
