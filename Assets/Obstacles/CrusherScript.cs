using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrusherScript : MonoBehaviour
{
   public Rigidbody2D Rigidbody2D; // the crusher's rigidbody
   
    
    public float CrusherUpAmount; // the amount the crusher goes upwards, after crushing
    public float CrusherInterval; // the interval between crushes
    public float CrusherTimeDown; // the time the crusher stays down

    public Transform BoxCast;
    void Start()
    {
        
        Rigidbody2D.bodyType = RigidbodyType2D.Static;// crusher starts up, and crushes in a few secconds
        StartCoroutine(Crush());
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var Player = collision.gameObject.GetComponent<PlayerController>(); // tries to get the PlayerController script from the collided object
        if (Player != null) Player.KillPlayer(); // if it succseeds, it means it hit the player and they die
        else if (collision.gameObject.tag == "Body" || collision.gameObject.tag == "Ground") // if it hit a body or the ground, it gets ready to rise in a few secconds
        {
            StartCoroutine(Rise()); //starts to rise
        }
    }

    private IEnumerator Crush()
    {
        yield return new WaitForSeconds(CrusherInterval); // time it stays up before coming down to crush
        
        
        Rigidbody2D.bodyType = RigidbodyType2D.Dynamic; // allows the crusher to fall down
    }
    private IEnumerator Rise()
    {
        yield return new WaitForSeconds (CrusherTimeDown); // keeps the crush down for as long as you want
       
        transform.DOMoveY(CrusherUpAmount,1); // raises the crusher/

        Rigidbody2D.bodyType = RigidbodyType2D.Static;// makes it so the crusher stays in the air, and then counts down tell it crushes again
        StartCoroutine(Crush()); // allows crusher to come down, after the interval
    }
}
