using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Animators")]
    [Space]
    public Animator PlayerAnim;

    [Header("Booleans")]
    [Space]
    bool isFacingRight = true;
    public static bool isCrawling = false;
    public static bool canCrawl = false;
    public bool isSprinting = false;
    public static bool canSprint = false;

    [Header("Floats")]
    [Space]
    public float moveSpeed;
    public float dirX, dirY;
    public float positionRadius;
    public float speed;
    public float jumpForce;
    public float maxVelocity = 80f;

    [Header("Game Objects")]
    [Space]
    public static GameObject player;
    public GameObject playerSpawnPos;
    private static GameObject instance;

    [Header("Integers")]
    [Space]
    public int multiplier = 1;

    [Header("Rigidbody's")]
    [Space]
    public Rigidbody2D rb;

    [Header("-Others-")]
    [Space]
    [SerializeField] private Collider2D playerCollider;
    [SerializeField] private LayerMask groundLayer;
    
    void Start()
    {
        DuplicationCorrection();
    }
    void Update()
    {
        if (StartCutscene.isInCutscene == false)
        {
            if(Machine.isStandingStill == false)
            {
                Jump();
                Movement();
            }
            
        }

        PlayerAnim.SetFloat("Speed", Mathf.Abs(dirX));
    }
    void FixedUpdate()
    {//max velocity
        this.rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxVelocity);
    }
    private void Movement()
    {
        //General Movement (left or right)
        dirX = Input.GetAxis("Horizontal") * Time.deltaTime * speed * multiplier;

        if(canSprint)Sprinting();
        

        //FLIPPING PLAYER
        //if the player is moving right but player is facing left then:
        if (dirX > 0)
        {
            if (!isFacingRight)flipPlayer();
        }
        //if the player is moving left but player is facing right then:
        else if (dirX < 0)
        {
            if (isFacingRight)flipPlayer();
        }

        if (canCrawl)
        {
            Crawling();
        }
        if (isCrawling == true)
        {//walk slow when crouching
            rb.transform.Translate(dirX / 5, 0, 0, 0);
            //if not crouching then walk normal
        }else rb.transform.Translate(dirX, 0, 0, 0);
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
    private void Sprinting()
    {
        if (Input.GetKey(KeyCode.LeftControl) && !isSprinting && !isCrawling)
        {
            isSprinting = true;
            PlayerAnim.SetBool("issprinting", true);
            //is sprinting walk so faster
            multiplier = 2;
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl) && isSprinting && !isCrawling)
        {
            isSprinting = false;
            PlayerAnim.SetBool("issprinting", false);
            //walk normal
            multiplier = 1;
        }
        
    }
    private void Crawling()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isCrawling)
        {
            isCrawling = true;
            GetComponent<CapsuleCollider2D>().size = new Vector3(GetComponent<CapsuleCollider2D>().size.x, GetComponent<CapsuleCollider2D>().size.y / 2, 0.30f);
            PlayerAnim.SetBool("iscrawling", true);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) && isCrawling == true)
        {
            isCrawling = false;
            GetComponent<CapsuleCollider2D>().size = new Vector3(GetComponent<CapsuleCollider2D>().size.x, GetComponent<CapsuleCollider2D>().size.y * 2, 0.30f);
            PlayerAnim.SetBool("iscrawling", false);
        }
    }
    public void Jump()
    {
        if (IsGrounded() && !isCrawling && Input.GetKey(KeyCode.Space))
        {//for jumping
            PlayerAnim.SetBool("isjumping", true);
            rb.velocity = Vector3.up * jumpForce;
        }
        if (!IsGrounded() && rb.velocity.y <= -2)
        {//for falling
            PlayerAnim.SetBool("isjumping", true);
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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //for jumping
        if (collision.transform.gameObject.tag == "Up & Down")
        {
            if (rb.velocity.y <= 0.5f && rb.velocity.y >= -0.5f) PlayerAnim.SetBool("isjumping", false);
        }
    }
}
