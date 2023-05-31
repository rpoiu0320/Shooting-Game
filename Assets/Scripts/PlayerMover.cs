using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

// Transform, Rigidbody, Character controller 각 상황에 맞게 오브젝트의 이동 구현

public class PlayerMover : MonoBehaviour
{
    private Animator animator;
    private CharacterController controller;
    private Vector3 moveDir;
    private float ySpeed = 0;
    private float moveSpeed = 0;
    private bool isWarking;

    [SerializeField] private float runSpeed;
    [SerializeField] private float warkSpeed;
    [SerializeField] private float JumpSpeed;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
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

        if(moveDir.magnitude == 0)      // 안움직임
        {
            moveSpeed = Mathf.Lerp(moveSpeed, 0, 0.25f);   // 선형보간
        }
        else if(isWarking)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, warkSpeed, 0.25f);
        }
        else
        {
            moveSpeed = Mathf.Lerp(moveSpeed, runSpeed, 0.25f);
        }

        // 로컬기준 움직임
        controller.Move(transform.forward * moveDir.z * moveSpeed * Time.deltaTime);
        controller.Move(transform.right * moveDir.x * moveSpeed * Time.deltaTime);

        animator.SetFloat("XSpeed", moveDir.x, 0.25f, Time.deltaTime);
        animator.SetFloat("YSpeed", moveDir.z, 0.25f, Time.deltaTime);
        animator.SetFloat("Speed", moveSpeed);
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

    private void OnWork(InputValue value)
    {
        isWarking = value.isPressed;
    }
}
