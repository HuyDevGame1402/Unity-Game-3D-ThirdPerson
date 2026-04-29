using UnityEngine;
using UnityEngine.AI;

public class Zombie1 : MonoBehaviour
{
    [Header("Zombie Health and Damage")]
    private float zombieHealth = 100f; // máu zombie
    private float presentHealth; // máu tiện tại
    public float giveDamage = 5f; // dame gây ra
    public HealthBar healthBar; // thanh healthbar ui máu của zombie

    [Header("Zombie Things")]
    // dùng để di chuyển zombie đến vị trí của player khi player vào trong tầm nhìn của zombie
    // AI giống pathfinding
    public NavMeshAgent zombieAgent;
    // điểm nhìn của zombie khi tấn công player
    public Transform lookPoint;
    // camera point để bắn raycast khi tấn công
    public Camera attackingRaycastArea;
    // transform của player để zombie có thể di chuyển đến vị trí của player khi player vào trong tầm nhìn của zombie
    public Transform playerBody;
    // layer player
    public LayerMask playerLayer;

    [Header("Zombie Guarding Var")]
    // các điểm zombie đi tuần tra
    public GameObject[] walkPoint;
    // điểm hiện tại (chỉ số vị trí mảng hiện tại)
    int currentZombiePosition = 0;
    public float zombieSpeed; // tốc độ
    float walkingPointRadius = 2; // khoảng cách tới điểm

    [Header("Zombie Attacking Var")]
    // delay giữa các lần tấn công
    public float timeBtwAttack;
    // tránh spawn attack liên tục
    private bool previouslyAttack;

    [Header("Zombie Animation")]
    public Animator anim; // anim zombie


    [Header("Zombie mood/states")]
    // bán kính phát hiện player
    public float vissionRadius; 
    // bán kính tấn công player
    public float attackingRadius;
    // check trạng thái player có trong bán kính phát hiện hay không
    public bool playerInvissionRadius;
    public bool playerInattackingRadius;

