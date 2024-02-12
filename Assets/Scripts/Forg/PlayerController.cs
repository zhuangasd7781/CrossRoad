using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public TerrainManager terrainManager;
    private enum Direction { Left, Right, Up }

    private Rigidbody2D rb;
    private Animator anim;
    public float jumpDistance;
    private float moveDistance;
    private bool buttonHeld;
    private bool isJumpping;
    private bool canJump;
    private SpriteRenderer sr;

    private Vector2 destination;
    private Vector2 touchPosition;
    private Direction dir;
    private bool isDead;

    private RaycastHit2D[] result = new RaycastHit2D[2];
    void Start()
    {

    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Water") && !isJumpping)
        {
            Physics2D.RaycastNonAlloc(transform.position + (Vector3.up * 0.1f), Vector2.zero, result);

            bool inWater = true;

            foreach (var hit in result)
            {
                if (hit.collider == null) continue;
                if (hit.collider.CompareTag("Wood"))
                {
                    //TODO:跟隨木板移動
                    //Debug.Log("在木板上");
                    transform.parent = hit.collider.transform;
                    inWater = false;
                }
            }

            //沒有木板 遊戲結束
            if (inWater && !isJumpping)
            {
                Debug.Log("In Water GameOver");
                isDead = true;
            }
        }

        if (collision.CompareTag("Border") || collision.CompareTag("Car"))
        {
            Debug.Log("Border Car Game over");
            isDead = true;
        }

        if (isJumpping == false && collision.CompareTag("Obstacle"))
        {
            Debug.Log("Obstacle Game over");
            isDead = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //if (collision.CompareTag("Wood"))
        //{
        //    transform.parent = null;
        //}
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

        //修改排序圖層
        sr.sortingLayerName = "Front";

        //
        transform.parent = null;

        //Debug.Log(dir);
        //switch (dir)
        //{
        //    case Direction.Up:
        //        
        //        destination = new Vector2(transform.position.x, transform.position.y + moveDistance);
        //        break;
        //    case Direction.Right:
        //        destination = new Vector2(transform.position.x + moveDistance, transform.position.y);
        //        break;
        //    case Direction.Left:
        //        destination = new Vector2(transform.position.x - moveDistance, transform.position.y);
        //        break;
        //}
    }
    public void FinishJumpAnimationEvent()
    {
        isJumpping = false;
        //修改排序圖層
        sr.sortingLayerName = "Middle";

        if (dir == Direction.Up && !isDead)
        {
            // to do 得分 + 地圖檢測
            terrainManager.CheckPosition();
        }
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
                anim.SetBool("isSide", false);
                destination = new Vector2(transform.position.x, transform.position.y + moveDistance);
                transform.localScale = Vector3.one;
                break;
            case Direction.Right:
                anim.SetBool("isSide", true);
                transform.localScale = new Vector3(-1, 1, 1);
                destination = new Vector2(transform.position.x + moveDistance, transform.position.y);
                break;
            case Direction.Left:
                anim.SetBool("isSide", true);
                destination = new Vector2(transform.position.x - moveDistance, transform.position.y);
                transform.localScale = Vector3.one;
                break;
        }

        anim.SetTrigger("Jump");
    }


}
