using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float _enemySpeed = 4f;
    Player _player;
    Animator _anim;
    AudioSource _audioSource;

    [SerializeField] private AudioClip _audioclip;

    private float _coolDown = 3f;
    private float _canFire = -1f;

    private int _movementDecider;


    [SerializeField] GameObject _enemyLaser;
    
    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        if(!_anim)
        {
            Debug.LogError("HEY! Enemy is missing its animator bro");
        }
        _player = FindObjectOfType<Player>().GetComponent<Player>();

        if(_player == null)
        {
            Debug.LogError("Enemy could not get the player - NULL CHECK");
        }

        _audioSource = GetComponent<AudioSource>();
        if(_audioSource == null)
        {
            Debug.LogError("Enemy could ont get the audio source -  NULL CHECK");
        }

        _movementDecider = Random.Range(1, 4);
        if(_movementDecider == 2)
        {
            transform.position = new Vector3(-10.2f, Random.Range(2f, 4.7f), 0);
        }
        else if(_movementDecider == 3)
        {
            transform.position = new Vector3(10.2f, Random.Range(2f, 4.7f), 0);
        }
     

    }


    // Update is called once per frame
    void Update()
    {
        Movement();
        if(Time.time >_canFire)
        {
            _coolDown = Random.Range(3f, 7f);
            _canFire = Time.time + _coolDown;
            GameObject enemyLaser = Instantiate(_enemyLaser, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            for(int i=0; i< lasers.Length; i++)
            {
                lasers[i].AssignEnemy();
            }
        }

    }

    private void Movement()
    {
        switch(_movementDecider)
        {
            case 1:
                transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);
                break;
            case 2:
                transform.Translate(Vector3.right * _enemySpeed * Time.deltaTime);
                break;
            case 3:
                transform.Translate(Vector3.left * _enemySpeed * Time.deltaTime);
                break;
            
            //will use these on the new waves
                /*case 4:
                transform.Translate(new Vector3(1, -1, 0) *_enemySpeed * Time.deltaTime );
                break;
            case 5:
                transform.Translate(new Vector3(-1, -1, 0) * _enemySpeed * Time.deltaTime);
                break;
            */
        }


        if (transform.position.y < -5.5f)
        {
            float randomX = Random.Range(-9, 9);
            transform.position = new Vector3(randomX, 5.5f, 0);
        }
        else if(transform.position.x < -10.3f)
        {
            transform.position = new Vector3(10.2f, transform.position.y, 0);
        }
        else if(transform.position.x > 10.3f)
        {
            transform.position = new Vector3(-10.2f, transform.position.y, 0);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.gameObject.tag == "Player")
        {
           if (_player)
           {
                _player.Damage();
            }
            _anim.SetTrigger("OnEnemyDeath");
            _enemySpeed = 0;
            _audioSource.Play(); 
            Destroy(this.gameObject, 0.9f);
        }

        else if(other.gameObject.tag == "Laser")
        {
            Destroy(other.gameObject);
            if(_player)
            {
                _player.AddScore(10);
            }
            _anim.SetTrigger("OnEnemyDeath");
            _enemySpeed = 0;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 0.9f);
        }
    }


}
