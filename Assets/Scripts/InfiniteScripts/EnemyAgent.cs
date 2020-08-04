using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class EnemyAgent : Agent
{
    [SerializeField]
    float moveSpeed = 5;
    [SerializeField]
    float turnSpeed = 180;
    [SerializeField]
    float aimSpeed = 5;
    [SerializeField]
    Weapon weaponHeld;
    [SerializeField]
    int health;
    [SerializeField]
    InfiniteWorldArea worldArea;

    public float speed = 5;
    new private Rigidbody rigidbody;
    public int ownedHeals;
    int movementEnabled;
    private float distanceTraveled;

    public float DistanceTraveled { get { return distanceTraveled; } }
    public Weapon WeaponHeld { get { return weaponHeld; } }

    public EnvironmentParameters m_ResetParams;

    public int Health { get { return health; }  set { health = value; } }
    public float MoveSpeed { get { return moveSpeed; } set { moveSpeed = value; } }

    public override void Initialize()
    {
        base.Initialize();
        rigidbody = GetComponent<Rigidbody>();
        //worldArea = GetComponentInParent<InfiniteWorldArea>();
        m_ResetParams = Academy.Instance.EnvironmentParameters;
    }

    public override void OnActionReceived(float[] vectorAction)
    {

        // Convert the first action to forward movement
        float forwardAmount = 0;
        // Convert the second action to turning left or right
        float turnAmount = 0f;
        float sideAmount = 0f;

        //moving forward and backwards
        /*if (vectorAction[0] == 1f)
        {
            forwardAmount = 1f;
            //Debug.LogError("move forward");
        }*/

        //moving left and right
        if (vectorAction[0] == 1f)
        {
            sideAmount = -1f;
        }
        else if (vectorAction[0] == 2f)
        {
            sideAmount = 1f;
        }

        //rotating agent
        /*if (vectorAction[2] == 1f)
        {
            turnAmount = -1f;
        }
        else if (vectorAction[2] == 2f)
        {
            turnAmount = 1f;
        }*/

        if (vectorAction[1] == 1f)
        {
            Shoot();
        }        

        if (vectorAction[2] == 1f)
        {
            Heal(30);
        }

        // Apply movement
        rigidbody.MovePosition(transform.position + (transform.forward * forwardAmount * moveSpeed + transform.right * sideAmount * moveSpeed) * Time.fixedDeltaTime);
        
        //rigidbody.MovePosition(transform.position * Time.fixedDeltaTime);
        transform.Rotate(transform.up * turnAmount * turnSpeed * Time.fixedDeltaTime);
        


        // Apply a tiny negative reward every step to encourage action
        // AddReward(-0.0005f);
    }

    public override void Heuristic(float[] actionsOut)
    {
        //actionsOut[0] = 0f; // moving forward/backward
        actionsOut[0] = 0f; // moving left/right
        //actionsOut[2] = 0f; // rotating agent
        actionsOut[1] = 0f; // shooting
        actionsOut[2] = 0f; // healing
        // moving sideways
        /*if (Input.GetKey(KeyCode.W))
        {
            // move forward
            actionsOut[0] = 1f;
        }*/

        if (Input.GetKey(KeyCode.A))
        {
            // move left
            actionsOut[0] = 1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            // move right
            actionsOut[0] = 2f;
        }

       /* if (Input.GetKey(KeyCode.Q))
        {
            // turn left
            actionsOut[2] = 1f;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            // turn right
            actionsOut[2] = 2f;
        }*/

        if (Input.GetMouseButton(0))
        {
            actionsOut[1] = 1f;
        }

       /* if (Input.GetKey(KeyCode.H))
        {
            actionsOut[2] = 1f;
        }*/



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
        //worldArea.ResetArea();
        distanceTraveled = 0;
        ownedHeals = 0;
        moveSpeed = 2;
        GetComponent<DecisionRequester>().DecisionPeriod = (int)m_ResetParams.GetWithDefault("decisions", 5);
        health = (int)m_ResetParams.GetWithDefault("health", 60);
        //worldArea.ResetArea();
        weaponHeld.WeaponReset();
    }

    private void FixedUpdate()
    {
       /* if (worldArea.Spawner.StopSpawning == false)
        {
            distanceTraveled += Time.fixedDeltaTime * speed;
        }*/
       /* if(transform.position.z < 6.5f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + speed * Time.fixedDeltaTime);
        }*/
    }

    public override void CollectObservations(VectorSensor sensor)
    {

        //sensor.AddObservation(worldArea.targetsCount);

        //sensor.AddObservation(health);
        sensor.AddObservation(weaponHeld.TimeToReload);
        sensor.AddObservation(weaponHeld.bulletLeft);
       // sensor.AddObservation(ownedHeals);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("reward"))
        {
            AddReward(0.1f);
        }
        if (other.gameObject.CompareTag("firstAid"))
        {
            //AddReward(0.1f);
            if(ownedHeals < 3)
            {
                AddReward(0.05f);
                ownedHeals++;
                Destroy(other.gameObject);
            }
        }
    }

    private void Shoot()
    {
        weaponHeld.Shoot();        
    }

    public void DealDamage(int amount, InfinityAgent enemy)
    {
        health -= amount;
        AddReward(-1.0f*amount/100);
        enemy.AddReward(0.03f);
        if (health <= 0)
        {
            /* AddReward(-1f);
             enemy.AddReward(1f);
             enemy.EndEpisode();
             EndEpisode();*/
            enemy.AddReward(0.1f);
            GetComponentInParent<InfiniteSpawner>().ResumeSpawning();
            Destroy(transform.gameObject);
        }
    }

    private void Heal(int amount)
    {
        if (ownedHeals > 0 && health < 100)
        {
            AddReward(Mathf.Min(amount, 100 - health) / 100);
            health += amount;
            health = Mathf.Min(100, health);
            ownedHeals--;
        }
    }





}
