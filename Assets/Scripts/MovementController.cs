using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{

    public float speed;
    private float latestDirectionChangeTime;
    private readonly float directionChangeTime = 4f;
    private Vector3 movementDirection;
    private Vector3 movementPerSecond;
    public float headspeed;
    // Start is called before the first frame update
    void Start()
    {
        latestDirectionChangeTime = 0f;
        speed = 0.05f;
        CalculateNewMovementVector();
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - latestDirectionChangeTime > directionChangeTime)
        {
            latestDirectionChangeTime = Time.time;
            CalculateNewMovementVector();
        }

        this.transform.position = new Vector3(this.transform.position.x + (movementPerSecond.x * Time.deltaTime), this.transform.position.y, this.transform.position.z + (movementPerSecond.z * Time.deltaTime));
        this.transform.Find("Body").transform.Rotate(new Vector3(movementDirection.x , 0, movementDirection.z));
        if (this.transform.Find("Head").transform.rotation.z != this.transform.Find("Body").transform.rotation.z)
            this.transform.Find("Head").transform.Rotate(new Vector3(-0, 0, movementDirection.z));
        //this.transform.Find("Head").transform.Rotate(new Vector3(0, movementDirection.y * Time.deltaTime, 0));
    }

    private void CalculateNewMovementVector()
    {
        movementDirection = new Vector3(Random.Range(-1.0f, 1.0f), this.transform.position.y, Random.Range(-1.0f, 1.0f)).normalized;
        movementPerSecond = movementDirection * speed;
        movementPerSecond.y = this.transform.position.y;
    }

    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        if (collision.collider.tag == "Boundaries")
        {
            Debug.Log("Something was hit");
            latestDirectionChangeTime = Time.time;
            CalculateNewMovementVector();
        }

    }
}
