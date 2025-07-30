using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Ground Movement")]
    public float speed;
    public float maxSpeed;

    [Header("Air Movement")]
    public float jumpForce;
    public float airSpeed;
    public float airMaxSpeed;

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
            jumpBuffering = false;
            jumpTime = 0;
            if (joimp != null) StopCoroutine(joimp);
        }
    }
}
