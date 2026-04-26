using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class VehicelController : MonoBehaviour
{
    [Header("Wheels colliders")]
    public WheelCollider frontRightWheelCollider;
    public WheelCollider frontLeftWheelCollider;
    public WheelCollider backRightWheelCollider;
    public WheelCollider backLeftWheelCollider;

    [Header("Wheels transform")]
    public Transform frontRightWheelTransform;
    public Transform frontLeftWheelTransform;
    public Transform backRightWheelTransform;
    public Transform backLeftWheelTransform;
    public Transform vehicleDoor;

    [Header("Vehicle Engine")]
    public float accelerationForce = 100f;
    public float brakingForce = 200f;
    private float presentBreakForce = 0f;
    public float presentAcceleration = 0f;

    [Header("Vehicle Steering")]
    public float wheelsTorque = 20f;
    private float presentTurnAngle = 0f;

    [Header("Vehicle Security")]
    public PlayerScript player;
    private float radius = 5f;
    private bool isOpened = false;

    [Header("Disable Things")]
    public GameObject AimCam;
    public GameObject AimCanvas;
    public GameObject ThirdPersonCam;
    public GameObject ThirdPersonCanvas;
    public GameObject playerCharacter;

    [Header("Vehicle Hit Var")]
    public Camera cam;
    public float giveDamageOf = 10f;
    public float hitRange = 2f;
    public GameObject goreEffect;

    private void Update()
    {

        if (Vector3.Distance(transform.position, player.transform.position) < radius)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                isOpened = true;
                radius = 5000f;
            }
            else if (Input.GetKeyDown(KeyCode.G))
            {
                player.transform.position = vehicleDoor.transform.position;
                isOpened = false;
                radius = 5f;
            }
        }

        if (isOpened == true)
        {
            ThirdPersonCam.SetActive(false);
            ThirdPersonCanvas.SetActive(false);
            AimCam.SetActive(false);
            AimCanvas.SetActive(false);
            playerCharacter.SetActive(false);
            MoveVehicle();
            VehicleSteering();
            ApplyBreaks();
            HitZombies();
        }
        else if(isOpened == false)
        {
            ThirdPersonCam.SetActive(true);
            ThirdPersonCanvas.SetActive(true);
            AimCam.SetActive(true);
            AimCanvas.SetActive(true);
            playerCharacter.SetActive(true);
        }
    }
    private void MoveVehicle()
    {
        frontRightWheelCollider.motorTorque = presentAcceleration;
        frontLeftWheelCollider.motorTorque = presentAcceleration;
        backRightWheelCollider.motorTorque = presentAcceleration;
        backLeftWheelCollider.motorTorque = presentAcceleration;

        presentAcceleration = accelerationForce * -Input.GetAxis("Vertical");
    }
    private void VehicleSteering()
    {
        presentTurnAngle = wheelsTorque * Input.GetAxis("Horizontal");
        frontRightWheelCollider.steerAngle = presentTurnAngle;
        frontLeftWheelCollider.steerAngle = presentTurnAngle;
        SteeringWheels(frontRightWheelCollider, frontRightWheelTransform);
        SteeringWheels(frontLeftWheelCollider, frontLeftWheelTransform);
        SteeringWheels(backRightWheelCollider, backRightWheelTransform);
        SteeringWheels(backLeftWheelCollider, backLeftWheelTransform);
    }
    private void SteeringWheels(WheelCollider wc, Transform wt)
    {
        Vector3 position;
        Quaternion rotation;

        wc.GetWorldPose(out position, out rotation);

        wt.position = position;
        wt.rotation = rotation;
    }

    private void ApplyBreaks()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            presentBreakForce = brakingForce;
        }
        else
        {
            presentBreakForce = 0f;
        }

        frontRightWheelCollider.brakeTorque = presentBreakForce;
        frontLeftWheelCollider.brakeTorque = presentBreakForce;
        backRightWheelCollider.brakeTorque = presentBreakForce;
        backLeftWheelCollider.brakeTorque = presentBreakForce;
    }

    private void HitZombies()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hitInfo, hitRange))
        {
            Zombie1 zombie1 = hitInfo.transform.GetComponent<Zombie1>();
            Zombie2 zombie2 = hitInfo.transform.GetComponent<Zombie2>();

            if (zombie1 != null)
            {
                zombie1.ZombieHitDamage(giveDamageOf);
                zombie1.GetComponent<CapsuleCollider>().enabled = false;
                GameObject goreEffectOb = Instantiate(goreEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(goreEffectOb, 1f);
            }
            else if (zombie2 != null)
            {
                zombie2.ZombieHitDamage(giveDamageOf);
                zombie2.GetComponent<CapsuleCollider>().enabled = false;
                GameObject goreEffectOb = Instantiate(goreEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(goreEffectOb, 1f);
            }
        }
    }
}
