using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class weapon : MonoBehaviour
{



    //Config basica de disparo
    public bool isShooting, readyToShoot;
    bool allowReset = true;
    public float shootingDelay = 2f;



    //Modo de rafaga
    public int bulletsPerBurst = 3;
    public int burstBulletsLeft;

    //Propagacion
    public float spreadIntensity;

    //Propiedades de bala
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30;
    public float bulletPrefabLifeTime = 3f;

    public GameObject muzleEffect;
    public enum shootingMode
    {
        Single,
        Burst,
        Auto
    }

    public shootingMode currenShootingMode;
    private Animator animator;
    public float reloadtime;
    public int magazineSize, bulletsLeft;
    public bool isReloading;
    public bool isADS;


    public void Awake()
    {
        readyToShoot = true;
        burstBulletsLeft = bulletsPerBurst;
        animator = GetComponent<Animator>();

        bulletsLeft = magazineSize;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(1))
        {

            animator.SetTrigger("enterADS");
            isADS = true;
        }

        if (Input.GetMouseButtonUp(1))
        {

            animator.SetTrigger("exitADS");
            isADS = false;
        }
        if (bulletsLeft == 0 && isShooting)
        {
            soundManager.instance.emptySound.Play();
        }
        if (Input.GetKey(KeyCode.R) && bulletsLeft < magazineSize && isReloading == false)
        {
            Reload();
        }

        // if (readyToShoot && isShooting == false && isReloading == false && bulletsLeft <= 0)
        // {
        //     Reload();
        // }


        if (currenShootingMode == shootingMode.Auto)
        {
            isShooting = Input.GetKey(KeyCode.Mouse0);
        }
        else if (currenShootingMode == shootingMode.Single || currenShootingMode == shootingMode.Burst)
        {
            isShooting = Input.GetKeyDown(KeyCode.Mouse0);
        }

        if (readyToShoot && isShooting && bulletsLeft > 0 && !isReloading)
        {
            burstBulletsLeft = bulletsPerBurst;
            FireWeapon();
        }

        if (ammoManager.instance.ammoDisplay != null)
        {
            ammoManager.instance.ammoDisplay.text = $"{bulletsLeft / bulletsPerBurst}/{magazineSize / bulletsPerBurst}";
        }
    }

    private void FireWeapon()
    {
        bulletsLeft--;
        muzleEffect.GetComponent<ParticleSystem>().Play();
        soundManager.instance.shootingSoundM911.Play();
        if (isADS)
        {
            animator.SetTrigger("recoilADS");
        }
        else
        {
            animator.SetTrigger("recoil");
        }
        
        readyToShoot = false;
        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);

        bullet.transform.forward = shootingDirection;
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity, ForceMode.Impulse);

        StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLifeTime));

        if (allowReset)
        {
            Invoke(nameof(ResetShoot), shootingDelay);
            allowReset = false;
        }

        if (currenShootingMode == shootingMode.Burst && burstBulletsLeft > 1)
        {
            burstBulletsLeft--;
            Invoke(nameof(FireWeapon), shootingDelay);
        }
    }

    private void Reload()
    {
        animator.SetTrigger("reload");
        soundManager.instance.reloadSound.Play();
        isReloading = true;
        Invoke("reloadCompleted", reloadtime);
    }

    private void reloadCompleted()
    {
        bulletsLeft = magazineSize;
        animator.SetBool("reload", false);
        isReloading = false;
    }

    private void ResetShoot()
    {
        readyToShoot = true;
        allowReset = true;
    }

    private Vector3 CalculateDirectionAndSpread()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(100);
        }

        Vector3 direction = targetPoint - bulletSpawn.position;
        float x = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

        return direction + new Vector3(x, y, 0);
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
}
