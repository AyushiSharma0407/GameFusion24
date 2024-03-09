using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public static AnimationController instance;

    public float moveSpeed;
    public Rigidbody2D theRB;
    private bool isGrounded;
    public Transform groundCheckPoint;
    public LayerMask groundLayer;
    private Animator anim;
    private SpriteRenderer theSR;
    private float horizontalInput;

    public void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        theSR = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, 0.2f, groundLayer);

        // Get horizontal input
        horizontalInput = Input.GetAxis("Horizontal");

        // Update animator parameters
        anim.SetFloat("moveSpeed", Mathf.Abs(horizontalInput));
        anim.SetBool("isGrounded", isGrounded);
    }
}
