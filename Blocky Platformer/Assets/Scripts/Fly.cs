using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : MonoBehaviour
{
    Vector3 _startPosition;
    Vector2 _direction = Vector2.up;

    // Start is called before the first frame update
    void Start()
    {
        _startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(_direction * Time.deltaTime);
        var distance = Vector2.Distance(_startPosition, transform.position);
        if (distance >= 2)
        {
            _direction *= -1;
        }
    }
}
