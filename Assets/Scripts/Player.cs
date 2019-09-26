using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f;

    // set up Bounds object with camera view limits
    [SerializeField]
    private float _horizontalBound = 11.5f;
    [SerializeField]
    private float _verticalBound = 3.8f;

    [SerializeField]
    private float _fireRate = 0.15f;
    private float _nextFire = -1f;

    [SerializeField]
    private GameObject _laserPrefab = null;

    // Start is called before the first frame update
    void Start()
    {
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
        Instantiate(_laserPrefab, transform.position + new Vector3(0f, 0.8f, 0f), Quaternion.identity);
    }
}
