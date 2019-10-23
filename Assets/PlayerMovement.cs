using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rigidbody;
    public GameObject ground;
    public ObstacleSpawner obstacleSpawner;
    public bool onGround = false;
    public Vector3 startPosition = new Vector3(0, -1, 30);
    public float forwardForce = 2000f;
    public float sidewaysForce = 200f;
    public float accelForce = 200f;
    public float jumpForce = 0.01f;

    public bool playSessionActive = false;
    public float runTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody.useGravity = true;
        transform.position = startPosition;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(playSessionActive)
        {
            runTime += Time.deltaTime;
            Debug.Log("run time = " + runTime);
        }
        // Restart
        if (Input.GetKeyDown("r"))
        {
            rigidbody.Sleep();
            transform.rotation = new Quaternion(0, 0, 0, 0);
            transform.position = startPosition;
            obstacleSpawner = ground.GetComponent<ObstacleSpawner>();
            obstacleSpawner.ResetSpawner();
            playSessionActive = false;
        }

        if (Input.GetKeyDown("space") && onGround)
        {
            rigidbody.velocity += new Vector3(0, jumpForce * 2.5f * Time.deltaTime, 0);
            onGround = false;
        }

        rigidbody.AddForce(0, 0, forwardForce * Time.deltaTime);
        rigidbody.AddForce(GetInputVelocity());
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
}
