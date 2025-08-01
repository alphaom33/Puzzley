using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Card : MonoBehaviour
{
    public void ApplyLevel(Level level) {
        GetComponentInChildren<TMP_Text>().text = level.displayName;
    }
}
