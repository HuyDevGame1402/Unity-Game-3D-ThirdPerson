using UnityEngine;
using UnityEngine.AI;

public class Zombie1 : MonoBehaviour
{
    [Header("Zombie Health and Damage")]
    public float giveDamage = 5f;

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
    float walkingPointRadius;

    [Header("Zombie Attacking Var")]
    public float timeBtwAttack;
    private bool previouslyAttack;


    [Header("Zombie mood/states")]
    public float vissionRadius;
    public float attackingRadius;
    public bool playerInvissionRadius;
    public bool playerInattackingRadius;

    public void Awake()
    {
        zombieAgent = GetComponent<NavMeshAgent>();
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
        zombieAgent.SetDestination(playerBody.position);
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

            }
            previouslyAttack = true;
            Invoke(nameof(ActiveAttacking), timeBtwAttack);
        }
    }
    private void ActiveAttacking()
    {
        previouslyAttack = false;
    }
}
