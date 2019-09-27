using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private GameObject _powerupPrefab;

    [SerializeField]
    private float _speed = 3f;

    [SerializeField]
    private float _verticalBound = 8f;
    [SerializeField]
    private float _horizontalBound = 9f;

    // Start is called before the first frame update
    void Start()
    {
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
        Debug.Log("Powerup Hit: " + other.tag);

        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Powerup(this.tag);
                Destroy(this.gameObject);
            }
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
