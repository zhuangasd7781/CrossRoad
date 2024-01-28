using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    public Vector2 startPos;
    private void Move()
    {
        transform.position += transform.right * speed * Time.deltaTime;
    }


    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(transform.position.x - startPos.x) > 15)
        {
            Destroy(this.gameObject);
        }
        Move();
    }
}
