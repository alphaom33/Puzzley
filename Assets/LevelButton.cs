using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelButton : MonoBehaviour
{
    public void Setup(int num) {
        GetComponent<Button>().onClick.AddListener(() => GameManager.GetInstance().LoadLevel(num));
        GetComponentInChildren<TMP_Text>().text = num + 1 + "";
    }
}
