using UnityEngine;

public class Shotgun : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private AudioSource weaponSource;
    [SerializeField] private Transform firePoint;
    [SerializeField] private int bulletCount = 10;
    [SerializeField] private float spreadAngle = 10f;
    [SerializeField] private float verticalSpreadAngle = 5f;
    [SerializeField] private float bulletSpeed = 20f;
    [SerializeField] private float bulletLifetime = 0.5f;

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            FireShotgun();
            weaponSource.Play();
        }
    }

    void FireShotgun()
    {
        for (int i = 0; i < bulletCount; i++)
        {
            float horizontalOffset = Random.Range(-spreadAngle, spreadAngle);
            float verticalOffset = Random.Range(-verticalSpreadAngle, verticalSpreadAngle);
            Quaternion spreadRotation = Quaternion.Euler(verticalOffset, horizontalOffset, 0);
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation * spreadRotation);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = bullet.transform.forward * bulletSpeed;
            }
            Destroy(bullet, bulletLifetime);
        }
    }
}
