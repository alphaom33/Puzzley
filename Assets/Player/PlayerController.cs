using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    public bool onGround;
    public float coyoteTime;

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

    [Header("Juice")]
    public Transform child;
    public Vector2 juiceMultipliers;
    public ParticleSystem movementParticles;
    public ParticleSystem landingParticles;
    public float minSplash;
    public float lastVel;

    private Rigidbody2D rb;

    private IEnumerator joimp;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Awake()
    {
        StartCoroutine(EnableDeath());
        
        
    }
    private IEnumerator EnableDeath()
    {
        yield return new WaitForSeconds(.1f);
        GameManager.GetInstance().canDie = true;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !jumpBuffering)
        {
            StartCoroutine(JumpBuffer());
        }

        child.localScale = CalcJuice();
        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0) child.localScale = new Vector2(Mathf.Abs(child.localScale.x) * Mathf.Sign(Input.GetAxis("Horizontal")), child.localScale.y);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (onGround)
        {
            rb.AddForce(Vector2.right * speed * Input.GetAxis("Horizontal"));
            rb.velocity = new Vector2(DoVelStuf(rb.velocity.x), rb.velocity.y);

            if (Mathf.Abs(rb.velocity.x) > maxSpeed / 2.0f && Random.Range(0, 5) == 0)
            {
                movementParticles.Emit(1);
            }
        } 
        else
        {
            rb.AddForce(Vector2.right * airSpeed * Input.GetAxis("Horizontal"));
            rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -airMaxSpeed, airMaxSpeed), Mathf.Clamp(rb.velocity.y, -maxFallSpeed, float.MaxValue));
        }

        lastVel = rb.velocity.y;
    }

    private Vector2 CalcJuice()
    {
        Vector2 juice = new Vector2(Mathf.Sign(child.localScale.x), 1);

        if (onGround)
        {
            float xJuice = juiceMultipliers.x * Mathf.Abs(rb.velocity.x / maxSpeed);
            juice *= new Vector2(1 + xJuice, 1 - xJuice);
        }

        float yJuice = juiceMultipliers.y * Mathf.Abs(rb.velocity.y / maxFallSpeed);
        juice *= new Vector2(1 - yJuice, 1 + yJuice);

        return juice;
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
        rb.gravityScale = jumpingGravity;
        rb.velocity *= Vector2.right;

        for (bool gone = false; Input.GetKey(KeyCode.Space) && jumpTime < maxJumpTime; jumpTime += Time.fixedDeltaTime)
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

    private void HitGround()
    {
        onGround = true;
        jumpBuffering = false;
        jumpTime = 0;
        if (joimp != null) StopCoroutine(joimp);

        if (lastVel < -minSplash) landingParticles.Emit((int)(-lastVel - minSplash));
    }

    private void LeftGround()
    {
        StartCoroutine(CoyoteTime());
    }

    IEnumerator CoyoteTime()
    {
        yield return new WaitForSeconds(coyoteTime);
        if (jumpTime > 0) yield break;
        onGround = false;
        jumpTime = maxJumpTime;
    }

}