using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform groundCheckTransform = null;
    [SerializeField] private LayerMask playerMask;
    private bool jumpKeyPress;
    private float horizontalInput;
    private Rigidbody rigidBodyComp;
    private int superJumpRem;
    private int twiceJumpRem;
    private bool twiceJumped;


    void Start()
    {
        rigidBodyComp = GetComponent<Rigidbody>();
    }



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpKeyPress = true;
        }

        horizontalInput = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate()
    {
        rigidBodyComp.velocity = new Vector3(horizontalInput * 2, rigidBodyComp.velocity.y, 0);

        if (Physics.OverlapSphere(groundCheckTransform.position, 0.01f, playerMask).Length == 0)
        {
            if (twiceJumpRem > 0 & jumpKeyPress & !twiceJumped)
            {
                rigidBodyComp.AddForce(Vector3.up * 4, ForceMode.VelocityChange);
                twiceJumpRem--;
                jumpKeyPress = false;
                twiceJumped = true;
            }
            return;
        }

        if (jumpKeyPress == true)
        {
            rigidBodyComp.AddForce(Vector3.up * 5, ForceMode.VelocityChange);
            jumpKeyPress = false;
            twiceJumped = false;
            if (superJumpRem > 0)
            {
               rigidBodyComp.AddForce(Vector3.up, ForceMode.VelocityChange);
                superJumpRem--;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            Destroy(other.gameObject);
            superJumpRem++;
            twiceJumpRem++;
        }
        else if (other.gameObject.layer == 8)
        {
            transform.position = new Vector3(0, 2, 0);
            superJumpRem = 0;
            twiceJumpRem = 0;
        }
    }
}