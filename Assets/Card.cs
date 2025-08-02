using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Card : MonoBehaviour
{

    public void Awake() {
        GetComponentInChildren<Button>().onClick.AddListener(GetComponentInParent<Carousel>().Go);
    }

    public void ApplyLevel(Level level, bool enabled) {
        GetComponentInChildren<Image>().sprite = level.sprite;

        Button button = GetComponentInChildren<Button>();
        button.interactable = enabled;
        button.GetComponentInChildren<TMP_Text>().text = enabled ? level.displayName : "Locked";
    }
}
