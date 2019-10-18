using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 10f;
    private float _baseSpeed = 10f;

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
    private bool _speedBoostActive = false;
    [SerializeField]
    private bool _shieldActive = false;

    [SerializeField]
    private float _tripleshotDuration = 5f;
    private IEnumerator _tripleshotCoroutine;

    [SerializeField]
    private float _speedBoostDuration = 5f;
    [SerializeField]
    private float _speedBoostMultiplier = 1.5f;
    private IEnumerator _speedBoostCoroutine;

    [SerializeField]
    private GameObject _tripleshotPrefab;

    [SerializeField]
    private GameObject _shieldVisual;

    private SpawnManager _spawnManager;

    [SerializeField]
    private int _score = 0;

    private UIManager _ui;

    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _ui = GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>();

        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is null");
        }

        if (_ui == null)
        {
            Debug.LogError("UI is null");
        }

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
        if (_shieldActive)
        {
            _shieldActive = false;
            _shieldVisual.SetActive(false);
            return;
        }
        
        _lives--;

        _ui.UpdateLives(_lives);

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }

    public void IncreaseScore(int points)
    {
        _score += points;
        _ui.UpdateScore(_score.ToString());
    }

    public void TripleShotActive()
    {
        _tripleshotActive = true;

        if (_tripleshotCoroutine != null)
        {
            StopCoroutine(_tripleshotCoroutine);
        }

        _tripleshotCoroutine = TripleshotDuration();
        StartCoroutine(_tripleshotCoroutine);
    }

    public void SpeedBoostActive()
    {
        _speedBoostActive = true;
        _speed = _baseSpeed * _speedBoostMultiplier;

        if (_speedBoostCoroutine != null)
        {
            StopCoroutine(_speedBoostCoroutine);
        }

        _speedBoostCoroutine = SpeedBoostDuration();
        StartCoroutine(_speedBoostCoroutine);
    }

    public void ShieldActive()
    {
        _shieldActive = true;
        _shieldVisual.SetActive(true);
    }

    IEnumerator TripleshotDuration()
    {
        yield return new WaitForSeconds(_tripleshotDuration);
        _tripleshotActive = false;
    }

    IEnumerator SpeedBoostDuration()
    {
        yield return new WaitForSeconds(_speedBoostDuration);
        _speedBoostActive = false;
        _speed = _baseSpeed;
    }
}
