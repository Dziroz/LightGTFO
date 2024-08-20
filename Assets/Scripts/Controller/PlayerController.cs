using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.iOS;

public class PlayerController : MonoBehaviour
{
    [Header("Stamina")]
    [SerializeField] private float stamina;
    [SerializeField] private float maxStamina;
    [SerializeField] private float startSlow;
    [SerializeField] private float lowSpeed;
    [SerializeField] private float maxSpeed;
    [Space]

    [Header("Hp")]
    [SerializeField] public int maxHp;
    [SerializeField] public int hp;
    [SerializeField] public bool alive;
    [SerializeField] public float immortalTime;
    [SerializeField]private bool isImmortal;
    private float respawnTimer;

    [Space]

    [Header("Attack Settings")]
    [SerializeField] private float attackColdown;
    private float attackTimer;
    [SerializeField] GameObject swordRange;
    [SerializeField] private int ax;

    [Space]

    [Header("Lamp Settings")]
    [SerializeField] public bool canTake;
    [SerializeField] public GameObject lampPrefab;
    [SerializeField] public GameObject lamp;
    [SerializeField] public GameObject lampInGame;
    [SerializeField] private float takeRange;
    [SerializeField] private LayerMask lampMask;
    static public bool lampInPlayer;

    [Space]

    [Header("Fire Setting")]
    [SerializeField] public bool fireCanTake;
    [SerializeField] private float fireTimer;
    [SerializeField] public GameObject thisFireGameObject;
    [SerializeField] private float timeForTakeFire;

    [Space]

    [SerializeField] private float playerSpeed = 5f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float controllerDeadZone = 0.1f;
    [SerializeField] private float gamepadRotateSmoothing = 1000f;
    [SerializeField] private bool PlayerTaking; // ����� ��������� �������

    private CharacterController controller;

    private Vector2 movement;
    private Vector2 aim;
    private bool isLeftTrigger;
    public bool isRightTrigger;
    private bool isPressB;
    private Vector3 playerVelocity;
    [SerializeField]private GameObject circle;
    [SerializeField] private GameObject color;
    [SerializeField]


    private PlayerControls playerControls;
    
    private PlayerInput playerInput;

    private FireManager fireManager;
    [SerializeField]private Collider col;
    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerControls = new PlayerControls();
        playerInput = GetComponent<PlayerInput>();
        fireManager = GameObject.Find("FireManager").GetComponent<FireManager>();
        
    }
    private void Start()
    {
        SetColor();
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
        //canTake = Physics.CheckSphere(transform.position, takeRange, lampMask);
        //lampInGame = Physics.CheckSphere(transform.position, takeRange, lampMask);
        if (alive)
        {
            if (PlayerTaking == false)
            {
                HandleMovement();
                HandleRotation();
                Attack();
            }
            takeFire();
            StaminaController();
            DeathOutLight();

        }
        else
        {
            respawnTimer += Time.deltaTime;
            if(respawnTimer >= 10)
            {
                Rebirth();
                respawnTimer = 0;
            }
        }
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
            if (lamp.activeSelf)
            {
                Debug.Log(1);
                DropLamp();
            }
            else
            {
                TakeLamp();
                Debug.Log(2);
            }

        }
        if (context.canceled)
        {
            isRightTrigger = false;
        }
    }
    public void OnPressB(InputAction.CallbackContext contex)
    {
        if (contex.started)
        {
            isPressB = true;
        }
        if (contex.canceled)
        {
            isPressB = false;
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
        if (canTake && lampInPlayer == false)
        {
            lamp.SetActive(true);
            Destroy(lampInGame);
            canTake = false;
            lampInPlayer = true;
        }
    }
    private void DropLamp()
    {
        Debug.Log("������");
        if(lamp.activeSelf == true)
        {
            lamp.SetActive(false);
            lampInPlayer = false;
            Instantiate(lampPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        }       
    }
    private void takeFire()
    {
        if (isPressB && fireCanTake)
        {
            PlayerTaking = true;
            fireTimer += Time.deltaTime;
        }
        else
        {
            PlayerTaking = false;
            ResetFireTimer();

        }
        if (fireTimer >= timeForTakeFire)
        {
            fireManager.AddPower();
            Destroy(thisFireGameObject);
            fireCanTake = false;
            fireTimer = 0;
        }
        
    }
    public void ResetFireTimer()
    {
        fireTimer = 0;
    }
    private void StaminaController()
    {
        if (lamp.activeSelf)
        {
            if (stamina >=0)
            {
                stamina -= Time.deltaTime;
            }
            if (stamina < startSlow && stamina >=0)
            {
                if (playerSpeed > lowSpeed)
                {
                    playerSpeed -= Time.deltaTime;
                }
            }
        }
        else
        {
            if (stamina <= maxStamina)
            {
                stamina += Time.deltaTime;
            }
            if(playerSpeed <= maxSpeed)
            {
                playerSpeed += Time.deltaTime;
            }
           
        }
        if (stamina <= 0)
        {
            circle.SetActive(true);
            DropLamp();
        }
        if(stamina >= maxStamina)
        {
            circle.SetActive(false);
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
    private void DeathOutLight()
    {
        GameObject Lamp = GameObject.FindGameObjectWithTag("Lamp");
        float distance = Vector3.Distance(Lamp.transform.position, this.transform.position);
        if(distance > Lamp.transform.GetChild(0).gameObject.GetComponent<Light>().range)
        {
            Debug.Log(gameObject.name + "��� �����");
        }
        else
        {
            Debug.Log(gameObject.name + "�����");
        }
    }
    public void Death()
    {
        alive = false;
    }
    public void Rebirth()
    {
        transform.position = fireManager.lamp.transform.position;
        alive = true;
    }
    public void TakeDamage()
    {
        if (isImmortal)
        {

        }
        else
        {
            StartCoroutine(Immortal());
            hp--;
            if(lamp.activeSelf == true)
            {
                fireManager.AttackLight();
            }
        }
    }
    private IEnumerator Immortal()
    {
        isImmortal = true;

        yield return new WaitForSeconds(immortalTime);

        isImmortal = false;

        yield return null;
    }
    public void TakeHeal()
    {
        if (hp < maxHp)
        {
            hp++;
        }
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
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Enemy")
        {
            TakeDamage();
        }

    }
    void SetColor()
    {
        
        var Renderer = color.GetComponent<Renderer>();
        Renderer.material.SetColor("_Color", new Color(UnityEngine.Random.Range(0,1), UnityEngine.Random.Range(0, 1), UnityEngine.Random.Range(0, 1), 1f));
    }
}
