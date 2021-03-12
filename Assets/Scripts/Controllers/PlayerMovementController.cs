using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

public class PlayerMovementController : NetworkedBehaviour
{
    [SerializeField]
    private float PlayerSpeed = 5f;
    [SerializeField]
    private float MouseCameraSensitivity = 3f;
    private CharacterController characterController;

    [SerializeField]
    private Transform cameraTransform;
    private float pitch = 0f;

    // Start is called before the first frame update
    void Start()
    {
        if (!IsLocalPlayer)
        {
            cameraTransform.TryGetComponent(out AudioListener audio); audio.enabled = false;
            cameraTransform.TryGetComponent(out Camera camera); camera.enabled = false;
        }
        else
        {
            TryGetComponent(out characterController);
            //Cursor.lockState = CursorLockMode.Locked;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (IsLocalPlayer)
        {
            MovePlayer();
            Look();
        }
      
    }

    void MovePlayer(){
        
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        move = Vector3.ClampMagnitude(move, 1f);
        move = transform.TransformDirection(move);
        characterController.SimpleMove(move * PlayerSpeed);
    }
    
    void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * MouseCameraSensitivity;
        transform.Rotate(0, mouseX, 0);
        pitch -= Input.GetAxis("Mouse Y") * MouseCameraSensitivity;
        pitch = Mathf.Clamp(pitch, -45f,45f);
        cameraTransform.localRotation = Quaternion.Euler(pitch, 0, 0);
    }
}
