using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class MouseLook : NetworkBehaviour
{
    [Header("Mouse Sensitivity")]
    public float mouseSensitivity = 100f;

    [Header("GameObjects")]
    public Transform playerBody; 

    float xRotation = 0f;

    public GameObject head; 
    // Update is called once per frame
    void Update()
    {
        
        if(hasAuthority && SceneManager.GetActiveScene().name == "Game"){
            CameraLook();
        }
        
    }

    public void CameraLook(){
        Cursor.lockState = CursorLockMode.Locked;
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        xRotation -= mouseY; 
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        head.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        playerBody.Rotate(Vector3.up * mouseX);        
    }
}
