using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyFeed : MonoBehaviour
{
    public float destriyTime = 4f;

    private void OnEnable()
    {
        Destroy(gameObject, destriyTime);
    }
}
