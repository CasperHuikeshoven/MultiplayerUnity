using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{

    [Header("GameObjects")]
    public CharacterController controller; 

    [Header("Speed")]
    float speed = 12f; 
    public float runSpeed = 12f; 
    public float normalSpeed = 6f; 

    [Header("Height")]
    public float crouchHeight = 0.5f; 
    public float normalHeight = 2f; 

    [Header("Gravity")]
    public float gravity = -9.81f; 
    public float jumpHeight = 10f; 
    public Transform groundCheck; 
    public float groundDistance = 0.4f; 
    public LayerMask groundMask; 
    bool isGrounded; 

    [Header("Key Inputs")]
    public string forwardKey;
    public string backKey;
    public string leftKey;
    public string rightKey;
    public string jumpKey;
    public string sprintKey;
    public string crouchKey;

    Vector3 velocity; 
    
    private void Update()
    {

        Movement();

    }

    private void Movement(){

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0){
            velocity.y = -2f; 
        }

        //Crouch
        if(!Input.GetKey(crouchKey)){
            controller.height = normalHeight;
        }
        if(Input.GetKey(crouchKey)){
            controller.height = normalHeight*crouchHeight;
        }

        //Sprint
        if(!Input.GetKey(sprintKey)){
            speed = normalSpeed;
        }
        if(Input.GetKey(sprintKey) && isGrounded){
            speed = runSpeed;
        }
    
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z; 

        controller.Move(move * speed * Time.deltaTime);

        if(Input.GetKey(jumpKey) && isGrounded){
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime; 
                
        controller.Move(velocity * Time.deltaTime);

    }
}
