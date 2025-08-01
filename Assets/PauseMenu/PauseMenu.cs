using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PauseMenu : MonoBehaviour
{
    public RectTransform child;

    private bool leaving;

    private Vector2 CalcStartPos() => Vector2.up * (GetComponentInChildren<RectTransform>().rect.height + child.rect.height) / 2;
    private Tween TweenAnchored(Vector2 end) => DOTween.To(() => child.anchoredPosition, x => child.anchoredPosition = x, end, 1).SetUpdate(UpdateType.Normal, true);

    void Start()
    {
        child.anchoredPosition = CalcStartPos();
        TweenAnchored(Vector2.zero);
        Time.timeScale = 0;
    }

    public void UnPause()
    {
        if (leaving)
        {
            StopAllCoroutines();
            TweenAnchored(Vector2.zero);
        }
        else
        {
            StartCoroutine(UnPauseRoutine());
        }
        leaving = !leaving;
        IEnumerator UnPauseRoutine()
        {
            Tween leave = TweenAnchored(CalcStartPos());
            yield return leave.WaitForCompletion();
            Time.timeScale = 1;
            GameManager.GetInstance().canMove = true;
            Destroy(gameObject);
        }
    }
}
