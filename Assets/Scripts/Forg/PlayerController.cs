using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    public float jumpDistance;
    private float moveDistance;
    private bool buttonHeld;
    private Vector2 destination;
    private bool isJumpping;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        if (isJumpping)
            rb.position = Vector2.Lerp(transform.position, destination, 0.134f);

    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && !isJumpping)
        {
            //Debug.Log("JUMP! " + moveDistance);
            moveDistance = jumpDistance;
            destination = new Vector2(transform.position.x, transform.position.y + moveDistance);
            isJumpping = true;
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
            isJumpping = true;
        }
    }

    public void GetTouchPosition(InputAction.CallbackContext context)
    {

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (destination.y - transform.position.y < 0.1)
        {
            isJumpping = false;
        }
    }
}
