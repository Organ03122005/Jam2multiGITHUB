using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 8f;        // ความเร็วเดิน
    public float gravity = -9.81f;  // แรงโน้มถ่วง

    Vector3 velocity;

    void Update()
    {
        // 1. รับค่าการกดปุ่ม WASD หรือลูกศร
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // 2. คำนวณทิศทาง (เดินตามหน้าตัวละคร)
        Vector3 move = transform.right * x + transform.forward * z;

        // 3. สั่งให้เดิน
        controller.Move(move * speed * Time.deltaTime);

        // 4. ระบบแรงโน้มถ่วง (กันตัวลอย)
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}