    public void Awake()
    {
        // máu hiện tại = máu gốc
        presentHealth = zombieHealth;
        // lấy mesh agent của zombie để di chuyển
        zombieAgent = GetComponent<NavMeshAgent>();
        // set máu đầy cho healthbar
        healthBar.GiveFullHealth(zombieHealth);
    }
    private void Update()
    {
        // sử dụng hàm CheckSphere để kiểm tra xem player có trong bán kính phát hiện hay không
        // hàm checksphere sẽ tạo ra một hình cầu tại vị trí của zombie với bán kính là
        // vissionRadius và kiểm tra xem có vật thể nào thuộc playerLayer nằm trong hình cầu đó hay không
        playerInvissionRadius = Physics.CheckSphere(transform.position, vissionRadius,
            playerLayer);
        playerInattackingRadius = Physics.CheckSphere(transform.position, attackingRadius,
            playerLayer);
        if(!playerInvissionRadius && !playerInattackingRadius)
        {
            // không thấy player nào cả thì gọi Guard
            Guard();
        }
        // thấy nhg chưa trong tầm đánh => đuổi theo player
        if (playerInvissionRadius && !playerInattackingRadius) Pursueplayer();
        // thấy và trong tầm đánh => đánh
        if (playerInvissionRadius && playerInattackingRadius) AttackPlayer();
    }
    private void Guard()
    {
        // nếu zombie đến gần điểm tuần hiện tại với khoảng cách < radius
        if (Vector3.Distance(walkPoint[currentZombiePosition].transform.position,
            transform.position) < walkingPointRadius)
        {
            // thực hiện if bên trong
            // random điểm tới ms trong mảng điểm tuần tra
            currentZombiePosition = Random.Range(0, walkPoint.Length);
            // lớn hơn thì trả về 0 để tránh lỗi index out of range
            if(currentZombiePosition >= walkPoint.Length)
            {
                currentZombiePosition = 0;
            }
        }
        // sử dụng hàm MoveTowards để di chuyển từ vị trí hiện tại
        // tới mục tiêu mỗi lần đi 1 khoảng Time.deltaTime * zombieSpeed
        // thường là gọi 1 frame 1 lần 
        // như trong update gọi di chuyển thủ công
        transform.position = Vector3.MoveTowards(transform.position,
            walkPoint[currentZombiePosition].transform.position, Time.deltaTime * zombieSpeed);
        // luôn nhìn về điểm mục tiêu tuần tra
        transform.LookAt(walkPoint[currentZombiePosition].transform.position);
    }
    private void Pursueplayer()
    {
        // sử dụng hàm SetDestination của NavMeshAgent để di chuyển zombie đến vị trí của player
        // thực hiện lệnh di chuyển từ transform.position tới target player
        // nếu đúng thì sẽ di chuyển và trả về true
        // thì sẽ thực hiện anim chạy
        if (zombieAgent.SetDestination(playerBody.position))
        {
            anim.SetBool("Walking", false);
            anim.SetBool("Running", true);
            anim.SetBool("Attacking", false);
            anim.SetBool("Died", false);
        }
        else
        {
            // nếu k setdestination đc thì zombie chết
            // logic hơi sai để chết hoặc để false hết cũng đc
            anim.SetBool("Walking", false);
            anim.SetBool("Running", false);
            anim.SetBool("Attacking", false);
            anim.SetBool("Died", true);
        }
    }
    private void AttackPlayer()
    {
        // khi zombie tấn công thì setdestination tại vị trí của nó để dừng di chuyển
        zombieAgent.SetDestination(transform.position);
        transform.LookAt(lookPoint); // quay về phía mục tiêu thường là đầu player
        if (!previouslyAttack) // phải đc phép đánh ms đánh tranh spawn
        {
            RaycastHit hitInfo;
            // bắn raycast từ vị trí của attackingRaycastArea theo hướng forward của nó với khoảng cách là attackingRadius
            if (Physics.Raycast(attackingRaycastArea.transform.position,
                attackingRaycastArea.transform.forward, out hitInfo, attackingRadius))
            {
                Debug.Log("Attack");
                PlayerScript playerBody = hitInfo.transform.GetComponent<PlayerScript>();
                if(playerBody != null)
                {
                    // gây dame player
                    playerBody.PlayerHitDamage(giveDamage);
                }
                // thực hiện anim tấn công
                anim.SetBool("Walking", false);
                anim.SetBool("Running", false);
                anim.SetBool("Attacking", true);
                anim.SetBool("Died", false);
            }
            // đặt là true để delay mỗi lần đánh
            previouslyAttack = true;
            // invoke gọi hàm để reset trạng thái đánh sau khoảng timeBtwAttack
            Invoke(nameof(ActiveAttacking), timeBtwAttack);
        }
    }
    // reset lại tấn công
    private void ActiveAttacking()
    {
        previouslyAttack = false;
    }
    // zombie hit dame
    public void ZombieHitDamage(float takeDamage)
    {
        // trừ máu
        presentHealth -= takeDamage;
        // set healthbar máu hiện tại
        healthBar.SetHealth(presentHealth);
        // kiểm tra máu có <= 0
        if(presentHealth <= 0)
        {
            // đúng thì gọi anim chết
            anim.SetBool("Walking", false);
            anim.SetBool("Running", false);
            anim.SetBool("Attacking", false);
            anim.SetBool("Died", true);
            // gọi hàm die cho zombie
            ZombieDie();
        }
    }

    private void ZombieDie()
    {
        // setdestination tại vị trí của zombie để dừng di chuyển
        zombieAgent.SetDestination(transform.position);
        zombieSpeed = 0; // đặt tốc độ = 0
        attackingRadius = 0f; // kc tc = 0
        vissionRadius = 0f; // phạm vi phát hiện = 0
        playerInattackingRadius = false; // đặt là false vì chết k bắt đc player
        playerInvissionRadius = false; // tương tự là false k bắt đc player trong pvi tấn công
        Object.Destroy(gameObject, 5.0f); // destroy zombie sau 5s
    }
}
