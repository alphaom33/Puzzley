using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateScript : MonoBehaviour
{
    public void ButtonPressed()
    {
        transform.DOMoveY(10, 1);
    }
    public void ButtonUnPressed()
    {
        transform.DOMoveY(0, 1);
    }
}
