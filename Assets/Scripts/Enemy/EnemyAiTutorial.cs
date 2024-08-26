
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAiTutorial : MonoBehaviour
{
    [Header("Kick Setting")]

    [SerializeField] private float kickPower;
    [SerializeField] private float kickTimer;
    [SerializeField] private bool isAttacked;

    [Space]

    [Header("Snake")]
    [SerializeField] private bool isSnake;
    [SerializeField] private GameObject fireInScene;
    [SerializeField] private GameObject foodBone;
    [SerializeField] private float timeToEat;
    private float eatTimer;
    [Space]

    [SerializeField] private float chasePlayerTimer;
    private float chaseTimer;

    public NavMeshAgent agent;

    public Transform player;

    public GameObject playerObject;

    public LayerMask whatIsGround, whatIsPlayer, whatIsFire;

    public float health;

    public Transform lamp;

    public GameObject col;

    [SerializeField] Animator anim;

    


    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    

    //States
    public float sightRange, attackRange, fireAttackRange;
    public bool playerInSightRange, playerInAttackRange, fireInSightRange, fireInAttackRange;
    private Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        FindLamp();

    }

    private void Update()
    {
        if(lamp == null || lamp.gameObject.activeSelf == false)
        {
            Debug.Log("1");
            lamp = FindLamp();
        }
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        fireInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsFire);
        fireInAttackRange =Physics.CheckSphere(transform.position, fireAttackRange, whatIsFire);


        if (isAttacked)
        {

            if (playerInSightRange && !playerInAttackRange) ChasePlayer();
            if (playerInAttackRange && playerInSightRange) Attack();
            chaseTimer += Time.deltaTime;
            eatTimer = 0;
            if(chaseTimer >= chasePlayerTimer)
            {
                isAttacked = false;
            }
        }
        else
        {
            if (isSnake)
            {
                Debug.Log(fireInSightRange);
                Debug.Log(fireInAttackRange);
                fireInScene = GameObject.FindGameObjectWithTag("Fire");
                if(fireInScene == null)
                {
                    ChaseLamp();
                }
                if(fireInSightRange && !fireInAttackRange)
                {
                    Debug.Log("я работаю");
                    ChaseFire();
                    anim.SetBool("Eat", false);

                }
                if(fireInSightRange && fireInAttackRange)
                {
                    fireEating();
                    //fireInScene.transform.SetParent(foodBone.transform);
                    //fireInScene.transform.position = new Vector3(0, 0, 0);
                    eatTimer += Time.deltaTime;
                    if (eatTimer >= timeToEat)
                    {
                        Destroy(fireInScene);
                        eatTimer = 0;
                    }
                    anim.SetBool("Eat",true);
                }
            }
            else
            {
                if (playerInSightRange && !playerInAttackRange) ChaseLamp();
                if (playerInAttackRange && playerInSightRange) Attack();
            }
        }


    }
    private Transform FindLamp()
    {
        lamp = GameObject.FindGameObjectWithTag("Lamp").transform;
        if(lamp.parent == null && lamp.gameObject.activeSelf == true)
        {
            return lamp;
        }
        if(lamp.parent.gameObject.tag == "Player" && lamp.gameObject.activeSelf == true)
        {
            return lamp;
        }
        return null;
    }
    private void ChaseFire()
    {
        agent.SetDestination(fireInScene.transform.position);
    }
    private void ChaseLamp()
    {
        agent.SetDestination(lamp.position);
    }
    private void ChasePlayer()
    {
        agent.SetDestination(playerObject.transform.position);
    }

    private void Attack()
    {
        agent.SetDestination(transform.position);
    }
    private void fireEating()
    {
        agent.SetDestination(transform.position);
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage, Transform point, GameObject player)
    {
        playerObject = player;
        isAttacked = true;
        chaseTimer = 0;
        health -= damage;
        StartCoroutine(Kick(point));
        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }
    private void kick(Transform point)
    {
        agent.enabled = false;
        rb.isKinematic = false;
        rb.AddForce((transform.position - point.position) * 100);
    }
    private IEnumerator Kick(Transform point)
    {
        agent.enabled = false;
        rb.isKinematic = false;
        rb.AddForce((transform.position - point.position) * kickPower);
        yield return new WaitForSeconds(kickTimer);
        rb.velocity = Vector3.zero;
        agent.enabled = true;
        rb.isKinematic = true;
        yield return null;
    }
    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
