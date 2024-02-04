using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    public Vector2 startPos;
    public int dir;
    private void Start()
    {
        startPos = transform.position;
        transform.localScale = new Vector3(dir, 1, 1);
    }

    private void Move()
    {
        transform.position += transform.right * dir * speed * Time.deltaTime;
    }


    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(transform.position.x - startPos.x) > 25)
        {
            Destroy(this.gameObject);
        }
        Move();
    }
}
