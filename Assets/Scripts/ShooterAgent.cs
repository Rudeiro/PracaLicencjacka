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

    public EnvironmentParameters m_ResetParams;
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
        //float forwardAmount = vectorAction[0];
        // Convert the second action to turning left or right
        float turnAmount = 0f;
        float upRotateAmount = 0f;
        float scanerRotation = 0f;
        if (vectorAction[0] == 1f)
        {
            turnAmount = -1f;
        }
        else if (vectorAction[0] == 2f)
        {
            turnAmount = 1f;
        }

        if (vectorAction[1] == 1f)
        {
            upRotateAmount = -1f;
        }
        else if (vectorAction[1] == 2f)
        {
            upRotateAmount = 1f;
        }

        if (vectorAction[2] == 1f)
        {
            scanerRotation = -1f;
        }
        else if (vectorAction[2] == 2f)
        {
            scanerRotation = 1f;
        }

        if (vectorAction[3] == 1f)
        {
            Shoot();
        }
        

        
        // Apply movement
        //rigidbody.MovePosition(transform.position + transform.forward * forwardAmount * moveSpeed* Time.fixedDeltaTime);
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
        actionsOut[0] = 0f;
        actionsOut[1] = 0f;
        actionsOut[3] = 0f;
        if (Input.GetKey(KeyCode.W))
        {
            // move forward
            actionsOut[0] = 1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            // turn left
            actionsOut[0] = 1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            // turn right
            actionsOut[0] = 2f;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            // turn left
            actionsOut[1] = 1f;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            // turn right
            actionsOut[1] = 2f;
        }

        if (Input.GetMouseButtonDown(0))
        {
            actionsOut[3] = 1f;
        }

        // Put the actions into an array and return
        //return new float[] { forwardAction, turnAction };
    }

    public override void OnEpisodeBegin()
    {
        worldArea.ResetArea();
        weaponHeld.weaponDamage = (int)m_ResetParams.GetWithDefault("weapon_damage", 10);
        weaponHeld.WeaponReset();
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

    private void FixedUpdate()
    {
        /* if (GetStepCount() % 5 == 0)
         {
             RequestDecision();
         }
         else
         {
             RequestAction();
         }
         if (agent.Units.Count >= 12) Done();*/
        //Debug.LogError(weaponHeld.ReadyToShoot);
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
        AddReward(-0.05f);
        //AddReward(-m_ResetParams.GetWithDefault("shoot_penalty", 0.0f));
        weaponHeld.Shoot();
    }

    

    
}
