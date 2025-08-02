using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private bool CollidingPlatform(Collider2D collision) => collision.CompareTag("Ground") || collision.CompareTag("Body");
    public int numColliding;
    public int lastNumColliding;

    private void FixedUpdate() {
        if (numColliding == 0 && lastNumColliding > 0) StartCoroutine(CheckMessage("LeftGround", () => numColliding == 0));
        else if (numColliding > 0 && lastNumColliding == 0) StartCoroutine(CheckMessage("HitGround", () => numColliding != 0));
        lastNumColliding = numColliding;
    }

    IEnumerator CheckMessage(string message, Func<bool> check) 
    {
        yield return new WaitForFixedUpdate();
        if (check()) SendMessageUpwards(message);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (CollidingPlatform(collision)) numColliding++;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (CollidingPlatform(collision) && gameObject.activeInHierarchy) numColliding--;
    }
}
