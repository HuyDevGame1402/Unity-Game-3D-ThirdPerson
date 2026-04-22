using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCamera : MonoBehaviour
{
    [Header("Camera to Assign")]
    public GameObject AimCam;
    public GameObject AimCanvas;
    public GameObject ThirdPersonCam;
    public GameObject ThirdPersonCanvas;

    [Header("Camera Animator")]
    public Animator animator;

    private void Update()
    {
        if (Input.GetButton("Fire2") && Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            animator.SetBool("Idle", false);
            animator.SetBool("IdleAim", true);
            animator.SetBool("RifleWalk", true);
            animator.SetBool("Walk", true);

            AimCam.SetActive(true);
            AimCanvas.SetActive(true);
            ThirdPersonCam.SetActive(false);
            ThirdPersonCanvas.SetActive(false);
        }
        else if (Input.GetButton("Fire2"))
        {
            animator.SetBool("Idle", false);
            animator.SetBool("IdleAim", true);
            animator.SetBool("RifleWalk", false);
            animator.SetBool("Walk", false);

            AimCam.SetActive(true);
            AimCanvas.SetActive(true);
            ThirdPersonCam.SetActive(false);
            ThirdPersonCanvas.SetActive(false);
        }
        else
        {
            animator.SetBool("Idle", true);
            animator.SetBool("IdleAim", false);
            animator.SetBool("RifleWalk", false);

            AimCam.SetActive(false);
            AimCanvas.SetActive(false);
            ThirdPersonCam.SetActive(true);
            ThirdPersonCanvas.SetActive(true);
        }
    }
}
