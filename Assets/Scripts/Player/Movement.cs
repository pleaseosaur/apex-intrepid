using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour, IHealthy
{
    private Rigidbody rb;
    private Animator anim;

    [SerializeField] private KeyCode sprintKey = KeyCode.LeftControl;

    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float jumpForce = 5f;
    private float movementSpeed;
    private Vector3 spawn;
    private Player_Stats playerStats;
    
    // SFX
    private SoundManager soundManager;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        playerStats = GetComponent<Player_Stats>();

        anim = GetComponent<Animator>();
        anim.applyRootMotion = false;
        
        soundManager = FindObjectOfType<SoundManager>();

        movementSpeed = walkSpeed;

        StartCoroutine(GetSpawn());
    }

    void Update()
    {
        if (!Busy())
        {
            Move();
            Jump();
            Fall();
        }
        else
        {
            return;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the player is touching any collider tagged "Floor"
        if (collision.gameObject.CompareTag("Floor"))
        {
            anim.SetBool("Jumping", false);
        }
    }


    private IEnumerator GetSpawn()
    {
        yield return new WaitForSeconds(0.1f);
        spawn = transform.position;
    }

    /// <summary>
    /// This method handles user input wrt movement.
    /// </summary>
    void Move()
    {
        float moveSideways = Input.GetAxis("Horizontal") * 0.75f;                    //  Keep strafing speed to 3/4
        float moveForward  = Input.GetAxis("Vertical");

        if (moveForward < 0)                                                         //  Keep backpedal speed to 3/4.
        {
            moveForward *= 0.75f;
        }

        movementSpeed = Input.GetKey(sprintKey) ? runSpeed : walkSpeed;

        if (movementSpeed == runSpeed && moveForward > 0)                            //  Only allow sprinting if moving forward.
        {
            anim.SetBool("Sprinting", true);
        }
        else
        {
            anim.SetBool("Sprinting", false);
            movementSpeed = walkSpeed;
        }

        Vector3 movement = transform.right   * moveSideways                          //  Calculate movement.
                         + transform.forward * moveForward;

        if (movement.magnitude > 1f)                                                 //  Normalize movement in event player is moving diagonally.
        {
            movement.Normalize();
        }

        rb.MovePosition(rb.position + Slope(movement) * movementSpeed * Time.deltaTime);    //  Apply movement force to player rigidbody.

        anim.SetFloat("Horizontal", moveSideways);
        anim.SetFloat("Vertical", moveForward);
        anim.SetFloat("Speed", movementSpeed);
    }
    /// <summary>
    /// This method applies upward force to the player when the space bar is pressed.
    /// </summary>
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Mathf.Abs(rb.velocity.y) < 0.001f)
        {
            // play jump animation without impacting movement
            //anim.SetTrigger("Jump");
            anim.SetBool("Jumping", true);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            soundManager.Play("JumpSound");
        }
    }

    /// <summary>
    /// This method resets the player's position to the initial position if they fall below a certain Y value.
    /// </summary>
    void Fall()
    {
        if (transform.position.y < -10f)
        {
            transform.position = spawn;
            rb.velocity = Vector3.zero;
            playerStats.Damage(20, Vector3.zero);
        }
    }

    /// <summary>
    /// This method checks if the player is "busy":
    /// <list>
    /// -If they are taunting
    /// </list>
    /// </summary>
    /// <returns>true if busy, false if not</returns>
    bool Busy()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("Taunt")                   //  Prevent movement if actively taunting.
            || anim.GetCurrentAnimatorStateInfo(0).IsName("Dead")
            || anim.GetCurrentAnimatorStateInfo(0).IsName("Build");
    }

    /// <summary>
    /// This method determines the angle of the surface the player is walking on.
    /// </summary>
    /// <param name="movement"></param>
    /// <returns></returns>
    Vector3 Slope(Vector3 movement)
    {
        RaycastHit hit;
        Vector3 origin = transform.position + Vector3.up * 0.1f;

        if (Physics.Raycast(origin, Vector3.down, out hit, 0.2f))
        {
            return Vector3.ProjectOnPlane(movement, hit.normal).normalized * movement.magnitude;
        }

        return movement;
    }

    public void doDamage(float damage)
    {
        playerStats.doDamage(damage);
    }

    public void healHealth(float health)
    {
        throw new System.NotImplementedException();
    }

    public void setHealth(float health)
    {
        throw new System.NotImplementedException();
    }
}