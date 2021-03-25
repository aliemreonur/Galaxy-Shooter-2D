using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float _xAxis;  
    private float _yAxis;

    [SerializeField] private GameObject _laser, _tripleLaser;
    [SerializeField] private GameObject _rightFire, _leftFire, _explosion;
    [SerializeField] private float _speed = 6f;
    private float _cooldownTime = 0.2f;
    private float _fireTime;

    [SerializeField] private GameObject _shield;
    private bool _shieldActive;

    [SerializeField]
    private bool _tripleShotActive = false;

    [SerializeField] private int _lives = 3;
    [SerializeField] private int _score = 0;

    UIManager _uiManager;

    public int Live
    {
        get
        {
            return _lives;
        }      
    }

    public int Score
    {
        get
        {
            return _score;
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
    }

    private void Fire()
    {
        _fireTime = Time.time + _cooldownTime;
        if(!_tripleShotActive)
        {
            Instantiate(_laser, new Vector3(transform.position.x, transform.position.y + 1f, 0), Quaternion.identity);
        }
        else
        {
            Instantiate(_tripleLaser, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
        }
        
    }

    private void Movement()
    {
        _xAxis = Input.GetAxis("Horizontal");
        _yAxis = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(_xAxis, _yAxis, 0);

        transform.Translate(direction * _speed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -4.8f, 0), 0);

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
            _shieldActive = false;
            _shield.gameObject.SetActive(false);
        }
        else
        {
            _lives--;
            _uiManager.UpdateLives(_lives);
            switch (_lives)
            {
                case 2:
                    _rightFire.gameObject.SetActive(true);
                    break;
                case 1:
                    _leftFire.gameObject.SetActive(true);
                    break;
                case 0:
                    Instantiate(_explosion, transform.position, Quaternion.identity);
                    _uiManager.GameOver();
                    Destroy(this.gameObject);
                    break;
            }
        }      
    }

    public void TripleShotActive()
    {
        _tripleShotActive = true;
        StartCoroutine(TripleShotCoroutine());
    }

    public void SpeedActive()
    {
        _speed = 10f;
        StartCoroutine(SpeedActiveCoroutine());
    }

    public void ShieldActive()
    {
        _shieldActive = true;
        _shield.gameObject.SetActive(true);
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
        if(_speed == 10f)
        {
            yield return new WaitForSeconds(5f);
            _speed = 6f;
        }
    }

}
