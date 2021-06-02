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
    public ParticleSystem Dust;
    public ParticleSystem SlamDust;
    [SerializeField] private Collider2D playerCollider;
    [SerializeField] private LayerMask groundLayer;
    
    void Update()
    {
        DebugControlls();
        //if in cutscene should not move
        if (StartCutscene.isInCutscene == false)
        {
            if(Machine.shouldNotBeMoving == false)
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
        {//walk slow when crawling
            rb.transform.Translate(dirX / 5, 0, 0, 0);
            //if not crawling then walk normal
        }
        else rb.transform.Translate(dirX, 0, 0, 0);
    }
    private void Sprinting()
    {
        if (Input.GetKey(KeyCode.LeftControl) && !isSprinting && !isCrawling)
        {
            isSprinting = true;
            PlayerAnim.SetBool("issprinting", true);
            //if is sprinting walk faster
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
        }else if(IsGrounded())
        {//for falling and if not on jumpable ground
            PlayerAnim.SetBool("isjumping", false);
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
        CreateDust();
        transform.Rotate(0f, 180f, 0f);
        Camera.main.transform.Rotate(0f, -180f, 0f);
    }
    void CreateDust()
    {
        Dust.Play();
    }
    void DebugControlls()
    {
        if (Input.GetKey(KeyCode.F2))
        {
            Debug.Log("teleported to mouse position");
            if(Input.GetMouseButtonDown(0)) this.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -1));
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //for slamming back on the ground
        if (collision.gameObject.layer == LayerMask.NameToLayer("Jumpable"))
        {
            if (rb.velocity.y <= 0.5f && rb.velocity.y >= -0.5f)
            {
                SlamDust.Play();
                PlayerAnim.SetBool("isjumping", false);
            }
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {//if on slope then add particles
        if (collision.transform.gameObject.tag == "Slope" && rb.velocity.y != 0 && !Input.GetKey(KeyCode.Space))
        {
            CreateDust();
        }
    }
}
