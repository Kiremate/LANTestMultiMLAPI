using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

public class PlayerMovement : NetworkedBehaviour
{
    [SerializeField]
    private float PlayerSpeed = 5f;
    private CharacterController cc;



    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent<CharacterController>(out cc);
    }

    // Update is called once per frame
    void Update()
    {
        if(IsLocalPlayer)
            MovePlayer();
    }

    void MovePlayer(){
        
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        move = Vector3.ClampMagnitude(move, 1f);

        cc.SimpleMove(move * PlayerSpeed);
    }
    
}
