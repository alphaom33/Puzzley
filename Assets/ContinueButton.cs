using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContinueButton : MonoBehaviour
{
    void Start()
    {
        Button button = GetComponent<Button>();
        button.interactable = GameManager.GetInstance().maxLevel > 0;
        button.onClick.AddListener(() => GameManager.GetInstance().LoadLevel(GameManager.GetInstance().maxLevel));
    }
}
