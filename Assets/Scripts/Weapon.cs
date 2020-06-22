using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    Bullet bulletPrefab;
    [SerializeField]
    float weaponBulletSpeed = 10;
    [SerializeField]
    float timeToReload = 1f;
    public int weaponDamage = 50;
    [SerializeField]
    int magCapacity;
    private bool readyToShoot = true;
    private float reloadingTime = 1f;
    
    public int bulletLeft;
    [SerializeField]
    ShooterAgent shooter;

    public bool ReadyToShoot { get { return readyToShoot; } }
    public float TimeToReload { get { return timeToReload; } }
    public float ReloadingTime { get { return reloadingTime; } }
    public int BulletLeft { get { return bulletLeft; } }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(reloadingTime >= timeToReload && !readyToShoot)
        {
            readyToShoot = true;
        }
        else
        {
            reloadingTime += Time.deltaTime;
        }
    }

    public bool Shoot()
    {
        if (readyToShoot)
        {
            if (bulletLeft <= 0) shooter.EndEpisode();
            else bulletLeft--;
            Bullet bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.bulletSpeed = weaponBulletSpeed;
            bullet.bulletOwner = shooter;
            bullet.bulletDamage = weaponDamage;
            bullet.transform.rotation = transform.rotation;
            reloadingTime = 0f;
            readyToShoot = false;
            return true;
        }
        return false;
    }

    public void WeaponReset()
    {
        bulletLeft = magCapacity;
        readyToShoot = true;
        reloadingTime = timeToReload;
    }
}
