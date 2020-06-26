using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;


public class ShooterAgent : Agent
{
    [SerializeField]
    float moveSpeed = 5;
    [SerializeField]
    float turnSpeed = 180;
    [SerializeField]
    float upRotateSpeed = 180;
    [SerializeField]
    float aimSpeed = 5;
    [SerializeField]
    Weapon weaponHeld;
    [SerializeField]
    GameObject scaner;    
    [SerializeField]
    int health;
    [SerializeField]
    Weapon weaponPrefab;


    private bool hasWeapon = false;
    public bool canPickUp;
    public GameObject pickUpObject;
    new private Rigidbody rigidbody;
    private WorldArea worldArea;
    public int ownedHeals;
    int movementEnabled;

    
    public EnvironmentParameters m_ResetParams;

    public int Health { get { return health; } }

    public override void Initialize()
    {
        base.Initialize();        
        rigidbody = GetComponent<Rigidbody>();
        worldArea = GetComponentInParent<WorldArea>();
        m_ResetParams = Academy.Instance.EnvironmentParameters;
    }

    public override void OnActionReceived(float[] vectorAction)
    {      

        // Convert the first action to forward movement
        float forwardAmount = 0;
        // Convert the second action to turning left or right
        float turnAmount = 0f;
        float upRotateAmount = 0f;
        float scanerRotation = 0f;
        float sideAmount = 0f;

        //moving forward and backwards
        if (vectorAction[0] == 1f)
        {
            forwardAmount = 1f;
            //Debug.LogError("move forward");
        }
        else if (vectorAction[0] == 2f)
        {
            //Debug.LogError("move backwards");
            forwardAmount = -0.5f;
        }

        //moving left and right
        if (vectorAction[1] == 1f)
        {
            sideAmount = -0.5f;
        }
        else if (vectorAction[1] == 2f)
        {
            sideAmount = 0.5f;
        }

        //rotating agent
        if (vectorAction[2] == 1f)
        {
            turnAmount = -1f;
        }
        else if (vectorAction[2] == 2f)
        {
            turnAmount = 1f;
        }

        if (vectorAction[3] == 1f)
        {
            Shoot();
            forwardAmount = 0;
            sideAmount = 0;
            turnAmount = 0;
        }

        if(vectorAction[4] == 1f)
        {
            PickUp();
        }

        if (vectorAction[5] == 1f)
        {
            Heal(30);
        }

        //rotating weapon up and down
        /*if (vectorAction[1] == 1f)
        {
            upRotateAmount = -1f;
        }
        else if (vectorAction[1] == 2f)
        {
            upRotateAmount = 1f;
        } */

        //shooting




        forwardAmount *= movementEnabled;
        sideAmount *= movementEnabled;

        if(forwardAmount != 0 || sideAmount != 0)
        {
            AddReward(-m_ResetParams.GetWithDefault("move_penalty", 0));
        }
        if (turnAmount != 0)
        {
            AddReward(-m_ResetParams.GetWithDefault("rotate_penalty", 0));
        }

        // Apply movement
        rigidbody.MovePosition(transform.position + (transform.forward * forwardAmount * moveSpeed + transform.right * sideAmount * moveSpeed )* Time.fixedDeltaTime);
        //rigidbody.MovePosition(transform.position * Time.fixedDeltaTime);
        transform.Rotate(transform.up * turnAmount * turnSpeed * Time.fixedDeltaTime);
        //sDebug.LogError(weaponHeld.transform.eulerAngles.x);
        /*if (upRotateAmount > 0 && (weaponHeld.transform.eulerAngles.x < 3 || weaponHeld.transform.eulerAngles.x > 315))
        {

            weaponHeld.transform.Rotate(upRotateAmount * upRotateSpeed * Time.fixedDeltaTime, 0, 0);
        }
        else
        if (upRotateAmount < 0 && (weaponHeld.transform.eulerAngles.x > 320 || weaponHeld.transform.eulerAngles.x < 5))
        {
            weaponHeld.transform.Rotate(upRotateAmount * upRotateSpeed * Time.fixedDeltaTime, 0, 0);
        }*/
        

        // Apply a tiny negative reward every step to encourage action
       // AddReward(-0.0005f);
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = 0f; // moving forward/backward
        actionsOut[1] = 0f; // moving left/right
        actionsOut[2] = 0f; // rotating agent
        actionsOut[3] = 0f; // shooting
        actionsOut[4] = 0f; // picking up objects
        actionsOut[5] = 0f; // healing
        // moving sideways
        if (Input.GetKey(KeyCode.W))
        {
            // move forward
            actionsOut[0] = 1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            // move backwards
            actionsOut[0] = 2f;
        }
        

        if (Input.GetKey(KeyCode.A))
        {
            // move left
            actionsOut[1] = 1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            // move right
            actionsOut[1] = 2f;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            // turn left
            actionsOut[2] = 1f;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            // turn right
            actionsOut[2] = 2f;
        }

        if (Input.GetMouseButton(0))
        {
            actionsOut[3] = 1f;
        }

        if (Input.GetKey(KeyCode.F))
        {
            actionsOut[4] = 1f;
        }

        if (Input.GetKey(KeyCode.H))
        {
            actionsOut[5] = 1f;
        }



        /*if (Input.GetKey(KeyCode.A))
        {
            // move left
            actionsOut[5] = 1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            // move right
            actionsOut[5] = 2f;
        }*/

        // Put the actions into an array and return
        //return new float[] { forwardAction, turnAction };
    }

