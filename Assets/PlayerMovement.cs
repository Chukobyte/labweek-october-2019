using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rigidbody;
    public GameObject ground;
    public ObstacleSpawner obstacleSpawner;
    public float player_z_distance_spawn_min = 0.1f;
    public bool onGround = false;
    public Vector3 startPosition = new Vector3(0, -1, 30);
    public float forwardForce = 2000f;
    public float sidewaysForce = 200f;
    public float accelForce = 200f;
    public float jumpForce = 0.01f;

    public float zMovementCounter = 0.0f;
    private float previousZPosition = 0.0f;

    public bool playSessionActive = false;
    public float runTime = 0.0f;
    public Text timerText;    
    public float fastestTime = 0.0f;
    public Text fastestTimeText;

    // Start is called before the first frame update
    void Start()
    {
        obstacleSpawner = ground.GetComponent<ObstacleSpawner>();
        rigidbody.useGravity = true;
        transform.position = startPosition;
        previousZPosition = transform.position.z;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(playSessionActive)
        {
            runTime += Time.deltaTime;
            UpdateTimeText();
            zMovementCounter = Mathf.Abs(transform.position.z - previousZPosition) * 2;
            previousZPosition = transform.position.z;
            //Debug.Log("zMovementCounter = " + zMovementCounter);
            //Debug.Log("player_z_distance_spawn_min = " + player_z_distance_spawn_min);
            if (zMovementCounter >= player_z_distance_spawn_min)
            {
                Debug.Log("TEST");
                zMovementCounter = 0.0f;
                obstacleSpawner.spawnObstacle = true;
            }
        }
        // Restart
        if (Input.GetKeyDown("r"))
        {
            RestartGame();
        }

        if (Input.GetKeyDown("space") && onGround)
        {
            rigidbody.velocity += new Vector3(0, jumpForce * 2.5f * Time.deltaTime, 0);
            onGround = false;
        }

        rigidbody.AddForce(0, 0, forwardForce * Time.deltaTime);
        rigidbody.AddForce(GetInputVelocity());

        //Restart if player fell off
        if(transform.position.y <= -30.0f)
        {
            RestartGame();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name.Equals("Ground"))
        {
            onGround = true;
            if (!playSessionActive)
            {
                playSessionActive = true;
            }
        }
        else if (collision.collider.name.Equals("Finish Line")) {
            if (runTime < fastestTime || fastestTime <= 0.0f)
            {
                fastestTime = runTime;
                UpdateFastestTimeText();
            }
            playSessionActive = false;
        }
        Debug.Log(collision.collider.name);
    }

    Vector3 GetInputVelocity()
    {
        Vector3 movementVelocity = new Vector3(0.0f, 0.0f, 0.0f);
        // Four Directional Movement
        if (Input.GetKey("d"))
        {
            movementVelocity += new Vector3(sidewaysForce* Time.deltaTime, 0, 0);
        }
        if (Input.GetKey("a"))
        {
            movementVelocity += new Vector3(-sidewaysForce * Time.deltaTime, 0, 0);
        }
        //if (Input.GetKey("w"))
        //{
        //    movementVelocity += new Vector3(0, 0, -accelForce * Time.deltaTime);
        //}
        //if (Input.GetKey("s"))
        //{
        //    movementVelocity += new Vector3(0, 0, accelForce * Time.deltaTime);
        //}
        return movementVelocity;
    }

    private void UpdateTimeText()
    {
        timerText.text = "Time: " + System.Math.Round(runTime, 2).ToString();
    }

    private void UpdateFastestTimeText()
    {
        fastestTimeText.text = "Record: " + System.Math.Round(fastestTime, 2).ToString();
    }

    private void RestartGame()
    {
        rigidbody.Sleep();
        transform.rotation = new Quaternion(0, 0, 0, 0);
        transform.position = startPosition;
        obstacleSpawner.ResetSpawner();
        obstacleSpawner.spawnObstacle = false;
        playSessionActive = false;
        previousZPosition = transform.position.z;
        zMovementCounter = 0.0f;
        runTime = 0.0f;
        UpdateTimeText();
    }
}
