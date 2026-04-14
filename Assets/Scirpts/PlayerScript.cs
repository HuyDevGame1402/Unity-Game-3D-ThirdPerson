using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [Header("Player Movement")]
    public float playerSpeed = 1.9f;
    public float playerSprint = 3.0f;

    [Header("Player Script Camera")]
    public Transform playerCamera;

    [Header("Player Animator and Gravity")]
    public CharacterController cC;
    public float gravity = -9.81f;
    public Animator animator;

    [Header("Input Value")]
    private float horizontal_axis;
    private float vertical_axis;
    private Vector3 direction; // hg di chuyển từ input
    private float targetAngle; // góc cần quay tới
    private float angle; // góc hiện tại 
    private Vector3 moveDirection; // hg di chuyển cuối cùng

    [Header("Player Jumping and velocity")]
    public float turnCalmTime = 0.1f;
    private float turnCalmVelocity;
    public float jumpRange = 1.0f;
    private Vector3 velocity;
    public Transform surfaceCheck;
    private bool onSurface;
    public float surfaceDistance = 0.4f;
    public LayerMask surfaceMask;

    private void Update()
    {
        onSurface = Physics.CheckSphere(surfaceCheck.position, 
            surfaceDistance, surfaceMask);
        if (onSurface && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        velocity.y += gravity * Time.deltaTime;
        cC.Move(velocity * Time.deltaTime);

        PlayerMove(playerSpeed, true);
        Jump();
        Sprint();
    }

    private void PlayerMove(float speed, bool isWalk)
    {
        // lấy input từ bàn phím
        horizontal_axis = Input.GetAxis("Horizontal");
        vertical_axis = Input.GetAxis("Vertical");

        // tính toán hướng di chuyển dựa trên input
        direction = new Vector3(horizontal_axis, 0, vertical_axis).normalized;

        // nếu có input
        if (direction.magnitude >= 0.1f)
        {
            if (isWalk)
            {
                SetAnimWalk();
            }
            else
            {
                SetAnimSprint();
            }

            // lấy góc quay của hg + thêm vào góc quay hiện tại của camera ra đc góc quay cần
            // ví dụ camera góc 90 ấn W => góc dir = 90 -> targetAngle = 90 + 90 = 180 => camera quay về hướng 180
            targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
            // tính toán angle cho mượt k quay ngay lập tức
            angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnCalmVelocity, turnCalmTime);
            // quay player về hướng angle
            transform.rotation = Quaternion.Euler(0, angle, 0);
            // chuyển hướng forward về hướng targetAngle để di chuyển player (hướng z thẳng )
            moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            cC.Move(moveDirection.normalized * speed * Time.deltaTime);
        }
        else
        {
            if (isWalk)
            {
                SetAnimIdle();
            }
            else
            {
                SetAnimIdleInSprint();
            }
        }
    }
    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && onSurface)
        {
            SetAnimJump();
            velocity.y = Mathf.Sqrt(jumpRange * -2f * gravity);
        }
        else
        {
            ResetAnimJump();
        }
    }

    private void Sprint()
    {
        if(Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W) ||
            Input.GetKey(KeyCode.UpArrow) && onSurface)
        {
            PlayerMove(playerSprint, false);
        }
    }
    private void SetAnimWalk()
    {
        animator.SetBool("Idle", false);
        animator.SetBool("Walk", true);
        animator.SetBool("Running", false);
        animator.SetBool("RifleWalk", false);
        animator.SetBool("IdleAim", false);
    }
    private void SetAnimIdle()
    {
        animator.SetBool("Idle", true);
        animator.SetBool("Walk", false);
        animator.SetBool("Running", false);
    }
    private void SetAnimIdleInSprint()
    {
        animator.SetBool("Walk", true);
        animator.SetBool("Running", false);
    }
    private void SetAnimSprint()
    {
        animator.SetBool("Walk", false);
        animator.SetBool("Running", true);
    }
    private void SetAnimJump()
    {
        animator.SetBool("Idle", false);
        animator.SetTrigger("Jump");
    }
    private void ResetAnimJump()
    {
        animator.SetBool("Idle", true);
        animator.ResetTrigger("Jump");
    }
}
