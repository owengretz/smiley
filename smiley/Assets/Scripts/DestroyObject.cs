using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    private void Start()
    {
        // destroy particle after 2 seconds
        Destroy(gameObject, 2);
    }
}
