using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScpMovimento : MonoBehaviour
{

    private CharacterController characterController;
    private Transform myCamera;
    private float speed = 5f;
    private Animator animator; 

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        myCamera = Camera.main.transform;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //conntroles "wasd" e setas
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");


        Vector3 movimento = new Vector3(horizontal, 0, vertical);

        //Pra frente é referente a camerra e não ao global 
        movimento = myCamera.TransformDirection(movimento);
        movimento.y = 0;

        characterController.Move(movimento * Time.deltaTime * speed);
        //Gravidade
        characterController.Move(new Vector3(0, -9.81f, 0) * Time.deltaTime);

        //rotação do personagem
        if (movimento != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movimento), Time.deltaTime * 10);
        }

        //animacao teste
        animator.SetBool("correndo", movimento != Vector3.zero);

    }
}
