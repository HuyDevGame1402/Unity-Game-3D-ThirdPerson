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

    private void Update()
    {
        if (Input.GetButton("Fire2"))
        {
            AimCam.SetActive(true);
            AimCanvas.SetActive(true);
            ThirdPersonCam.SetActive(false);
            ThirdPersonCanvas.SetActive(false);
        }
        else
        {
            AimCam.SetActive(false);
            AimCanvas.SetActive(false);
            ThirdPersonCam.SetActive(true);
            ThirdPersonCanvas.SetActive(true);
        }
    }
}
