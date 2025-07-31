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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.GetInstance().KillPlayer();
        }
        else if (collision.gameObject.CompareTag("Body") || collision.gameObject.CompareTag("Ground"))
        {
            StartCoroutine(Rise());
        }
    }

    private IEnumerator Crush()
    {
        yield return new WaitForSeconds(CrusherInterval);
        Rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
    }

    private IEnumerator Rise()
    {
        yield return new WaitForSeconds (CrusherTimeDown);
       
        transform.DOMoveY(CrusherUpAmount,1);

        Rigidbody2D.bodyType = RigidbodyType2D.Static;
        StartCoroutine(Crush());
    }
}
