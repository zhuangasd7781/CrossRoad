using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    public float jumpDistance;
    private float moveDistance;
    private bool buttonHeld;
    private Vector2 destination;
    private bool isJumpping;
    private bool canJump;

    void Start()
    {

    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        if (canJump)
        {
            TriggerJump();
        }
    }
    private void FixedUpdate()
    {
        if (isJumpping)
            rb.position = Vector2.Lerp(transform.position, destination, 0.134f);
    }

    #region INPUT ��J�^�ը��
    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && !isJumpping)
        {
            //Debug.Log("JUMP! " + moveDistance);
            moveDistance = jumpDistance;
            destination = new Vector2(transform.position.x, transform.position.y + moveDistance);
            canJump = true;
        }

    }
    public void LongJump(InputAction.CallbackContext context)
    {
        if (context.performed && !isJumpping)
        {
            moveDistance = jumpDistance * 2;
            buttonHeld = true;
        }

        if (context.canceled && buttonHeld && !isJumpping)
        {
            //Debug.Log("LONG JUMP! " + moveDistance);
            buttonHeld = false;
            destination = new Vector2(transform.position.x, transform.position.y + moveDistance);
            canJump = true;
        }
    }
    public void GetTouchPosition(InputAction.CallbackContext context)
    {

    }
    #endregion

    #region AnimationEvent �ʵe�ƥ�
    public void JumpAnimationEvent()
    {
        // ���ܪ��A
        isJumpping = true;
    }
    public void FinishJumpAnimationEvent()
    {
        isJumpping = false;
    }
    #endregion


    private void TriggerJump()
    {
        // to do ��o���ʤ�V ����ʵe
        canJump = false;
        anim.SetTrigger("Jump");
    }
    

}
