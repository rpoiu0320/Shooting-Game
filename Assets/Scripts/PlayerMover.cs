using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMover : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 moveDir;
    private float ySpeed = 0;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float JumpSpeed;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Move();
        Jump();
    }

    private void Move()
    {
        // 월드기준 움직임
        // controller.Move(moveDir * moveSpeed * Time.deltaTime); 

        // 로컬기준 움직임
        controller.Move(transform.forward * moveDir.z * moveSpeed * Time.deltaTime);
        controller.Move(transform.right * moveDir.x * moveSpeed * Time.deltaTime);
    }

    private void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        moveDir = new Vector3(input.x, 0, input.y);
    }

    private void Jump()
    {
        ySpeed += Physics.gravity.y * Time.deltaTime;

        /* if(characterController.isGrounded)   // isGrounded가 정교하지 않아 사용을 추천하지 않음
            ySpeed = 0;
        */

        if (GroundCheck() && ySpeed < 0)
            ySpeed = -1;

        // if(GroundCheck())
            controller.Move(Vector3.up * ySpeed * Time.deltaTime);
    }

    private void OnJump(InputValue value)
    {
        if (GroundCheck())
            ySpeed = JumpSpeed;
    }

    private bool GroundCheck()
    {
        RaycastHit hit; // 2D랑 3D랑 살짝 다름
        return Physics.SphereCast(transform.position + Vector3.up * 1, 0.5f, Vector3.down, out hit, 0.6f);
        //                          쏠 위치,                         쏘는 둘레,  쏘는 방향,   out 파라미터, 쏘는 길이
    }
}
