using UnityEngine;
using UnityEngine.AI;

public class Zombie1 : MonoBehaviour
{
    [Header("Zombie Health and Damage")]
    private float zombieHealth = 100f;
    private float presentHealth;
    public float giveDamage = 5f;
    public HealthBar healthBar;

    [Header("Zombie Things")]
    public NavMeshAgent zombieAgent;
    public Transform lookPoint;
    public Camera attackingRaycastArea;
    public Transform playerBody;
    public LayerMask playerLayer;

    [Header("Zombie Guarding Var")]
    public GameObject[] walkPoint;
    int currentZombiePosition = 0;
    public float zombieSpeed;
    float walkingPointRadius = 2;

    [Header("Zombie Attacking Var")]
    public float timeBtwAttack;
    private bool previouslyAttack;

    [Header("Zombie Animation")]
    public Animator anim;


    [Header("Zombie mood/states")]
    public float vissionRadius;
    public float attackingRadius;
    public bool playerInvissionRadius;
    public bool playerInattackingRadius;

    public void Awake()
    {
        presentHealth = zombieHealth;
        zombieAgent = GetComponent<NavMeshAgent>();
        healthBar.GiveFullHealth(zombieHealth);
    }
    private void Update()
    {
        playerInvissionRadius = Physics.CheckSphere(transform.position, vissionRadius,
            playerLayer);
        playerInattackingRadius = Physics.CheckSphere(transform.position, attackingRadius,
            playerLayer);
        if(!playerInvissionRadius && !playerInattackingRadius)
        {
            Guard();
        }
        if (playerInvissionRadius && !playerInattackingRadius) Pursueplayer();
        if (playerInvissionRadius && playerInattackingRadius) AttackPlayer();
    }
    private void Guard()
    {
        if (Vector3.Distance(walkPoint[currentZombiePosition].transform.position,
            transform.position) < walkingPointRadius)
        {
            currentZombiePosition = Random.Range(0, walkPoint.Length);
            if(currentZombiePosition >= walkPoint.Length)
            {
                currentZombiePosition = 0;
            }
        }
        transform.position = Vector3.MoveTowards(transform.position,
            walkPoint[currentZombiePosition].transform.position, Time.deltaTime * zombieSpeed);
        transform.LookAt(walkPoint[currentZombiePosition].transform.position);
    }
    private void Pursueplayer()
    {
        if (zombieAgent.SetDestination(playerBody.position))
        {
            anim.SetBool("Walking", false);
            anim.SetBool("Running", true);
            anim.SetBool("Attacking", false);
            anim.SetBool("Died", false);
        }
        else
        {
            anim.SetBool("Walking", false);
            anim.SetBool("Running", false);
            anim.SetBool("Attacking", false);
            anim.SetBool("Died", true);
        }
    }
    private void AttackPlayer()
    {
        zombieAgent.SetDestination(transform.position);
        transform.LookAt(lookPoint);
        if (!previouslyAttack)
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(attackingRaycastArea.transform.position,
                attackingRaycastArea.transform.forward, out hitInfo, attackingRadius))
            {
                Debug.Log("Attack");
                PlayerScript playerBody = hitInfo.transform.GetComponent<PlayerScript>();
                if(playerBody != null)
                {
                    playerBody.PlayerHitDamage(giveDamage);
                }
                anim.SetBool("Walking", true);
                anim.SetBool("Running", false);
                anim.SetBool("Attacking", false);
                anim.SetBool("Died", false);
            }
            previouslyAttack = true;
            Invoke(nameof(ActiveAttacking), timeBtwAttack);
        }
    }
    private void ActiveAttacking()
    {
        previouslyAttack = false;
    }
    public void ZombieHitDamage(float takeDamage)
    {
        presentHealth -= takeDamage;
        healthBar.SetHealth(presentHealth);
        if(presentHealth <= 0)
        {
            anim.SetBool("Walking", false);
            anim.SetBool("Running", false);
            anim.SetBool("Attacking", false);
            anim.SetBool("Died", true);
            ZombieDie();
        }
    }

    private void ZombieDie()
    {
        zombieAgent.SetDestination(transform.position);
        zombieSpeed = 0;
        attackingRadius = 0f;
        vissionRadius = 0f;
        playerInattackingRadius = false;
        playerInvissionRadius = false;
        Object.Destroy(gameObject, 5.0f);
    }
}
