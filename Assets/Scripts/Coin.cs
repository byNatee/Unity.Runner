using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private float _speedRotation = 50;
    private void Update()
    {
        transform.Rotate(0, 0, _speedRotation * Time.deltaTime);
    }
}
