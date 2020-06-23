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

    new private Rigidbody rigidbody;
    private WorldArea worldArea;
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

        //rotating agent
        if (vectorAction[0] == 1f)
        {
            turnAmount = -1f;
        }
        else if (vectorAction[0] == 2f)
        {
            turnAmount = 1f;
        }

        //rotating weapon up and down
        if (vectorAction[1] == 1f)
        {
            upRotateAmount = -1f;
        }
        else if (vectorAction[1] == 2f)
        {
            upRotateAmount = 1f;
        }

        //rotating agents rayPerception3d
       /* if (vectorAction[2] == 1f)
        {
            scanerRotation = -1f;
        }
        else if (vectorAction[2] == 2f)
        {
            scanerRotation = 1f;
        }*/

        //shooting
        if (vectorAction[3] == 1f)
        {
            Shoot();
        }

        //moving forward and backwards
        if (vectorAction[4] == 1f)
        {
            forwardAmount = 1f;
            //Debug.LogError("move forward");
        }
        else if (vectorAction[4] == 2f)
        {
            //Debug.LogError("move backwards");
            forwardAmount = -0.5f;
        }

        //moving left and right
        if (vectorAction[5] == 1f)
        {
            sideAmount = -0.5f;
        }
        else if (vectorAction[5] == 2f)
        {
            sideAmount = 0.5f;
        }

        forwardAmount *= movementEnabled;
        sideAmount *= movementEnabled;

        // Apply movement
        rigidbody.MovePosition(transform.position + (transform.forward * forwardAmount * moveSpeed + transform.right * sideAmount * moveSpeed )* Time.fixedDeltaTime);
        //rigidbody.MovePosition(transform.position * Time.fixedDeltaTime);
        transform.Rotate(transform.up * turnAmount * turnSpeed * Time.fixedDeltaTime);
        //sDebug.LogError(weaponHeld.transform.eulerAngles.x);
        if (upRotateAmount > 0 && (weaponHeld.transform.eulerAngles.x < 3 || weaponHeld.transform.eulerAngles.x > 315))
        {

            weaponHeld.transform.Rotate(upRotateAmount * upRotateSpeed * Time.fixedDeltaTime, 0, 0);
        }
        else
        if (upRotateAmount < 0 && (weaponHeld.transform.eulerAngles.x > 320 || weaponHeld.transform.eulerAngles.x < 5))
        {
            weaponHeld.transform.Rotate(upRotateAmount * upRotateSpeed * Time.fixedDeltaTime, 0, 0);
        }

        if (scanerRotation > 0 && (scaner.transform.eulerAngles.x < 3 || scaner.transform.eulerAngles.x > 315))
        {

            scaner.transform.Rotate(scanerRotation * upRotateSpeed * Time.fixedDeltaTime, 0, 0);
        }
        else
        if (scanerRotation < 0 && (scaner.transform.eulerAngles.x > 320 || scaner.transform.eulerAngles.x < 5))
        {
            scaner.transform.Rotate(scanerRotation * upRotateSpeed * Time.fixedDeltaTime, 0, 0);
        }

        // Apply a tiny negative reward every step to encourage action
        AddReward(-0.0005f);
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = 0f; // rotating agent
        actionsOut[1] = 0f; // rotating weapon
        actionsOut[3] = 0f; // shooting
        actionsOut[4] = 0f; // moving forward and backwards
        actionsOut[5] = 0f; // moving sideways
        if (Input.GetKey(KeyCode.Q))
        {
            // move forward
            actionsOut[0] = 1f;
        }
        if (Input.GetKey(KeyCode.E))
        {
            // turn left
            actionsOut[0] = 2f;
        }
        

        if (Input.GetKey(KeyCode.Alpha1))
        {
            // turn left
            actionsOut[1] = 1f;
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            // turn right
            actionsOut[1] = 2f;
        }

        if (Input.GetMouseButtonDown(0))
        {
            actionsOut[3] = 1f;
        }

        if (Input.GetKey(KeyCode.W))
        {
            // move forward
            actionsOut[4] = 1f;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            // move backward
            actionsOut[4] = 2f;
        }

        if (Input.GetKey(KeyCode.A))
        {
            // move left
            actionsOut[5] = 1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            // move right
            actionsOut[5] = 2f;
        }

        // Put the actions into an array and return
        //return new float[] { forwardAction, turnAction };
    }

    public override void OnEpisodeBegin()
    {
        movementEnabled = (int)m_ResetParams.GetWithDefault("movement_enabled", 1);
        worldArea.ResetArea();
        weaponHeld.weaponDamage = (int)m_ResetParams.GetWithDefault("weapon_damage", 10);
        weaponHeld.WeaponReset();
        health = 100;
    }

    

    public override void CollectObservations(VectorSensor sensor)
    {
        // Direction penguin is facing (1 Vector3 = 3 values)
       
        sensor.AddObservation(transform.forward);

        sensor.AddObservation(weaponHeld.ReadyToShoot);
        sensor.AddObservation(weaponHeld.TimeToReload);
        sensor.AddObservation(weaponHeld.ReloadingTime);
        sensor.AddObservation(weaponHeld.BulletLeft);

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
        weaponHeld.Shoot();
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


}
