using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Carousel : MonoBehaviour
{
    public RectTransform mainCard;
    public RectTransform animCard;

    public float offCardPos;
    public int currentLevel;

    public int moving;
    public float animLength = 0.5f;
    public float bufferTime = 0.1f;

    private Tween TweenAnchored(RectTransform t, Vector2 end) => DOTween.To(() => t.anchoredPosition, x => t.anchoredPosition = x, end, animLength);
    private void ApplyLevelTo(RectTransform t) => t.GetComponent<Card>().ApplyLevel(GameManager.GetInstance().levels[currentLevel]);

    public void Start() 
    {
        ApplyLevelTo(mainCard);
    }

    public void Update() 
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) Move(-1);
        else if (Input.GetKeyDown(KeyCode.RightArrow)) Move(1);
    }

    IEnumerator BufferMove(int dir) 
    {
        for (float time = 0; time < bufferTime; time += Time.deltaTime) 
        {
            if (moving == 0) 
            {
                Move(dir);
                yield break;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    public void Move(int dir) 
    {
        if (moving == dir) 
        {
            StartCoroutine(BufferMove(dir));
            return;
        }

        ApplyLevelTo(animCard);
        currentLevel = (int)Mathf.Repeat(currentLevel + dir, GameManager.GetInstance().levels.Count);
        ApplyLevelTo(mainCard);

        if (moving == -dir) 
        {
            StopAllCoroutines();
            DOTween.KillAll();
            (mainCard.anchoredPosition, animCard.anchoredPosition) = (animCard.anchoredPosition, mainCard.anchoredPosition);
        }
        else
        {
            mainCard.anchoredPosition = Vector2.right * dir * offCardPos;
            animCard.anchoredPosition = Vector2.zero;
        }

        StartCoroutine(Tween());
        IEnumerator Tween() 
        {
            moving = dir;
            TweenAnchored(mainCard, Vector2.zero);
            var tween = TweenAnchored(animCard, Vector2.left * dir * offCardPos);
            yield return tween.WaitForCompletion();
            moving = 0;
        }
    }

    public void Go() 
    {
        GameManager.GetInstance().LoadLevel(currentLevel);
    }
}
