using UnityEngine;
using UnityEngine.AI;

public class Zombie2 : MonoBehaviour
{
    [Header("Zombie Health and Damage")]
    private float zombieHealth = 100f;
    private float presentHealth;
    public float giveDamage = 5f;

    [Header("Zombie Things")]
    public NavMeshAgent zombieAgent;
    public Transform lookPoint;
    public Camera attackingRaycastArea;
    public Transform playerBody;
    public LayerMask playerLayer;

    [Header("Zombie Standing Var")]
    public float zombieSpeed;

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
    }
    private void Update()
    {
        playerInvissionRadius = Physics.CheckSphere(transform.position, vissionRadius,
            playerLayer);
        playerInattackingRadius = Physics.CheckSphere(transform.position, attackingRadius,
            playerLayer);
        if (!playerInvissionRadius && !playerInattackingRadius)
        {
            Idle();
        }
        if (playerInvissionRadius && !playerInattackingRadius) Pursueplayer();
        if (playerInvissionRadius && playerInattackingRadius) AttackPlayer();
    }
    private void Idle()
    {
        zombieAgent.SetDestination(transform.position);
        anim.SetBool("Idle", true);
        anim.SetBool("Running", false);
    }
    private void Pursueplayer()
    {
        if (zombieAgent.SetDestination(playerBody.position))
        {
            anim.SetBool("Idle", false);
            anim.SetBool("Running", true);
            anim.SetBool("Attacking", false);
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
                if (playerBody != null)
                {
                    playerBody.PlayerHitDamage(giveDamage);
                }
                anim.SetBool("Running", false);
                anim.SetBool("Attacking", true);
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

        if (presentHealth <= 0)
        {
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
