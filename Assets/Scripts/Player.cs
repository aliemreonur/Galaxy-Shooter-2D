using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float _xAxis;  
    private float _yAxis;

    [SerializeField] private GameObject _laser;
    [SerializeField] private float _speed = 4f;


    private float _cooldownTime = 0.5f;
    private float _fireTime;

    [SerializeField] private int _lives = 3;

    public int Lives
    {
        get
        {
            return _lives;
        }
        set
        {
            _lives = value;
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, -3, 0);
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
        Instantiate(_laser, new Vector3(transform.position.x, transform.position.y + 0.6f,0), Quaternion.identity);
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
}
