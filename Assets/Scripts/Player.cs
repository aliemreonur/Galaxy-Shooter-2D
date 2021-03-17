using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float _xAxis;
    private float _yAxis;

    [SerializeField] private float _speed = 4f;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, -3, 0);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();

    }

    private void Movement()
    {
        _xAxis = Input.GetAxis("Horizontal");
        _yAxis = Input.GetAxis("Vertical");

        //Debug.Log("_xAxis value is : " + _xAxis + ", _yAxis value is : " + _yAxis);

        Vector3 direction = new Vector3(_xAxis, _yAxis, 0);

        transform.Translate(direction * _speed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -4.8f, 0), 0);

        /*if(transform.position.y > 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }

        else if (transform.position.y < -4.8f)
        {
            transform.position = new Vector3(transform.position.x, -4.8f, 0);
        }
        */

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
