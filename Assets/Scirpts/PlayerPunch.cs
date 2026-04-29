using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPunch : MonoBehaviour
{
    [Header("Player Punch Var")]
    public Camera cam;
    public float giveDamageOf = 10f; // lượng dame gây ra
    public float punchingRange = 5f; // khoảng cách tấn công đến mục tiêu

    public void Punch()
    {
        RaycastHit hitInfo;
        // bắn 1 tia raycast từ vị trí camera hướng theo camera 
        // khoảng cách bắn raycast là 5 chính là biến đã khai báo ở trên
        // neu raycast trúng 1 vật thể nào đó thì sẽ trả về thông tin của vật thể đó vào biến hitInfo
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hitInfo,
            punchingRange))
        {
            // lấy ra thông tin vật thể có thể gây dame đc thì gây dame vào vật thể đó
            ObjectToHit objectToHit = hitInfo.transform.GetComponent<ObjectToHit>();
            Zombie1 zombie1 = hitInfo.transform.GetComponent<Zombie1>();
            Zombie2 zombie2 = hitInfo.transform.GetComponent<Zombie2>();
            if (objectToHit != null)
            {
                objectToHit.ObjectHitDamage(giveDamageOf);
            }
            if (zombie1 != null)
            {
                zombie1.ZombieHitDamage(giveDamageOf);
            }
            if (zombie2 != null)
            {
                zombie2.ZombieHitDamage(giveDamageOf);
            }
        }
    }
}
