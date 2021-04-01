using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    //this should be better if it inherited from the enemy
    [SerializeField] GameObject _enemy2Laser, _explosion;
    Player _player;
    AudioSource _audioSource;
    SpawnManager _spawnManager;

    private float _coolDown = 1.5f;
    private float _canFire = -1f;

    [SerializeField] Sprite _laserSprite;
    [SerializeField] private GameObject _enemyShield;

    private int _shieldDecider;

    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<Player>().GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Enemy could not get the player - NULL CHECK");
        }
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("Enemy could ont get the audio source -  NULL CHECK");
        }
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("Enemy could not get the spawn manager");
        }

        _enemyShield.gameObject.SetActive(false);
        _shieldDecider = Random.Range(0, 2);
        if (_shieldDecider == 1)
        {
            _enemyShield.gameObject.SetActive(true);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > _canFire)
        {
            _coolDown = Random.Range(1f, 3f);
            _canFire = Time.time + _coolDown;
            GameObject enemyLaser = Instantiate(_enemy2Laser, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemy();
                lasers[i].GetComponent<SpriteRenderer>().sprite = _laserSprite;
                lasers[i].transform.localScale *= 4;
            }
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
            if (_shieldDecider == 1)
            {
                _enemyShield.gameObject.SetActive(false);
                _shieldDecider = 0;
            }
            else
            {
                _audioSource.Play();
                _spawnManager.ActiveEnemy--;
                Destroy(GetComponent<SpriteRenderer>(), 0.1f);
                Instantiate(_explosion, transform.position, Quaternion.identity);
                Destroy(this.gameObject, 0.1f);
            }

        }

        else if (other.gameObject.tag == "Laser")
        {
            Destroy(other.gameObject);
            if (_shieldDecider == 1)
            {
                _enemyShield.gameObject.SetActive(false);
                _shieldDecider = 0;
            }
            else
            {
                if (_player)
                {
                    _player.AddScore(10);
                }
                _audioSource.Play();
                _spawnManager.ActiveEnemy--;
                Instantiate(_explosion, transform.position, Quaternion.identity);
                Destroy(GetComponent<SpriteRenderer>(), 0.1f);
                Destroy(this.gameObject, 0.1f);
            }
   
        }
    }

}