    public override void OnEpisodeBegin()
    {
        movementEnabled = (int)m_ResetParams.GetWithDefault("movement_enabled", 1);
        worldArea.ResetArea();
        hasWeapon = false;
        if (weaponHeld != null)
        {
            Destroy(weaponHeld.gameObject);
            weaponHeld = null;
        }
        if((int)m_ResetParams.GetWithDefault("weapon_enabled", 0) == 1)
        {
            EquipWeapon(Instantiate(weaponPrefab));
        }
        ownedHeals = 0;
        health = 20;
    }

    

    public override void CollectObservations(VectorSensor sensor)
    {

        sensor.AddObservation(worldArea.targetsCount);
        sensor.AddObservation(transform.forward);
        sensor.AddObservation(health);
        sensor.AddObservation(ownedHeals);

        sensor.AddObservation(hasWeapon);
        sensor.AddObservation(weaponHeld == null ? false : weaponHeld.ReadyToShoot);
        sensor.AddObservation(weaponHeld == null ? 0 : weaponHeld.TimeToReload);
        sensor.AddObservation(weaponHeld == null ? 0 : weaponHeld.ReloadingTime);
        sensor.AddObservation(weaponHeld == null ? 0 :weaponHeld.BulletLeft);

    }

    private void Start()
    {
        worldArea = GetComponentInParent<WorldArea>();
    }

    

    private void OnCollisionEnter(Collision collision)
    {
        /*if (collision.transform.CompareTag("Building"))
        {
            SpawnUnit(collision.gameObject.GetComponent<Building>());
        }*/
        
    }   

    private void Shoot()
    {
        //AddReward(-0.05f);
        //AddReward(-m_ResetParams.GetWithDefault("shoot_penalty", 0.0f));
        if (hasWeapon && weaponHeld != null)
        {
            weaponHeld.Shoot();
        }
    }

    public void DealDamage(int amount, ShooterAgent enemy)
    {
        health -= amount;
        enemy.AddReward(1.0f);
        if (health <= 0)
        {
            //Debug.LogError("killed");
            enemy.AddReward(10f);
            AddReward(-10f);
            enemy.EndEpisode();
            EndEpisode();
        }

    }

    private void PickUp()
    {
        
        if (canPickUp)
        {
            canPickUp = false;
            Item item = pickUpObject.GetComponent<Item>();
            switch (item.Type)
            {
                case Item.ItemType.pistol:
                    EquipWeapon(pickUpObject.GetComponent<Weapon>());                    
                    break;
                case Item.ItemType.firstAid:
                    if (ownedHeals < 5)
                    {
                        AddReward(2f);
                        ownedHeals++;
                        Destroy(pickUpObject);
                        canPickUp = false;
                    }
                    break;
                default:
                    break;
            }
        }
    }

    private void EquipWeapon(Weapon weapon)
    {
        if(weaponHeld == null)
        {
            AddReward(5f);
            ItemSpawner itemSpawner = weapon.GetComponentInParent<ItemSpawner>();
            if (itemSpawner != null)
            {
                itemSpawner.Item = null;
            }
            weaponHeld = weapon;
            weaponHeld.transform.parent = transform;
            weaponHeld.transform.localPosition = new Vector3(0.65f, 0, 0);
            weaponHeld.transform.eulerAngles = transform.eulerAngles;            
            weaponHeld.shooter = this;
            weaponHeld.GetComponent<BoxCollider>().enabled = false;
            weaponHeld.WeaponReset();
            pickUpObject = null;
            hasWeapon = true;
        }
    }

    public void EnablePickUp(GameObject obj, bool enabled)
    {
        pickUpObject = obj;
        canPickUp = enabled;
    }

    private void Heal(int amount)
    {
        if (ownedHeals > 0 && health < 100)
        {
            AddReward(Mathf.Min(amount, 100 - health) / 10);
            health += amount;
            ownedHeals--;
        }
    }
}
