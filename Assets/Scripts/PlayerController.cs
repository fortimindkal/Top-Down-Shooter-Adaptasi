using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;        // Kecepatan gerakan karakter
    [SerializeField] float _playerHealth = 100f;

    private Vector2 moveDir;      // Arah gerakan karakter
    private Vector2 mousePos;   // Posisi Mouse

    public Rigidbody2D rb;
    public Camera cam;

    private void Start()
    {

    }

    private void Update()
    {
        // Mendapatkan posisi kursor mouse
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Mendapatkan input gerakan karakter
        moveDir.x = Input.GetAxisRaw("Horizontal");
        moveDir.y = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        Movement();
        Rotation();
    }

    private void Movement()
    {
        // Menggerakkan karakter sesuai arah gerakan dan kecepatan
        rb.MovePosition(rb.position + moveDir * moveSpeed * Time.fixedDeltaTime);
    }

    private void Rotation()
    {
        // Mengarahkan senjata ke arah kursor mouse
        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
    }

    public void TakeDamage(float health)
    {
        _playerHealth -= health;
    }
}
