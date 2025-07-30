using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBox : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(.4f, .5f, 1);
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
