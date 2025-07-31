using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrusherScript : MonoBehaviour
{


    public Rigidbody2D Rigidbody2D;

    public float CrusherUpAmount;
    public float CrusherInterval;
    public float CrusherTimeDown;

    void Start()
    {
        Rigidbody2D.bodyType = RigidbodyType2D.Static;
        StartCoroutine(Crush());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Body") || collision.gameObject.CompareTag("Ground"))
        {
            StartCoroutine(Rise());
        }
        else if (collision.gameObject.CompareTag("Player") && collision.transform.position.y < transform.position.y)
        {
            GameManager.GetInstance().KillPlayer();
        }
        
    }

        private IEnumerator Crush()
        {

            yield return new WaitForSeconds(CrusherInterval); // time it stays up before coming down to crush


            Rigidbody2D.bodyType = RigidbodyType2D.Dynamic; // allows the crusher to fall down
        }
        private IEnumerator Rise()
        {
            yield return new WaitForSeconds(CrusherTimeDown); // keeps the crush down for as long as you want

            transform.DOMoveY(CrusherUpAmount, 1); // raises the crusher/

            Rigidbody2D.bodyType = RigidbodyType2D.Static;// makes it so the crusher stays in the air, and then counts down tell it crushes again
            StartCoroutine(Crush()); // allows crusher to come down, after the interval

            yield return new WaitForSeconds(CrusherInterval);
            Rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        }


    } 
