using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    public CharacterController controller;
    public Animator animator;
    public GameObject punchHitbox; // ลากวัตถุ Hitbox มาใส่

    [Header("Movement Settings")]
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float gravity = -20f;
    public float jumpHeight = 2f;

    [Header("Mouse Rotation")]
    public float mouseSensitivity = 100f;

    [Header("Attack Settings")]
    public float attackCooldown = 0.5f;
    float lastAttackTime;
    [Header("Combat Settings")]
    public float fightingModeDuration = 3f; // จะค้างท่าเตรียมสู้ไว้นานแค่ไหน (วินาที)
    float fightingTimer;

    Vector3 velocity;

    void Update()
    {
        // --- โลจิกนับเวลาถอยหลังการค้างท่าเตรียมสู้ ---
        if (fightingTimer > 0)
        {
            fightingTimer -= Time.deltaTime;
            animator.SetBool("isFighting", true);
        }
        else
        {
            animator.SetBool("isFighting", false);
        }

        // --- แก้ไขส่วนการโจมตี (HandleAttack) ---
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("punch");
            fightingTimer = fightingModeDuration; // รีเซ็ตเวลานับถอยหลังทุกครั้งที่ต่อย
            lastAttackTime = Time.time;
        }

        // ... ส่วนอื่นๆ ของโค้ดคงเดิม ...
        // 1. หมุนตัวละครด้วยคลิกขวา
        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            transform.Rotate(Vector3.up * mouseX);
        }

        // 2. คลิกซ้ายเพื่อต่อย (ถ้าพ้นคูลดาวน์)
        if (Input.GetMouseButtonDown(0) && Time.time >= lastAttackTime + attackCooldown)
        {
            animator.SetTrigger("punch"); // ชื่อ Parameter ใน Animator
            lastAttackTime = Time.time;
        }

        // 3. ระบบเดิน
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;

        if (move.magnitude >= 0.1f)
        {
            animator.SetBool("isWalking", true);
            float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
            controller.Move(move * currentSpeed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        // 4. กระโดดและแรงโน้มถ่วง
        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    // ฟังก์ชันเปิด/ปิด Hitbox (เรียกใช้ผ่าน Animation Event ตามที่เคยแนะนำ)
    public void EnablePunchHitbox() { if (punchHitbox) punchHitbox.SetActive(true); }
    public void DisablePunchHitbox() { if (punchHitbox) punchHitbox.SetActive(false); }
}