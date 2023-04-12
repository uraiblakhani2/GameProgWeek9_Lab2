using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public float speed = 5.0f;
    public float rotationSpeed = 200.0f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(0, 0, vertical);
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= speed;

        transform.position += moveDirection * Time.deltaTime;
        transform.Rotate(0, horizontal * rotationSpeed * Time.deltaTime, 0);
    }
}
