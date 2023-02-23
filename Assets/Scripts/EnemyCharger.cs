using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharger : Enemy
{
    public float chargeRange = 5f;   // Jarak serangan menyeruduk
    public float chargeSpeed = 10f;  // Kecepatan menyeruduk
    public float chargeTimer = 0f; // Timer

    private float _delayCharge = 0f;   // Waktu selanjutnya enemy dapat menyeruduk
    [SerializeField] bool _charging = false;  // Status menyeruduk

    protected override void Update()
    {
        base.Update();

        // Jika pemain dalam jarak serangan menyeruduk, seruduk pemain
        float distanceToTarget = Vector2.Distance(transform.position, target.position);
        if (distanceToTarget <= chargeRange && !_charging && _delayCharge <= 0)
        {
            _charging = true;
            StartCoroutine(Charge());
        }

        if (_charging)
        {
            // Menghitung waktu serangan
            chargeTimer += Time.deltaTime;
            if (chargeTimer >= 1f)
            {
                // Mengakhiri serangan setelah 1 detik
                _charging = false;
                chargeTimer = 0;
                _delayCharge = 5f;
                rb.velocity = Vector2.zero;
            }
        }
        else
        {
            if(_delayCharge > 0)
            {
                _delayCharge -= Time.deltaTime;
            }
        }
    }

    protected override void ChasePlayer()
    {
        if (!_charging)
        {
            // Mengatur arah gerakan musuh ke arah pemain
            Vector2 direction = (target.position - transform.position).normalized;
            rb.MovePosition((Vector2)transform.position + (direction * moveSpeed * Time.deltaTime));

            // Mengatur rotasi musuh agar menghadap ke arah pemain
            Vector3 targetDirection = target.position - transform.position;
            float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90f;
            rb.rotation = angle;
        }
    }

    private IEnumerator Charge()
    {
        // Menyimpan arah awal gerakan musuh
        Vector2 initialDirection = (target.position - transform.position).normalized;

        // Mengatur arah gerakan musuh ke arah pemain dan kecepatan menyeruduk
        Vector2 chargeDirection = (target.position - transform.position).normalized;
        rb.velocity = chargeDirection * chargeSpeed;

        // Menunggu hingga musuh berhenti menyeruduk
        yield return new WaitForSeconds(0.5f);
        rb.velocity = Vector2.zero;
        _charging = false;
    }
}
