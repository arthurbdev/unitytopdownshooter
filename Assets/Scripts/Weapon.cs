using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float timeBetweenRounds;
    [SerializeField] private float fireForce;
    [SerializeField] private float bulletSpread;

    private float cooldown;

    public void Shoot()
    {
        if (Time.time > cooldown)
        {
            Quaternion spread = shootPoint.rotation * Quaternion.Euler(0f, 0f, Random.Range(-bulletSpread, bulletSpread));
            GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, spread);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(bullet.transform.up * fireForce, ForceMode2D.Impulse);
            cooldown = Time.time + timeBetweenRounds;
        }
    }
}
