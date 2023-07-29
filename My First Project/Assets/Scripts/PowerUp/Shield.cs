using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(Vector3.up * 400f * Time.deltaTime);
    }
}
