using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public Transform firePoint;
    public GameObject[] bulletPrefabs; // Tipe Peluru

    public float[] bulletForces; //Kekuatan Peluru

    private int _currentWeaponIndex = 0;
    private BulletPool _bulletPool;

    // Flamethrower specific variables
    public GameObject flamePrefab;
    public float flameDuration = 3f;
    public float flameDelay = 0.1f;

    // Grenade Launcher specific variables
    public int grenadeAmmo = 3;
    public GameObject grenadePrefab;
    public float grenadeForce = 10f;
    public float grenadeThrowDelay = 5f;

    private bool _isThrowingGrenade = false;
    private bool _isShootingFlame = false;

    public GameObject[] weaponPrefabs; // Array dari Weapon Prefabs
    public GameObject currentWeapon; // Senjata yang sedang digunakan

    // Start is called before the first frame update
    void Start()
    {
        InitBulletPool();
    }

    void InitBulletPool()
    {
        _bulletPool = FindObjectOfType<BulletPool>();
        if (_bulletPool == null)
        {
            Debug.LogError("No BulletPool found in scene.");
            return;
        }
        _bulletPool.bulletPrefab = bulletPrefabs[_currentWeaponIndex];
        _bulletPool.poolSize = 10;
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
            if (_currentWeaponIndex == 0)
            {
                //Jika senjata no 0, tembak bullet biasa
                ShootBullet();
            }
            else if (_currentWeaponIndex == 1)
            {
                //Jika senjata no 1, tembak grenade launcher
                ShootGrenadeLauncher();
            }
            else if (_currentWeaponIndex == 2)
            {
                //Jika senjata no 2, tembak flamethrower
                ShootFlamethrower();
            }
        }
        else if (Input.GetButtonUp("Fire1") && _currentWeaponIndex == 2)
        {
            //Hentikan tembakan jika melepaskan klik kiri pada flamethrower
            StopShootingFlame();
        }
    }

    void ShootBullet()
    {
        GameObject bullet = _bulletPool.GetBullet();
        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = firePoint.rotation;
        if(bullet.TryGetComponent(out Rigidbody2D rb))
        {
            rb.AddForce(firePoint.up * bulletForces[_currentWeaponIndex], ForceMode2D.Impulse);
        }
    }

    void ShootGrenadeLauncher()
    {
        StartCoroutine(ThrowGrenade());
    }

    void ShootFlamethrower()
    {
        if (!_isShootingFlame)
        {
            _isShootingFlame = true;
            StartCoroutine(ShootFlame());
        }
    }
    void StopShootingFlame()
    {
        _isShootingFlame = false;
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
        if (grenade.TryGetComponent(out Rigidbody2D rb))
        {
            rb.AddForce(firePoint.up * grenadeForce, ForceMode2D.Impulse);

            // Delay waktu player melempar grenade
            yield return new WaitForSeconds(grenadeThrowDelay);
            _isThrowingGrenade = false;

            // Menghentikan grenade yang telah dilempar
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
        
        // Membuat efek ledakan
        ExplodeGrenade(grenade.transform.position);
        Destroy(grenade);
    }

    IEnumerator ShootFlame()
    {
        while (Input.GetButton("Fire1"))
        {
            // Buat instance flamePrefab
            GameObject flame = Instantiate(bulletPrefabs[_currentWeaponIndex], firePoint.position, firePoint.rotation);

            // Set parent flame ke firePoint agar mengikuti rotasi dari firePoint
            flame.transform.SetParent(firePoint);

            // Set Active flame prefab
            flame.SetActive(true);

            // Set kecepatan flame
            if (flame.TryGetComponent(out Rigidbody2D rb))
            {
                rb.velocity = firePoint.up * bulletForces[2];
            }

            // Hancurkan setelah beberapa waktu
            Destroy(flame, 2f);

            // Pause sejenak untuk memberi waktu flame menyentuh objek
            yield return new WaitForSeconds(0.1f);

            // Menghitung jarak dari api ke setiap musuh
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(flame.transform.position, 3f);
            foreach (Collider2D hit in hitEnemies)
            {
                if (hit.TryGetComponent(out Enemy enemy))
                {
                    if (enemy != null)
                    {
                        enemy.TakeDamage(100);
                    }
                }
            }
        }
    }

    void ExplodeGrenade(Vector3 position)
    {
        // Mencari semua objek di sekitar posisi ledakan
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, 5f);

        foreach (Collider2D hit in colliders)
        {
            // Jika objek memiliki komponen Rigidbody2D, tambahkan kekuatan ledakan ke objek tersebut
            if (hit.TryGetComponent(out Rigidbody2D rb))
            {
                if (rb != null)
                {
                    Vector2 direction = hit.transform.position - position;
                    float distance = Vector2.Distance(hit.transform.position, position);

                    rb.AddForce(direction.normalized * (grenadeForce / distance), ForceMode2D.Impulse);
                }
            }

            // Jika objek adalah musuh, maka musuh tersebut akan mati
            if (hit.TryGetComponent(out Enemy enemy))
            {
                if (enemy != null)
                {
                    enemy.TakeDamage(100);
                }
            }
        }
    }
}
