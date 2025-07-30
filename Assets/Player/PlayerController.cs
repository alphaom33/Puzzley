using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    public bool onGround;

    [Header("Ground Movement")]
    public float speed;
    public float maxSpeed;
    public float friction;

    [Header("Air Movement")]
    public float airSpeed;
    public float airMaxSpeed;
    public float maxFallSpeed;

    [Header("Jump")]
    public float bufferWaitTime;
    public float initialJumpForce;
    public float boingJumpForce;
    public float jumpTime;
    public float maxJumpTime;
    public float jumpingGravity;
    public float fallingGravity;
    public bool jumpBuffering;

    private Rigidbody2D rb;

    private IEnumerator joimp;

    private bool IsFacingLeft;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !jumpBuffering)
        {
            StartCoroutine(JumpBuffer());
        }

        if (onGround)
        {
            rb.AddForce(Vector2.right * speed * Input.GetAxis("Horizontal"));
            rb.velocity = new Vector2(DoVelStuf(rb.velocity.x), rb.velocity.y);
            if (Input.GetAxis("Horizontal") < 0 && !IsFacingLeft) { transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z); IsFacingLeft = true; }
            if (Input.GetAxis("Horizontal") > 0 && IsFacingLeft) { transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z); IsFacingLeft = false; }
        } 
        else
        {
            rb.AddForce(Vector2.right * airSpeed * Input.GetAxis("Horizontal"));
            rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -airMaxSpeed, airMaxSpeed), Mathf.Clamp(rb.velocity.y, -maxFallSpeed, float.MaxValue));
            if (Input.GetAxis("Horizontal") < 0 && !IsFacingLeft) { transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z); IsFacingLeft = true; }
            if (Input.GetAxis("Horizontal") > 0 && IsFacingLeft) { transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z); IsFacingLeft = false; }
        }
    }

    private float DoVelStuf(float x)
    {
        float clamped = Mathf.Clamp(x, -maxSpeed, maxSpeed);
        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) <= 0.1)
        {
            clamped *= friction;
        }
        return clamped;
    }

    private IEnumerator JumpBuffer()
    {
        jumpBuffering = true;
        for (float time = 0; jumpTime != 0; time += Time.deltaTime)
        {
            if (time > bufferWaitTime)
            {
                jumpBuffering = false;
                yield break;
            }
            yield return new WaitForEndOfFrame();
        }
        rb.AddForce(Vector2.up * initialJumpForce);
        jumpBuffering = false;
        if (joimp != null) StopCoroutine(joimp);
        joimp = Joimp();
        StartCoroutine(joimp);
    }

    private IEnumerator Joimp()
    {
        onGround = false;
        bool gone = false;
        rb.gravityScale = jumpingGravity;
        for (; Input.GetKey(KeyCode.Space) && jumpTime < maxJumpTime; jumpTime += Time.fixedDeltaTime)
        {
            if (!gone && jumpTime > Time.fixedDeltaTime * 3)
            {
                rb.AddForce(Vector2.up * boingJumpForce);
                gone = true;
            }
            yield return new WaitForFixedUpdate();
        }
        rb.gravityScale = fallingGravity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            onGround = true;
            jumpBuffering = false;
            jumpTime = 0;
            if (joimp != null) StopCoroutine(joimp);
        }
    }
}
