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
    [Space]
    [SerializeField] private float playerSpeed = 5f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float controllerDeadZone = 0.1f;
    [SerializeField] private float gamepadRotateSmoothing = 1000f;

    public CharacterController controller;

    private Vector2 movement;
    private Vector2 aim;
    private bool isLeftTrigger;
    private bool isRightTrigger;

    private Vector3 playerVelocity;

    private PlayerControls playerControls;
    
    private PlayerInput playerInput;

    [SerializeField] GameObject swordRange;
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
        HandleInput();
        HandleMovement();
        HandleRotation();
        Attack();
        attackTimer += Time.deltaTime;
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }
    public void OnAim(InputAction.CallbackContext context)
    {
        aim = context.ReadValue<Vector2>();
    }
    void HandleInput()
    {

   
    }

    private void Attack()
    {
        if (isLeftTrigger)
        {
            if (attackTimer >= attackColdown)
            {
                StartCoroutine(attackCoroutines());
            }
        }
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
    private void OnRightTrigger()
    {
        Debug.Log("123");
    }
    private void Start()
    {
        Debug.Log((Input.GetJoystickNames().Length));
    }
}
