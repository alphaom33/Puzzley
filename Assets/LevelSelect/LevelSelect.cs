using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LevelSelect : MonoBehaviour
{
    public RectTransform rectTransform;

    private Vector2 CalcStartPos() => Vector2.right * (GetComponentInParent<RectTransform>().rect.width + 100);
    private Tween TweenAnchored(Vector2 end) => DOTween.To(() => rectTransform.anchoredPosition, x => rectTransform.anchoredPosition = x, end, 1);

    public void OnEnable() 
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = CalcStartPos();
        TweenAnchored(Vector2.zero);
    }

    public void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Leave();
    }

    public void Leave()
    {
        DOTween.KillAll();
        StartCoroutine(Leave());
        IEnumerator Leave() 
        {
            var tween = TweenAnchored(CalcStartPos());
            yield return tween.WaitForCompletion();
            gameObject.SetActive(false);
        }
    }
}