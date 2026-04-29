using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : MonoBehaviour
{
    [Header("Rifle Settings")]
    public Camera cam; // cam lấy vị trí và hg bắn
    public float giveDamageOf = 10f; // dame
    public float shootingRange = 100f; // kc bắn
    public float fireCharge = 15f; // tốc độ bắn, càng cao càng nhanh, 15 viên / 1s
    private float nextTimeToShoot = 0f; // thời điểm bắn tiếp
    public PlayerScript playerScript;
    public Transform hand; // tay gắn súng
    public Animator animator; // animation
    public GameObject rifleUI; // ui hiển thị thông số súng

    [Header("Rifle Ammunition and shooting")]
    private int maximumAmmunition = 32; // số lượng đạn trong 1 băng
    public int mag = 10; // số lượng băng đạn
    private int presentAmmunition; // đạn hiện tại
    public float reloadingTime = 1.3f; // thời reloading thay băng đạn
    private bool setReloading = false; // biến đang reloading hay k


    [Header("Rifle Effects")]
    // hiệu ứng muzzle khi bắn súng
    public ParticleSystem muzzleSpark;
    // hiệu ứng khi bắn vào gỗ
    public GameObject woodEffect;
    // hiệu ứng khi bắn vào zombie
    public GameObject goreEffect;

    [Header("Sound and UI")]
    // vùng nguy hiểm khi hết đạn, hiện lên để cảnh báo người chơi
    public GameObject dangerZone1;
    // âm thanh bắn súng
    public AudioClip shootingSound;
    // âm thanh reloading
    public AudioClip reloadingSound;
    // phát âm thanh
    public AudioSource audioSource;

    private void Awake()
    {
        // khởi tạo set cha la cánh tay phải
        transform.SetParent(hand);
        // số lượng đạn = số lượng đạn tối đa
        presentAmmunition = maximumAmmunition;
        rifleUI.SetActive(true); // bật UI rifle lên để có thông số súng
        //audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        // nếu đang reload thay đạn thì chặn các sự kiện bắn đạn hay các thứ khác
        if (setReloading) return;

        // nếu đạn <= 0 
        if(presentAmmunition <= 0)
        {
            // chạy hàm reloading
            // return chặn các lệnh phía sau
            StartCoroutine(Reload());
            return;
        }
        // ấn chuột trái (có cả giữ nữa vì k có Down)
        // và thời gian hiện tại >= thời điểm bắn tiếp theo
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToShoot)
        {
            // set anim bắn súng và false idle
            animator.SetBool("Fire", true);
            animator.SetBool("Idle", false);
            // tính lại thời điểm bắn tiếp theo
            // hiểu đơn giản Time.time = thời gian thực thế
            // 1f / fireCharge = thời gian giữa 2 lần bắn, vì fireCharge là số viên bắn trong 1s nên lấy 1s chia cho số viên sẽ ra thời gian giữa 2 lần bắn
            // Time.time = 10 (Ví dụ)
            // bắn 1 viên => thời gian tiếp theo là 
            // 10 + 1f / 15 = 10 + 0.066 = 10.066s => sau 0.066s sẽ bắn tiếp được viên tiếp theo
            // tức là lúc đấy ms đc bắn tiếp
            nextTimeToShoot = Time.time + 1f / fireCharge;
            // gọi hàm shoot để bắn
            Shoot();
        }
        // nếu ấn chuột trái và giữ phím W hoặc UpArrow thì chạy anim bắn súng khi đi bộ
        else if(Input.GetButton("Fire1") && Input.GetKey(KeyCode.W) || 
            Input.GetKey(KeyCode.UpArrow))
        {
            // vừa đi vừa bắn
            animator.SetBool("Idle", false);
            animator.SetBool("FireWalk", true);
        }
        // nếu ấn chuột phải và giữ phím W hoặc UpArrow thì chạy anim ngắm bắn súng khi đi bộ
        else if(Input.GetButton("Fire2") && Input.GetButton("Fire1"))
        {
            animator.SetBool("Idle", false);
            animator.SetBool("IdleAim", true);
            animator.SetBool("FireWalk", true);
            animator.SetBool("Walk", true);
            animator.SetBool("Reloading", false);
        }
        else
        {
            // ngược lại thì về trạng thái thg
            animator.SetBool("Fire", false);
            animator.SetBool("Idle", true);
            animator.SetBool("FireWalk", false);
        }
    }

    // hàm shoot
    private void Shoot()
    {
        // băn đạn = 0
        if (mag == 0)
        {
            // show ui hết đạn
            StartCoroutine(ShowAmmoOut());
            return;
        }
        // trừ đạn hiện tại đi 1 viên
        presentAmmunition--;
        if(presentAmmunition == 0)
        {
            // nếu đạn hiện tại = 0 thì trừ số băng đạn đi 1
            mag--;
        }
        // update số lượng đạn lên ui
        AmmoCount.occurrence.UpdateAmmoText(presentAmmunition);
        // update số lượng băng đạn lên UI
        AmmoCount.occurrence.UpdateMagText(mag);
        muzzleSpark.Play(); // chạy hàm hiệu ứng muzzle khi bắn ở nòng súng
        audioSource.PlayOneShot(shootingSound);
        RaycastHit hitInfo;
        // hàm raycast để bắn, lấy vị trí và hướng bắn từ cam,
        // nếu trúng vật gì đó trong khoảng cách bắn thì sẽ trả về thông tin va chạm trong hitInfo
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hitInfo, shootingRange))
        {
            ObjectToHit objectToHit = hitInfo.transform.GetComponent<ObjectToHit>();
            Zombie1 zombie1 = hitInfo.transform.GetComponent<Zombie1>();
            Zombie2 zombie2 = hitInfo.transform.GetComponent<Zombie2>();
            if (objectToHit != null)
            {
                objectToHit.ObjectHitDamage(giveDamageOf);
                GameObject impactGo = Instantiate(woodEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(impactGo, 1f);
            }
            else if (zombie1 != null)
            {
                zombie1.ZombieHitDamage(giveDamageOf);
                GameObject goreEffectOb = Instantiate(goreEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(goreEffectOb, 1f);
            }
            else if (zombie2 != null)
            {
                zombie2.ZombieHitDamage(giveDamageOf);
                GameObject goreEffectOb = Instantiate(goreEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(goreEffectOb, 1f);
            }
        }
    }
    // hàm IEnumerator để thực hiện reload
    private IEnumerator Reload()
    {
        // khi reload súng thì đặt tốc độ về 0 
        playerScript.playerSpeed = 0f;
        playerScript.playerSprint = 0f;
        setReloading = true; // set biến reloadig về true
        animator.SetBool("Reloading", true); // set anim
        audioSource.PlayOneShot(reloadingSound); // phát audio reload
        yield return new WaitForSeconds(reloadingTime); // đợi thời gian reload
        setReloading = false; // đặt lại sau khi reload xong
        animator.SetBool("Reloading", false); // set anim
        presentAmmunition = maximumAmmunition; // đặt lại lượng đạn = max đạn trong 1 băng
        playerScript.playerSpeed = 1.9f; // đặt lại tốc độ di chuyển và chạy
        playerScript.playerSprint = 3f;
    }

    // Hàm Show hết đạn
    private IEnumerator ShowAmmoOut()
    {
        // bật hết đạn lên
        dangerZone1.SetActive(true);
        yield return new WaitForSeconds(5f);
        // sau 5s thì tắt cảnh báo
        dangerZone1.SetActive(false);
    }
}
