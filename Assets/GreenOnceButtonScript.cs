using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenOnceButtonScript : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer;
    public Sprite OnSprite;
    public Sprite OffSprite;

    public GameObject[] Listeners;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        SpriteRenderer.sprite = OnSprite;
       foreach (var listener in Listeners)
        {
            listener.BroadcastMessage("ButtonPressed"); // IMPORTANT how to use buttons, but every game object in the level that you want the button to activate in the gameobject array
        }                                               // IMPORTANT they all need the "ButtonPressed" function, and ones using orange button also need the "ButtonUnPressed" function
        
    }
}
