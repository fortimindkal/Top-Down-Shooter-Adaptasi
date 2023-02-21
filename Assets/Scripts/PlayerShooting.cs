using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public Transform firePoint;
    public GameObject[] bulletPrefabs; // Tipe Peluru

    public float[] bulletForces; //Kekuatan Peluru

    private int _currentWeaponIndex = 0;

    // Grenade Launcher specific variables
    public int grenadeAmmo = 3;
    public GameObject grenadePrefab;
    public float grenadeForce = 10f;
    public float grenadeThrowDelay = 5f;

    private bool _isThrowingGrenade = false;

    public GameObject[] weaponPrefabs; // Array dari Weapon Prefabs
    public GameObject currentWeapon; // Senjata yang sedang digunakan

    public void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            //Mengganti Senjata jika menekan Tab
            SwitchWeapon();
        }

        if (Input.GetButtonDown("Fire1"))
        {
            //Menembak jika menekan klik kiri
            Shoot();
        }


        if (Input.GetKeyDown(KeyCode.G) && grenadeAmmo > 0 && !_isThrowingGrenade)
        {
            //Melempar Grenade
            StartCoroutine(ThrowGrenade());
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefabs[_currentWeaponIndex], firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * bulletForces[_currentWeaponIndex], ForceMode2D.Impulse);
    }

    void SwitchWeapon()
    {
        // Menonaktif senjata yang digunakan
        currentWeapon.SetActive(false);

        // Mengganti data array senjata
        _currentWeaponIndex = (_currentWeaponIndex + 1) % weaponPrefabs.Length;
        currentWeapon = weaponPrefabs[_currentWeaponIndex];

        // Mengaktifkan senjata kembali
        currentWeapon.SetActive(true);
    }

    IEnumerator ThrowGrenade()
    {
        _isThrowingGrenade = true;
        grenadeAmmo--;

        // Membuat dan membuang grenade
        GameObject grenade = Instantiate(grenadePrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = grenade.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * grenadeForce, ForceMode2D.Impulse);

        // Delay waktu player melempar grenade
        yield return new WaitForSeconds(grenadeThrowDelay);
        _isThrowingGrenade = false;

        // Menghentikan grenade yang telah dilempar
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;

        // Membuat efek blink
        StartCoroutine(BlinkSprite());
    }

    private IEnumerator BlinkSprite()
    {
        SpriteRenderer spriteRenderer = grenadePrefab.GetComponent<SpriteRenderer>();

        // Blink selama 3 detik
        float totalTime = 3f;
        float blinkTime = 0.2f;

        while (totalTime > 0f)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(blinkTime);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(blinkTime);

            totalTime -= blinkTime * 2f;
        }
    }
}
