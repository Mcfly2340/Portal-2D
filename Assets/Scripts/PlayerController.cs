using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float movespeed, dirX, dirY;
    public Animator PlayerAnim;

    public float positionRadius;
    public float speed;
    public int multiplier = 1;
    public float jumpForce;
    [SerializeField] private Collider2D playerCollider;
    [SerializeField] private LayerMask groundLayer;
    public static GameObject player;
    //facing direction player
    bool isFacingRight = true;

    public static bool isCrawling = false;
    private bool isSprinting = false;

    public float maxVelocity = 80;
    private static GameObject instance;

   // [Header("Events")]
    //[Space]

    
    void Start()
    {
        DuplicationCorrection();
    }
    void Update()
    {
        if (StartCutscene.isInCutscene == false)
        {
            Jump();
            Movement();
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

        Sprinting();
        

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
        
        Crawling();
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
    private void Jump()
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
        
        if (collision.transform.gameObject.name == "floor" || collision.transform.gameObject.name == "ceiling")
        {
            Debug.Log(collision.transform.gameObject.name);
            if (rb.velocity.y <= 0.5f && rb.velocity.y >= -0.5f) PlayerAnim.SetBool("isjumping", false);
        }
    }
}
