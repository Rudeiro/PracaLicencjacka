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
    [SerializeField]
    float fireRate;
    public int weaponDamage = 50;
    [SerializeField]
    int magCapacity;
    private bool readyToShoot = true;
    private float reloadingTime = 1f;
    private float timeToNextBullet = 1f;
    private bool reloading = false;
    
    public int bulletLeft;
    
    public InfinityAgent shooter;

    private List<Bullet> bullets = new List<Bullet>();

    public bool ReadyToShoot { get { return readyToShoot; } }
    public float TimeToReload { get { return timeToReload; } }
    public float ReloadingTime { get { return reloadingTime; } }
    public int BulletLeft { get { return bulletLeft; } }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(timeToNextBullet >= fireRate && !readyToShoot)
        {
            readyToShoot = true;
        }
        else
        {
            timeToNextBullet += Time.fixedDeltaTime;
        }
        if (reloadingTime >= timeToReload && reloading)
        {
            reloading = false;
            bulletLeft = magCapacity;
        }
        else
        {
            reloadingTime += Time.fixedDeltaTime;
        }
    }

    public bool Shoot()
    {
        if (readyToShoot && this != null && !reloading)
        {
            if (bulletLeft <= 0)
            {
                reloadingTime = 0;
                reloading = true;
            }
            else bulletLeft--;
            Bullet bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            
            bullet.bulletSpeed = weaponBulletSpeed;
            bullet.bulletOwner = shooter;
            bullet.bulletDamage = weaponDamage;
            bullet.transform.rotation = transform.rotation;

            bullets.Add(bullet);
            timeToNextBullet = 0;
            //reloadingTime = 0f;
            readyToShoot = false;
            return true;
        }
        return false;
    }

    public void WeaponReset()
    {
        for(int i = 0; i < bullets.Count; i++)
        {
            if(bullets[i] != null)
            {
                Destroy(bullets[i].gameObject);
            }
        }
        bullets = new List<Bullet>();
        //bulletLeft = magCapacity;
        //weaponDamage = (int)shooter.m_ResetParams.GetWithDefault("weapon_damage", 10);
        bulletLeft = magCapacity;
        readyToShoot = true;
        reloadingTime = timeToReload;
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("blueAgent"))
        {
            other.GetComponent<ShooterAgent>().EquipWeapon(this);
        }
    }*/
    public void ChangeWeaponParameters(float rate, int capacity, int health, float speed)
    {
        magCapacity = capacity;
        fireRate = rate;
        GetComponentInParent<EnemyAgent>().Health = health;
        GetComponentInParent<EnemyAgent>().MoveSpeed = speed;
        //GetComponentInParent<Target>().ChangeHealth(health);
    }
}
