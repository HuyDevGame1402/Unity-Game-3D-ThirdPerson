using UnityEngine;

public class RotateHealthBarUI : MonoBehaviour
{
    public Transform mainCamera;

    public void LateUpdate()
    {
        transform.LookAt(transform.position + mainCamera.forward);
    }
}
