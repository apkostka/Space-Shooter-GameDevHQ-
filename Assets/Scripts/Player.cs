﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 10f;

    // TODO: set up Bounds object with camera view limits
    [SerializeField]
    private float _horizontalBound = 11.5f;
    [SerializeField]
    private float _verticalBound = 3.8f;

    [SerializeField]
    private int _lives = 3;

    // Laser
    [SerializeField]
    private float _fireRate = 0.15f;
    private float _nextFire = -1f;

    [SerializeField]
    private GameObject _laserPrefab = null;

    // Powerups
    [SerializeField]
    private bool _tripleshotActive = false;
    [SerializeField]
    private float _tripleshotDuration = 5f;
    [SerializeField]
    private GameObject _tripleshotPrefab;
    private IEnumerator _tripleshotCoroutine;

    private SpawnManager _spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();

        transform.position = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) & Time.time > _nextFire)
        {
            FireLaser();
        }

        if (_spawnManager == null)
        {
            Debug.LogError("SpawnManager is null");
        }
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Set movement
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * _speed * Time.deltaTime);

        // handle screen bounds and wrapping
        Vector3 tempPosition = transform.position;

        tempPosition.y = Mathf.Clamp(transform.position.y, -_verticalBound, 0);

        // wrap if Player goes offscreen
        if (transform.position.x >= _horizontalBound)
        {
            tempPosition.x = -_horizontalBound;
        }
        else if (transform.position.x <= -_horizontalBound)
        {
            tempPosition.x = _horizontalBound;
        }

        transform.position = tempPosition;
    }

    void FireLaser()
    {
        _nextFire = Time.time + _fireRate;
        if (_tripleshotActive)
        {
            Instantiate(_tripleshotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0f, 1f, 0f), Quaternion.identity);
        }
    }

    public void Damage()
    {
        _lives--;

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }

    public void Powerup(string powerupTag)
    {
        switch (powerupTag)
        {
            case "Tripleshot":
                _tripleshotActive = true;

                if (_tripleshotCoroutine != null)
                {
                    StopCoroutine(_tripleshotCoroutine);
                }

                _tripleshotCoroutine = TripleshotDuration();
                StartCoroutine(_tripleshotCoroutine);
                
                break;
        }
    }

    IEnumerator TripleshotDuration()
    {
        yield return new WaitForSeconds(_tripleshotDuration);
        _tripleshotActive = false;
    }
}
