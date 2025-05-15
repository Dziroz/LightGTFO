using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
//using UnityEngine.InputSystem.iOS;

public class PlayerController : MonoBehaviour
{
    [Header("Stamina")]
    [Space]
    [Header("Sound")]
    [SerializeField] AudioClip[] clips;
    [Space]
    
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
    [SerializeField] private bool isImmortal;
    [SerializeField] private float timeToSpawn;
    [SerializeField] private float respawnTimer;

    [Space]

    [Header("Attack Settings")]
    [SerializeField]private bool startCorAttack;
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

    [SerializeField] private float timeToTakeDamage;
    [SerializeField] private float timerToTakeDamage;
    [SerializeField] private new AudioSource audio;

    [Space]

    [Header("Color")]

    static float timeToStart;

    public Color[] colors;
    public static int colorsNumber;
    [Space]
    [SerializeField] private float playerSpeed = 5f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float controllerDeadZone = 0.1f;
    [SerializeField] private float gamepadRotateSmoothing = 1000f;
    [SerializeField] private bool PlayerTaking; // Игрок подбирает предмет

    private CharacterController controller;

    private Vector2 movement;
    private Vector2 aim;
    private bool isLeftTrigger;
    public bool isRightTrigger;
    public bool isPressB;
    private Vector3 playerVelocity;
    [SerializeField]private GameObject circle;
    [SerializeField] private GameObject color;
    [SerializeField]


    private PlayerControls playerControls;
    
    private PlayerInput playerInput;

    

    private FireManager fireManager;
    [SerializeField]private Collider col;
    [SerializeField]private Animator anim;
    private void Awake()

    {
        controller = GetComponent<CharacterController>();
        playerControls = new PlayerControls();
        playerInput = GetComponent<PlayerInput>();
        fireManager = GameObject.Find("FireManager").GetComponent<FireManager>();
        //SetColor();
       

    }
    private void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        controller = GetComponent<CharacterController>();
        playerControls = new PlayerControls();
        playerInput = GetComponent<PlayerInput>();
        fireManager = GameObject.Find("FireManager").GetComponent<FireManager>();
        SetColor();
        playerControls.Enable();
    }
    private void OnDisable()
    {
        playerControls.Disable();
    }
    void Update()
    {
        StartGame();
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
            if(respawnTimer >= timeToSpawn)
            {
                if(fireManager.firePower <= 0)
                {

                }
                else
                {
                    Rebirth();
                    respawnTimer = 0;
                }
            }
        }
        Yposition();
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
            Debug.Log("Zapusk");
            isRightTrigger = true;     
            if (lamp.activeSelf)
            {
               
                DropLamp();
            }
            else
            {
                TakeLamp();
                
            }
        }
        if (context.performed)
        {
            Debug.Log("performed"); StartGame();
        }
        if (context.canceled)
        {
            /*
            isRightTrigger = false;
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
            */
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
                if(startCorAttack == false)
                {
                    StartCoroutine(attackCoroutines());
                    audio.PlayOneShot(clips[0]);
                }
            }
        }
    }
    public void TakeLamp()
    {
        if (canTake && lampInPlayer == false)
        {
            if(lamp.activeSelf == false)
            {
                Destroy(lampInGame);
                lamp.SetActive(true);
                lampInPlayer = true;
                canTake = false;
                Debug.Log("Взял");
                audio.PlayOneShot(clips[1]);
            }
            //fireManager.DestroyLamp()
            //asd

        }
    }
    private void DropLamp()
    {
        if(lamp.activeSelf == true)
        {
            lampInPlayer = false;
            Instantiate(lampPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z + 1), Quaternion.identity);
            lamp.SetActive(false);
            Debug.Log("Бросил");
            audio.PlayOneShot(clips[2]);
            //fireManager.DestroyLamp();
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
            audio.PlayOneShot(clips[3]);
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
    public void StartGame()
    {
        //var slider = GameObject.Find("Slider");
        //var fill = slider.GetComponent<Slider>();
        if(GameManager.game == false)
        {
            if (isPressB)
            {
                //slider.transform.localScale = new Vector3(0.9f, 0.8f, 0);
                timeToStart += Time.deltaTime;
                if (timeToStart / 2 <= 2)
                {
                    //fill.value = timeToStart / 2;
                    Debug.Log(timeToStart);
                }
            }
            else
            {
                //slider.transform.localScale = new Vector3(0, 0, 0);

                timeToStart = 0;
                //fill.value = 0;
            }
        }
        else
        {
            //slider.transform.localScale = new Vector3(0, 0, 0);

            timeToStart = 0;
            //fill.value = 0;
        }

        if(timeToStart >= 2)
        {
            GameManager.game = true;
            
        }
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
        startCorAttack = true;
        swordRange.SetActive(true);
        
        yield return new WaitForSeconds(0.5f);       
        swordRange.SetActive(false);
        attackTimer = 0;
        startCorAttack = false;
        yield return null;
    }
    private void DeathOutLight()
    {
        GameObject Lamp = GameObject.FindGameObjectWithTag("Lamp");
        float distance = Vector3.Distance(Lamp.transform.position, this.transform.position);
        if(distance > Lamp.transform.GetChild(0).gameObject.GetComponent<Light>().range)
        {
            Debug.Log(gameObject.name + "Вне света");
            timerToTakeDamage += Time.deltaTime;
            if(timerToTakeDamage >= timeToTakeDamage)
            {
                TakeDamage();
                timerToTakeDamage = 0;
            }
        }
        else
        {
            //Debug.Log(gameObject.name + "Горит");
            timerToTakeDamage = 0;
        }
    }
    public void Death()
    {
        audio.PlayOneShot(clips[4]);
        DropLamp();
        alive = false;
    }
    public void Rebirth()
    {
        float x = fireManager.lamp.transform.position.z + 2;
        controller.enabled = false;
        transform.position = new Vector3(fireManager.lamp.transform.position.x, fireManager.lamp.transform.position.y, x);
        controller.enabled = true;
        hp = maxHp;
        alive = true;
        audio.PlayOneShot(clips[5]);
    }
    public void TakeDamage()
    {
        if (GameManager.gameHelth)
        {
            if(swordRange.activeSelf == false)
            {
                if (isImmortal)
                {

                }
                else
                {
                    StartCoroutine(Immortal());
                    hp--;
                    if (lamp.activeSelf == true)
                    {
                        fireManager.AttackLight();
                    }
                    if (hp <= 0)
                    {
                        Death();
                    }
                }
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
        //Debug.Log(movement);
        Vector3 move = new Vector3(movement.x, 0, movement.y);
        if (move != Vector3.zero)
        {
            anim.SetBool("isWalk", true);
        }
        else
        {
            anim.SetBool("isWalk", false);
        }
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
        Renderer.material.color = colors[colorsNumber];
        Renderer.material.SetColor("_EmissionColor", colors[colorsNumber]);
        colorsNumber++;
        if(colorsNumber> colors.Length)
        {
            colorsNumber = 0;
        }
    }
    public int getHP()
    {
        return hp;
    }
    void Yposition()
    {
        if(transform.position.y != 1.58f)
        {
            controller.enabled = false;
            transform.position = new Vector3(transform.position.x, 1.58f, transform.position.z);
            controller.enabled = true;
        }
    }
}
