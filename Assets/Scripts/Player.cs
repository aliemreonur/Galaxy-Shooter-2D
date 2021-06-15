using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float _xAxis;  
    private float _yAxis;

    [SerializeField] private GameObject _laser, _tripleLaser;
    [SerializeField] private GameObject _rightFire, _leftFire, _explosion;
    [SerializeField] private GameObject _speedyThruster;
    [SerializeField] private GameObject _ultiShoot;
    
    [SerializeField] private float _speed = 6f;
    private float _cooldownTime = 0.2f;
    private float _fireTime;

    [SerializeField] private GameObject _shield;
    private bool _shieldActive;
    private bool _thrustActive;
    private bool _speedActive;
    [SerializeField] private bool _ultiActive;
    private bool _missileActive;

    [SerializeField]
    private bool _tripleShotActive = false;

    [SerializeField] private int _lives = 3;
    [SerializeField] private int _score = 0;
    [SerializeField] private int _shieldLives = 3;
    private int _ammo = 15;

    UIManager _uiManager;
    CameraShake _cameraShake;
    

    private AudioSource _audioSource;
    [SerializeField] AudioClip _laserShot, _deathSound, _outofAmmo;

    Color defaultShieldColor;
    SpriteRenderer _shieldSpriteRenderer;
    
    private float _thrustLeft = 1;

    public int Live
    {
        get
        {
            return _lives;
        }      
    }

    public int Ammo
    {
        get
        {
            return _ammo;
        }
        set
        {
            _ammo = value;
        }
    }

    public bool MissileActive
    {
        get
        {
            return _missileActive;
        }
        set
        {
            _missileActive = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if(!_uiManager)
        {
            Debug.LogError("Player Could not get the UI Manager");
        }

        _audioSource = GetComponent<AudioSource>();
        if(_audioSource == null)
        {
            Debug.LogError("Player could not get the audio source");
        }
        else
        {
            _audioSource.clip = _laserShot;
        }

        _cameraShake = Camera.main.GetComponent<CameraShake>();
        if(_cameraShake == null)
        {
            Debug.LogError("Player could not get the main camera's shake component");
        }

        _speedyThruster.gameObject.SetActive(false);

        _shieldSpriteRenderer = _shield.GetComponent<SpriteRenderer>();
        defaultShieldColor = _shieldSpriteRenderer.color;

        transform.position = new Vector3(0, -3, 0);
        _shield.gameObject.SetActive(false);
        _leftFire.gameObject.SetActive(false);
        _rightFire.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        if (Input.GetKeyDown(KeyCode.Space) && _fireTime < Time.time)
        {
            Fire();
        }
        ThrustMove();

        if(_shieldLives == 0)
        {
            _shield.gameObject.SetActive(false);
        }
        LifeAndDamageHandler();

    }

    private void Fire()
    {
        if (_ammo > 0)
        {
            _fireTime = Time.time + _cooldownTime;
            if(_ultiActive)
            {
                StartCoroutine(UltiRoutine());
                Instantiate(_ultiShoot, transform.position, Quaternion.identity);
            }
            else if(_tripleShotActive && !_ultiActive)
            {
                Instantiate(_tripleLaser, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
            }
            else
            {
                Instantiate(_laser, new Vector3(transform.position.x, transform.position.y + 1f, 0), Quaternion.identity);
            }

            _ammo--;
            _audioSource.clip = _laserShot;
            _audioSource.Play();
            _uiManager.UpdateAmmo();
        }

        else if(_ammo == 0)
        {
            _audioSource.clip = _outofAmmo;
            _audioSource.Play();
        }
    }

    private void ThrustMove()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            if(_thrustLeft >= 0)
            {
                _thrustActive = true;
                _speedyThruster.gameObject.SetActive(true);
                _speedyThruster.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                StartCoroutine(UseThrustRoutine());
            }
            else
            {
                _thrustActive = false;
                _speedyThruster.transform.localScale = new Vector3(1, 1, 1);
                _speedyThruster.gameObject.SetActive(false);
            }
        }
        else if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            _thrustActive = false;
            _speedyThruster.gameObject.SetActive(false);
            StartCoroutine(ThrustReloadRoutine());
        }
        if (_thrustActive)
        {
            if(_speedActive)
            {
                _speed = 12f;
            }
            else
            {
                _speed = 9f;
            }
        }
        else if (!_thrustActive && !_speedActive)
        {
            _speed = 6f;
        }
    }

    private void Movement()
    {
        _xAxis = Input.GetAxis("Horizontal");
        _yAxis = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(_xAxis, _yAxis, 0);

        transform.Translate(direction * _speed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -4.8f, 1), 0);

        if (transform.position.x > 10.2f)
        {
            transform.position = new Vector3(-10.2f, transform.position.y, 0);
        }
        else if (transform.position.x < -10.2f)
        {
            transform.position = new Vector3(10.2f, transform.position.y, 0);
        }
    }

    public void Damage()
    {
        if(_shieldActive)
        {
            switch(_shieldLives)
            {
                case 3:
                    _shieldLives--;
                    Color color1 = new Color32(188, 119, 119, 255);
                    _shieldSpriteRenderer.color = color1;
                    
                    break;
                case 2:
                    _shieldLives--;
                    StartCoroutine(ShieldRoutine1());
                    break;
                case 1:
                    _shieldLives--;
                    _shieldActive = false;
                    _shield.gameObject.SetActive(false);
                    break;
            }
        }
        else
        {
            _lives--;
            _cameraShake.ShakeCamera();
            _uiManager.UpdateLives(_lives);
            if(_lives <= 0)
            {
                Instantiate(_explosion, transform.position, Quaternion.identity);
                _uiManager.GameOver();
                Destroy(this.gameObject, 0.5f);
            }           
        }      
    }

    public void AddLife()
    {
        if(_lives <3)
        {
            _lives++;
            _uiManager.UpdateLives(_lives);
        }
    }

    public void LifeAndDamageHandler()
    {
        switch (_lives)
        {
            case 3:
                _rightFire.gameObject.SetActive(false);
                _leftFire.gameObject.SetActive(false);
                break;
            case 2:
                _rightFire.gameObject.SetActive(true);
                _leftFire.gameObject.SetActive(false);
                break;
            case 1:
                _leftFire.gameObject.SetActive(true);
                _rightFire.gameObject.SetActive(true);
                break;
            case 0:
                Instantiate(_explosion, transform.position, Quaternion.identity);
                _audioSource.clip = _deathSound;
                _audioSource.Play();
                _uiManager.GameOver();
                Destroy(this.gameObject);
                break;
        }
    }

    public void AddAmmo()
    {
        _ammo = 15;
        _uiManager.UpdateAmmo();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "EnemyLaser")
        {
            Damage();
        }
    }

    public void UltiActive()
    {
        _ultiActive = true;
        StartCoroutine(UltiRoutine());
    }


    public void TripleShotActive()
    {
        _tripleShotActive = true;
        StartCoroutine(TripleShotCoroutine());
    }

    public void SpeedActive()
    {
        _speedActive = true;
        _speed = 12f;
        StartCoroutine(SpeedActiveCoroutine());
    }

    public void ShieldActive()
    {
        _shieldLives = 3;
        _shield.gameObject.SetActive(true);
        _shieldActive = true;
        _shieldSpriteRenderer.color = defaultShieldColor;
        
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    IEnumerator TripleShotCoroutine()
    {
       if(_tripleShotActive)
        {
            yield return new WaitForSeconds(5f);
            _tripleShotActive = false;
        }
    }

    IEnumerator SpeedActiveCoroutine()
    {
        if(_speed == 12f)
        {
            yield return new WaitForSeconds(5f);
            _speed = 6f;
            _speedActive = false;
        }
    }

    IEnumerator ShieldRoutine1()
    {
        while (_shieldLives == 1)
        {
            _shield.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.1f);
            _shield.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);

        }
    }

    IEnumerator UltiRoutine()
    {
        while(_ultiActive)
        {
            yield return new WaitForSeconds(5f);
            _ultiActive = false;
        }        
    }

    IEnumerator ThrustReloadRoutine()
    {
        while(!_thrustActive && _thrustLeft <1)
        {
            _thrustLeft = _thrustLeft + (0.05f / 10);
            if(_thrustLeft >1)
            {
                _thrustLeft = 1;
            }
            _uiManager.SetThruster(_thrustLeft);
            yield return new WaitForSeconds(0.05f);
        }
    }

    IEnumerator UseThrustRoutine()
    {
        while(_thrustActive)
        {
            _thrustLeft = _thrustLeft - (0.05f / 60);
            _uiManager.SetThruster(_thrustLeft);
            yield return new WaitForSeconds(0.05f);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "EnemyLaser")
        {
            Destroy(other.gameObject);
        }
        
    }

    public void BossAmmoUnlimited()
    {
        _ammo = 999;
    }

}
