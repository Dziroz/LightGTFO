using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.iOS;

public class PlayerController : MonoBehaviour
{

    [Header("Attack Settings")]

    
    [SerializeField] private float attackColdown;
    private float attackTimer;
    [SerializeField] GameObject swordRange;
    [Space]
    [Header("Lamp Settings")]
    [SerializeField] public bool isLamped;
    [SerializeField] public GameObject lampPrefab;
    [SerializeField] GameObject lamp;


    [SerializeField] private float playerSpeed = 5f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float controllerDeadZone = 0.1f;
    [SerializeField] private float gamepadRotateSmoothing = 1000f;

    private CharacterController controller;

    private Vector2 movement;
    private Vector2 aim;
    private bool isLeftTrigger;
    public bool isRightTrigger;

    private Vector3 playerVelocity;

    private PlayerControls playerControls;
    
    private PlayerInput playerInput;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerControls = new PlayerControls();
        playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }
    private void OnDisable()
    {
        playerControls.Disable();
    }
    void Update()
    {
        HandleMovement();
        HandleRotation();
        Attack();      
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }
    public void OnAim(InputAction.CallbackContext context)
    {
        aim = context.ReadValue<Vector2>();
    }
    public void OnLeftTrigger(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isLeftTrigger = true;
            Debug.Log(isLeftTrigger);
        }
        if (context.canceled)
        {
            isLeftTrigger = false;
            Debug.Log(isLeftTrigger);
        }
    }
    public void OnRightTrigger(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isRightTrigger = true;
            //DropLamp();


        }
        if (context.canceled)
        {
            isRightTrigger = false;
        }
    }
    private void Attack()
    {
        attackTimer += Time.deltaTime;
        if (isLeftTrigger)
        {
            if (attackTimer >= attackColdown)
            {
                StartCoroutine(attackCoroutines());
            }
        }
    }
    public void TakeLamp()
    {
        lamp.SetActive(true);
        isLamped = true;
    }
    private void DropLamp()
    {
        lamp.SetActive(false);
        isLamped = false;
        Instantiate(lampPrefab, new Vector3(transform.position.x,transform.position.y), Quaternion.identity);
    }

    private IEnumerator attackCoroutines()
    {
        swordRange.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        swordRange.SetActive(false);
        attackTimer = 0;
        yield return null;
    }
    void HandleMovement()
    {
        Vector3 move = new Vector3(movement.x, 0, movement.y);
        controller.Move(move * Time.deltaTime * playerSpeed);
    }
    void HandleRotation()
    {
        if(Mathf.Abs(aim.x) > controllerDeadZone || Mathf.Abs(aim.y) > controllerDeadZone)
        {
            Vector3 playerDirection = Vector3.right * -aim.y + Vector3.forward * aim.x;
            if(playerDirection.sqrMagnitude > 0.0f)
            {
                Quaternion newrotation = Quaternion.LookRotation(playerDirection, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, newrotation, gamepadRotateSmoothing * Time.deltaTime);
            }
        }
    }
}
