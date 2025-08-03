using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelsHolder : MonoBehaviour
{
    public GameObject levelButton;
    public float xDst;
    public float yDst;

    // Start is called before the first frame update
    void Start()
    {
        float xOff = xDst * 4.0f / 2.0f;
        float yOff = yDst * 2.0f / 2.0f;
        for (int i = 0; i < 5; i++) {
            for (int j = 0; j < 3; j++) {
                GameObject g = Instantiate(levelButton, transform);
                g.GetComponent<RectTransform>().anchoredPosition = new Vector2(xDst * i - xOff, -yDst * j + yOff);
                int num = i + j * 5;
                g.GetComponent<LevelButton>().Setup(num);
                g.GetComponent<Button>().interactable = num <= GameManager.GetInstance().maxLevel;
            }
        }
    }
}
