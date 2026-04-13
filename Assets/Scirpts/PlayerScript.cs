using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [Header("Player Movement")]
    public float playerSpeed = 1.9f;

    [Header("Player Script Camera")]
    public Transform playerCamera;

    [Header("Player Animator and Gravity")]
    public CharacterController cC;

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

    private void Update()
    {
        PlayerMove();
    }

    private void PlayerMove()
    {
        // lấy input từ bàn phím
        horizontal_axis = Input.GetAxis("Horizontal");
        vertical_axis = Input.GetAxis("Vertical");

        // tính toán hướng di chuyển dựa trên input
        direction = new Vector3(horizontal_axis, 0, vertical_axis).normalized;

        // nếu có input
        if (direction.magnitude >= 0.1f)
        {
            // lấy góc quay của hg + thêm vào góc quay hiện tại của camera ra đc góc quay cần
            // ví dụ camera góc 90 ấn W => góc dir = 90 -> targetAngle = 90 + 90 = 180 => camera quay về hướng 180
            targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
            // tính toán angle cho mượt k quay ngay lập tức
            angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnCalmVelocity, turnCalmTime);
            // quay player về hướng angle
            transform.rotation = Quaternion.Euler(0, angle, 0);
            // chuyển hướng forward về hướng targetAngle để di chuyển player (hướng z thẳng )
            moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            cC.Move(moveDirection.normalized * playerSpeed * Time.deltaTime);
        }
    }
}
