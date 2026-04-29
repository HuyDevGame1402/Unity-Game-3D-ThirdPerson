using UnityEngine;

public class VehicelController : MonoBehaviour
{
    [Header("Wheels colliders")]
    // bánh xe vật lý
    // unity sử dụng mô phỏng lực để di chuyển xe nên cần có các collider để áp dụng lực vào đó
    // mô phỏng ma sát
    // mô phỏng lực phanh, ...
    public WheelCollider frontRightWheelCollider;
    public WheelCollider frontLeftWheelCollider;
    public WheelCollider backRightWheelCollider;
    public WheelCollider backLeftWheelCollider;

    [Header("Wheels transform")]
    // transform để mô phỏng bánh xe quay và rẽ
    public Transform frontRightWheelTransform;
    public Transform frontLeftWheelTransform;
    public Transform backRightWheelTransform;
    public Transform backLeftWheelTransform;
    // transform cửa xe để đặt player vào đó khi vào xe
    // và lấy vị trí khi player thoát xe để đặt player vào đó
    public Transform vehicleDoor;

    [Header("Vehicle Engine")]
    // lực tăng tốc
    public float accelerationForce = 100f;
    // lực phanh
    public float brakingForce = 200f;
    // lực phanh hiện tại
    private float presentBreakForce = 0f;
    // lực đang áp vào phanh
    public float presentAcceleration = 0f;

    [Header("Vehicle Steering")]
    // tốc độ xoay bánh
    public float wheelsTorque = 20f;
    // góc lái hiện tại
    private float presentTurnAngle = 0f;

    [Header("Vehicle Security")]
    // player
    public PlayerScript player;
    // khoảng cách tg tác vào xe
    private float radius = 5f;
    // biến đang trong xe hay k
    private bool isOpened = false;

    [Header("Disable Things")]
    // các biến cần tắt khi lái xe như came
    // player
    // hay các UI của player, ...
    public GameObject AimCam;
    public GameObject AimCanvas;
    public GameObject ThirdPersonCam;
    public GameObject ThirdPersonCanvas;
    public GameObject playerCharacter;

    [Header("Vehicle Hit Var")]
    // vị trí came để gây dame
    public Camera cam;
    // lượng dame
    public float giveDamageOf = 10f;
    // kc tấn công
    public float hitRange = 2f;
    // effect khi hit zombie
    public GameObject goreEffect;

    private void Update()
    {

        if (Vector3.Distance(transform.position, player.transform.position) < radius)
        {
            // nếu gần xe và ấn phím F để vào xe
            if (Input.GetKeyDown(KeyCode.F))
            {
                isOpened = true; // vào xe
                radius = 5000f; // tăng radius tránh trigger liên tục
                // cập nhật nhiệm vụ
                ObjectivesComplete.occurrence.GetObjectivesDone(true, true, true, false);
            }
            // nếu trong xe ấn G để thoát
            else if (Input.GetKeyDown(KeyCode.G))
            {
                // tăng 5000 radius để tránh việc đi xe ra xa quá player thì k xuống đc
                // hiểu đơn giản khi lên xe thì chúng ta ẩn player k có di chuyển player
                // thì lúc đó player vẫn ở vị trí cũ k đi theo xe 
                // nên p tăng radius lên cao để chắc chắn vẫn có thể xuống xe đc
                // xuống xe đặt lại vị trí player = vị trí cửa và chỉnh các biến khác
                player.transform.position = vehicleDoor.transform.position;
                isOpened = false;
                radius = 5f;
            }
        }
        // nếu đang trong xe 
        if (isOpened == true)
        {
            // ẩn các biến disible
            ThirdPersonCam.SetActive(false);
            ThirdPersonCanvas.SetActive(false);
            AimCam.SetActive(false);
            AimCanvas.SetActive(false);
            playerCharacter.SetActive(false);
            // gọi các hàm di chuyển xe vs các hàm lực, phanh
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
    // di chuyển xe
    private void MoveVehicle()
    {
        // lc tăng tốc = lực tăng tốc * input vertical (w/s)
        // lực truyền vào bánh xe
        // motorTorque là lực truyền vào bánh xe = lực làm quay bánh xe
        // nếu lực càng lớn thì bánh quay càng nhanh -> xe chạy nhanh
        frontRightWheelCollider.motorTorque = presentAcceleration;
        frontLeftWheelCollider.motorTorque = presentAcceleration;
        backRightWheelCollider.motorTorque = presentAcceleration;
        backLeftWheelCollider.motorTorque = presentAcceleration;
        // lực ga hiện tại = lực tăng tốc * input vertical (w/s)
        // w = +1, s = -1, k ấn = 0
        // tại sao có dấu trừ ở đây vì khi ấn w thì muốn xe đi về phía trước nên lực phải ngược lại với hướng của bánh xe
        // vì trong game forward của bánh xe là hướng về phía trước của bánh xe nên muốn đi về phía đó thì lực phải ngược lại với hướng đó
        // nghĩa là có thể ngược vs trục Z nên phải đổi lại
        // thực tế thì cứ test thì sẽ hiểu vì sao có dấu trừ ở đây
        // 1 trong 2 th là để nguyên dấu hoặc - ở trước là ra

        presentAcceleration = accelerationForce * -Input.GetAxis("Vertical");
    }
    private void VehicleSteering()
    {
        // tính tốc độ quay của bánh xe
        // A = -1, D = +1, k ấn = 0
        presentTurnAngle = wheelsTorque * Input.GetAxis("Horizontal");
        // steerAngle là góc lái của bánh xe theo trục Y
        // chúng ta chỉ cần đặt bánh trc theo hướng quay
        // bánh sau sẽ đi theo chứ k cần đặt bánh sau theo hướng lái
        frontRightWheelCollider.steerAngle = presentTurnAngle;
        frontLeftWheelCollider.steerAngle = presentTurnAngle;
        // đồng bộ mesh bánh xe với collider bánh xe
        SteeringWheels(frontRightWheelCollider, frontRightWheelTransform);
        SteeringWheels(frontLeftWheelCollider, frontLeftWheelTransform);
        SteeringWheels(backRightWheelCollider, backRightWheelTransform);
        SteeringWheels(backLeftWheelCollider, backLeftWheelTransform);
    }
    private void SteeringWheels(WheelCollider wc, Transform wt)
    {
        Vector3 position;
        Quaternion rotation;
        // lấy vị trí bánh thật của sau khi physics tính
        // từ đó lấy đc position vs rotation ap vào transform để chỉnh mesh view vs rotation ra
        wc.GetWorldPose(out position, out rotation);
        // đặt vào transform bánh xe
        wt.position = position;
        wt.rotation = rotation;
    }

    private void ApplyBreaks()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            // nếu ấn phím space để phanh thì lực phanh hiện tại = lực phanh
            presentBreakForce = brakingForce;
        }
        else
        {
            // k ấn thì phanh = 0
            presentBreakForce = 0f;
        }
        // đặt lực phanh vào biến brankeTorque của bánh xe để giảm tốc phanh xe
        frontRightWheelCollider.brakeTorque = presentBreakForce;
        frontLeftWheelCollider.brakeTorque = presentBreakForce;
        backRightWheelCollider.brakeTorque = presentBreakForce;
        backLeftWheelCollider.brakeTorque = presentBreakForce;
    }

    // hàm tấn công zombie khi đâm vào zombie
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
