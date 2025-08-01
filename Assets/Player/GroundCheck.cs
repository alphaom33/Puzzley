using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private bool CollidingPlatform(Collider2D collision) => collision.CompareTag("Ground") || collision.CompareTag("Body");

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (CollidingPlatform(collision)) SendMessageUpwards("HitGround");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (CollidingPlatform(collision) && gameObject.activeInHierarchy) SendMessageUpwards("LeftGround");
    }
}
