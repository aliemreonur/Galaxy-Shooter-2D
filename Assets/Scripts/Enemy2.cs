using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    //this should be better in a single enemy script or inherited from the enemy
    [SerializeField] GameObject _enemy2Laser, _explosion;
    Player _player;
    AudioSource _audioSource;
    SpawnManager _spawnManager;

    private float _coolDown = 1.5f;
    private float _canFire = -1f;

    private float _enemy2Speed = 3f;

    [SerializeField] Sprite _laserSprite;
    [SerializeField] private GameObject _enemyShield;

    private int _shieldDecider;
    private bool _shieldOn = false;

    private float _distanceToPlayer;
    private bool _ramming = false;
    private bool _ascending = false;
    private bool _isDead = false;
    private bool shootingPowerUp = false;

    int _multiplier = -1;

    int _enemyId = 1;

    Laser laser;


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


        ShieldEnemy();

        transform.position = new Vector3(-11f, 6f, 0);

        _enemyId = Random.Range(1, 3);
        if(_enemyId == 2)
        {
            //can fire backwards bool true
            //buna özel bir sprite
            //player obje arkasında mı nasıl anlayacak? : raycasting

            //
        }
    }

    private void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 7f, 1 << 10);
        Debug.DrawRay(transform.position, Vector2.down * 7, Color.green);
        //does not make much sense for dodging - better to use collider in child tho

        if (hit.collider != null)
        {

            if (hit.collider.name == "Ammo_PowerUp(Clone)" || hit.collider.name == "Life_PowerUp(Clone)" || hit.collider.name == "MultiShoot_PowerUp(Clone)" ||
                hit.collider.name == "Shield_PowerUp(Clone)" || hit.collider.name == "Speed_PowerUp(Clone)" || hit.collider.name == "Triple_Shot_PowerUp(Clone)")
            {
                if (!shootingPowerUp)
                {
                    StartCoroutine(PowerUpShoot());
                    Debug.Log("Enemy trying to  hit the power up bro");

                }
            }
        }

        else
        {
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {

        EnemyFire();
        if(!_ramming)
        {
            Movement();
        }

        _distanceToPlayer = Vector3.Distance(transform.position, _player.transform.position);
        if (_distanceToPlayer < 3)
        {
            RamPlayer();
        }
        else if(_distanceToPlayer > 4)
        {
            _ramming = false;
        }
    }

    public void RamPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, _enemy2Speed * Time.deltaTime);
        _ramming = true;
    }

    private void Movement()
    {

        transform.Translate(new Vector3(1, _multiplier, 0) * _enemy2Speed * Time.deltaTime);

        if (transform.position.y >= 4.5f)
        {
            _multiplier = -1;
        }
        else if(transform.position.y <= 0.5f)
        {
            _multiplier = 1;
        }

        if(transform.position.x >= 10.25f)
        {
            transform.position = new Vector3(-10.25f, transform.position.y, 0);
        } 
    }

    private void EnemyFire()
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
                Vector2 _laserColliderSize = lasers[i].GetComponent<CapsuleCollider2D>().size;
                _laserColliderSize = new Vector2(_laserColliderSize.x / 2, _laserColliderSize.y / 4);
                lasers[i].GetComponent<CapsuleCollider2D>().size = _laserColliderSize;
                //2 getcomponents - seems very ineffective.
            }
        }
    }

    public void ShieldEnemy()
    {
        switch (_spawnManager.Wave)
        {
            case 3:
                _shieldDecider = Random.Range(0, 2);
                if (_shieldDecider == 1)
                {
                    _enemyShield.gameObject.SetActive(true);
                    _shieldOn = true;
                }

                break;
            case 4:
                _enemyShield.SetActive(true);
                _shieldOn = true;
                break;
            default:
                _enemyShield.gameObject.SetActive(false);
                _shieldOn = false;
                break;

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
            if (_shieldOn)
            {
                _enemyShield.gameObject.SetActive(false);
                _shieldOn = false;
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
            if (_shieldOn)
            {
                _enemyShield.gameObject.SetActive(false);
                _shieldOn = false;
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


    IEnumerator PowerUpShoot()
    {
        _canFire = 0;
        shootingPowerUp = true;
        EnemyFire();
        yield return new WaitForSeconds(2f);
        shootingPowerUp = false;
        yield return new WaitForSeconds(0.5f);
    }

}
