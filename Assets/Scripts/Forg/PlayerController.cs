using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private enum Direction { Left, Right, Up }

    private Rigidbody2D rb;
    private Animator anim;
    public float jumpDistance;
    private float moveDistance;
    private bool buttonHeld;
    private bool isJumpping;
    private bool canJump;

    private Vector2 destination;
    private Vector2 touchPosition;
    private Direction dir;

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

    #region INPUT 輸入回調函數
    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && !isJumpping)
        {
            //Debug.Log("JUMP! " + moveDistance);
            moveDistance = jumpDistance;
            //destination = new Vector2(transform.position.x, transform.position.y + moveDistance);
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
            //destination = new Vector2(transform.position.x, transform.position.y + moveDistance);
            canJump = true;
        }
    }
    public void GetTouchPosition(InputAction.CallbackContext context)
    {
        // touchPosition = Camera.main.ScreenToWorldPoint(context.ReadValue<Vector2>());
        touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
        touchPosition = Camera.main.ScreenToWorldPoint(touchPosition);

        var offset = ((Vector3)touchPosition - transform.position).normalized;
        //Debug.Log(touchPosition);

        // 使用worldPosition进行你的逻辑处理
        if (Mathf.Abs(offset.x) <= 0.7f)
        {
            dir = Direction.Up;
        }
        else if (offset.x < 0)
        {
            dir = Direction.Left;
        }
        else if (offset.x > 0)
        {
            dir = Direction.Right;
        }

    }
    #endregion

    #region AnimationEvent 動畫事件
    public void JumpAnimationEvent()
    {
        // 改變狀態
        isJumpping = true;
        Debug.Log(dir);
        switch (dir)
        {
            case Direction.Up:
                destination = new Vector2(transform.position.x, transform.position.y + moveDistance);
                break;
            case Direction.Right:
                destination = new Vector2(transform.position.x + moveDistance, transform.position.y);
                break;
            case Direction.Left:
                destination = new Vector2(transform.position.x - moveDistance, transform.position.y);
                break;
        }
    }
    public void FinishJumpAnimationEvent()
    {
        isJumpping = false;
    }
    #endregion


    private void TriggerJump()
    {
        // to do 獲得移動方向 播放動畫
        canJump = false;

        switch (dir)
        {
            case Direction.Up:
                // to do : 觸發切換左右方向動畫
                destination = new Vector2(transform.position.x, transform.position.y + moveDistance);
                break;
            case Direction.Right:
                destination = new Vector2(transform.position.x + moveDistance, transform.position.y);
                break;
            case Direction.Left:
                destination = new Vector2(transform.position.x - moveDistance, transform.position.y);
                break;
        }

        anim.SetTrigger("Jump");
    }


}
