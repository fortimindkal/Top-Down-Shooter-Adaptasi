using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPistol : MonoBehaviour
{
    public float maxDistance = 10f;
    private float _distanceTravelled;

    // Update is called once per frame
    void Update()
    {
        _distanceTravelled += Time.deltaTime * GetComponent<Rigidbody2D>().velocity.magnitude;
        if (_distanceTravelled > maxDistance)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        _distanceTravelled = 0f;
    }
}
