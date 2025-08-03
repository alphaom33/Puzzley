using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
   public Rigidbody2D body;

    public float BulletSpeed;
    public float Direction;
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Body") || collision.gameObject.CompareTag("Ground"))
        {
            Destroy(this.gameObject);
        }
        else if (collision.gameObject.CompareTag("Player") && collision.transform.position.y < transform.position.y)
        {
            GameManager.GetInstance().KillPlayer();
        }
    }

    // Update is called once per frame
    void Update()
    {
        body.AddForce(new Vector2 (BulletSpeed * Direction,0));
    }
}
