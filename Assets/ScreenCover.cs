using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScreenCover : MonoBehaviour
{
    public RectTransform child;

    private Vector2 CalcStartPos() => Vector2.right * GetComponent<RectTransform>().rect.width;
    private Tween TweenAnchored(Vector2 end) => DOTween.To(() => child.anchoredPosition, x => child.anchoredPosition = x, end, 0.5f).SetUpdate(UpdateType.Normal, true);

    public void Start() 
    {
        child.anchoredPosition = CalcStartPos();
    }

    public Tween Enter() 
    { 
        child.anchoredPosition = CalcStartPos();
        return TweenAnchored(Vector2.zero);
    }

    public Tween Exit() => TweenAnchored(-CalcStartPos());
}
