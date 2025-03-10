using UnityEngine;
using TMPro;

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
    [SerializeField] private Animator animator;
    [SerializeField] private TextMeshProUGUI ammoText;

    private int ammo = 6;
    private bool canShoot = true;
    private bool isReloading = false;

    void Start()
    {
        UpdateAmmoUI();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && canShoot && !isReloading && ammo > 0)
        {
            FireShotgun();
        }

        if (Input.GetKeyDown(KeyCode.R) && ammo < 6 && canShoot && !isReloading)
        {
            Reload();
        }
    }

    void FireShotgun()
    {
        canShoot = false;
        animator.ResetTrigger("Fire");
        animator.SetTrigger("Fire");
        weaponSource.Play();
        ammo--;
        UpdateAmmoUI();
    }

    void Reload()
    {
        canShoot = false;
        isReloading = true;
        animator.ResetTrigger("Reload");
        animator.SetTrigger("Reload");
    }

    public void OnFireAnimationEvent()
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

    public void OnReloadAnimationEvent()
    {
        ammo = 6;
        UpdateAmmoUI();
    }

    public void OnReloadAnimationEnd()
    {
        canShoot = true;
        isReloading = false;
    }

    public void OnFireAnimationEnd()
    {
        canShoot = true;
    }

    void UpdateAmmoUI()
    {
        if (ammoText != null)
        {
            ammoText.text = "Ammo: " + ammo;
        }
    }
}