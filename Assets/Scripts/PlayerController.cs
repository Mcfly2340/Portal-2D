using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float movespeed, dirX, dirY;

    public float positionRadius;
    public float speed = 1;
    public float jumpForce;
    [SerializeField] private Collider2D playerCollider;
    [SerializeField] private LayerMask groundLayer;
    public static GameObject player;
    //facing direction player
    bool isFacingRight = true;

    public bool isCrouching = false;

    public float maxVelocity = 80;


    private static GameObject instance;

    // Start is called before the first frame update
    void Start()
    {
        DuplicationCorrection();
        //rb.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //rb.velocity = new Vector2(0, dirY);
        Jump();
        Movement();
    }

    void FixedUpdate()
    {
        this.rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxVelocity);
    }

    private void Movement()
    {
        dirX = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        rb.transform.Translate(dirX, 0, 0, 0);
        //dirY = Input.GetAxis("Jump") * Time.deltaTime * jumpForce;
        //if the player is moving right but player is facing left then:
        if (dirX > 0 && !isFacingRight)
        {
            flipPlayer();
        }
        //if the player is moving left but player is facing right then:
        else if (dirX < 0 && isFacingRight)
        {
            flipPlayer();
        }
        Crouching();
    }
    public void DuplicationCorrection()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = gameObject;
    }
    private void Crouching()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isCrouching = true;
            GetComponent<CapsuleCollider2D>().size = new Vector2(1, this.transform.localScale.y / 2);
            this.transform.localScale = new Vector3(1, this.transform.localScale.y / 2, 0.30f);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isCrouching = false;
            GetComponent<CapsuleCollider2D>().size = new Vector3(1, 2, 0.30f);
            this.transform.localScale = new Vector3(1, 1, 0.30f);
        }
    }
    private void Jump()
    {
        if (IsGrounded() && !isCrouching && Input.GetKey(KeyCode.Space))
        {
            rb.velocity = Vector3.up * jumpForce;
        }
    }
    private bool IsGrounded()
    {
        float extraHeight = .01f;
        RaycastHit2D raycastHit = Physics2D.Raycast(playerCollider.bounds.center, Vector2.down, playerCollider.bounds.extents.y + extraHeight, groundLayer);
        return raycastHit.collider != null;
    }
    void flipPlayer()
    {
        isFacingRight = !isFacingRight;

        transform.Rotate(0f, 180f, 0f);
    }
}
