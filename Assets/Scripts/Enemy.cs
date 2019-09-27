﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    private float _baseSpeed = 3;
    private float _speedRangeSize = 1;

    [SerializeField]
    private float _verticalBound = 8f;
    [SerializeField]
    private float _horizontalBound = 10f;

    // Start is called before the first frame update
    void Start()
    {
        _speed = Random.Range(_baseSpeed - _speedRangeSize, _baseSpeed + _speedRangeSize);

        SetRandomPositionAtTop();
    }

    // Update is called once per frame
    void Update()
    {
        // move down at 4 m/s
        CalculateMovement();

        // if bottom of screen, respawn at top with new random x position
        if (transform.position.y < -_verticalBound + 1) // makes up for length of ship
        {
            SetRandomPositionAtTop();
        }

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Enemy Hit: " + other.tag);

        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
                Destroy(this.gameObject);
            }
        }
        else if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }

    }

    void CalculateMovement()
    {
        Vector3 direction = new Vector3(0, -1, 0);
        transform.Translate(direction * _speed * Time.deltaTime);
    }

    void SetRandomPositionAtTop()
    {
        transform.position = new Vector3(Random.Range(-_horizontalBound, _horizontalBound), _verticalBound, 0);
    }
}
