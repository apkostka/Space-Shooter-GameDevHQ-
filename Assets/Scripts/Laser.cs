using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8f;

    [SerializeField]
    private float _upperBounds = 8f;

    private float _direction_y;
    private bool _isFired = false;
    private bool _isEnemyLaser;
    private Player _player;

    // Update is called once per frame
    void Update()
    {
        if (_isFired)
        {
            CalculateMovement();
        }
    }

    void CalculateMovement()
    {
        Vector3 direction = new Vector3(0, _direction_y, 0);
        transform.Translate(direction * _speed * Time.deltaTime);

        if (transform.position.y > _upperBounds)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }
    }

    public void Fire(float d_y, float speed_add = 0f, bool enemy = false)
    {
        _direction_y = d_y;
        _speed += speed_add;
        _isEnemyLaser = enemy;
        _isFired = true;
        Debug.Log("Fire Laser");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && _isEnemyLaser)
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
                Destroy(this.gameObject);
            }
        }
        else if (other.tag == "Enemy" && !_isEnemyLaser)
        {
            Enemy enemy = other.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.DestroyEnemy(true);
                Destroy(this.gameObject);
            }
        }

    }
}
