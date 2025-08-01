using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OrangeHoldButtonScript : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer;
    public Sprite OnSprite;
    public Sprite OffSprite;

    public GameObject[] Listeners;

    private bool BodyOnButton;
    private void OnTriggerEnter2D(Collider2D collision) 
    {
       if (collision.gameObject.CompareTag("Body")) BodyOnButton = true;
        SpriteRenderer.sprite = OnSprite;
        foreach (var listener in Listeners)
        {
            listener.BroadcastMessage("ButtonPressed");// IMPORTANT how to use buttons, but every game object in the level that you want the button to activate in the gameobject array
        }                                               // IMPORTANT they all need the "ButtonPressed" function, and ones using orange button also need the "ButtonUnPressed" function
    }

    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Body")) BodyOnButton = false;



        if (!BodyOnButton)
        {
            SpriteRenderer.sprite = OffSprite;
            foreach (var listener in Listeners)
            {
                if (listener != null) listener.BroadcastMessage("ButtonUnPressed"); 
            }

        }
        
    }
}
