using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiflePickup : MonoBehaviour
{
    [Header("Rifle's")]
    public GameObject playerRifle;
    public GameObject pickupRifle;

    [Header("Rifle Assign Things")]
    public PlayerScript playerScript;
    public float radius = 2.5f;

    private void Awake()
    {
        playerRifle.SetActive(false);   
    }

    private void Update()
    {
        if(Vector3.Distance(transform.position, playerScript.transform.position) <= radius)
        {
            if (Input.GetKeyDown("f"))
            {
                playerRifle.SetActive(true);
                pickupRifle.SetActive(false);


            }
        }
    }
}
