using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public CharacterController controller; // ลาก Character Controller มาใส่
    public float speed = 10f;          // ความเร็วในการเดิน
    public float gravity = -9.81f;      // แรงโน้มถ่วง (ถ้าอยากให้ตกลงเร็วๆ ให้เพิ่มค่าติดลบ เช่น -15)

    [Header("Jump Settings")]
    public float jumpHeight = 3f;      // ความสูงของการกระโดด

    [Header("Ground Check")]
    public Transform groundCheck;      // ลากวัตถุ GroundCheck มาใส่
    public float groundDistance = 0.4f; // รัศมีในการเช็คพื้น
    public LayerMask groundMask;       // เลือก Layer "Ground"

    Vector3 velocity;
    bool isGrounded;

    void Update()
    {
        // 1. เช็คว่าอยู่บนพื้นไหม
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // ถ้าอยู่บนพื้นและกำลังตก ให้หยุดความเร็วที่ติดลบไว้ (ให้แรงกดนิดหน่อยเพื่อให้เสถียร)
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // 2. รับค่าการเดิน (WASD)
        float x = Input.GetAxis("Horizontal"); // A (-1), D (1)
        float z = Input.GetAxis("Vertical");   // S (-1), W (1)

        // 3. คำนวณทิศทางเดิน (อิงตามหน้าตัวละคร)
        Vector3 move = transform.right * x + transform.forward * z;

        // 4. สั่งให้เดิน (ใช้ Time.deltaTime เพื่อให้เดินสมูททุกคอม)
        controller.Move(move * speed * Time.deltaTime);

        // 5. การกระโดด (กดปุ่ม Space และต้องอยู่บนพื้น)
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // สูตรคำนวณ v = sqrt(h * -2 * g)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // 6. คำนวณแรงโน้มถ่วง
        velocity.y += gravity * Time.deltaTime;

        // 7. สั่งให้ตกลงมา (หรือกระโดดขึ้นไป)
        controller.Move(velocity * Time.deltaTime);
    }
}