using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float movespeed, dirX, dirY;

    public float positionRadius;
    public float speed = 1;
    public LayerMask ground;
    public Transform playerPos;
    public float jumpForce;
    public Vector2 jumpHeight;
    private bool isOnGround;
    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = new Vector3(0, 0, 0);
        //rb.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //rb.velocity = new Vector2(0, dirY);
        Jump();
        Movement();
    }

    private void Movement()
    {
        dirX = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        rb.transform.Translate(dirX, 0, 0, 0);
        //dirY = Input.GetAxis("Jump") * Time.deltaTime * jumpForce;
    }
    private void FixedUpdate()
    {
    }
    private void Jump()
    {
        isOnGround = Physics2D.OverlapCircle(playerPos.position, positionRadius, ground);
        if (isOnGround && Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = Vector2.up * jumpForce;
        }
    }
}
