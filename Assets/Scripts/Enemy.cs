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
    [SerializeField] private GameObject _enemyShield;
    [SerializeField] private Sprite _enemy3;

    private float _coolDown = 3f;
    private float _canFire = -1f;

    private bool _isDead = false; //this is for making sure that enemy will not fire after its dead
    private bool _shieldOn = false;
    private bool _evading = false;
    private bool _shootPowerUp = false;
    private bool _shootPlayerUp = false;

    private int _movementDecider;
    private int _shieldDecider;

    private float _distanceToPlayer;

    SpawnManager _spawnManager;

    [SerializeField] private int _enemyTypeDecider;

    public bool isDead
    {
        get { return _isDead; }
    }
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

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if(_spawnManager == null)
        {
            Debug.LogError("Enemy could not get the spawn manager");
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

        _enemyTypeDecider = Random.Range(1, 3);
        if(_enemyTypeDecider == 2)
        {
            GetComponent<SpriteRenderer>().color = Color.cyan;
            //maybe use inherited enemy class on enemy3 and make it another sprite.
        }

        ShieldEnemy();

    }

    private void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 7f, 1<<10);
        Debug.DrawRay(transform.position, Vector2.down*7, Color.green);

        if(hit.collider != null)
        {
            if(hit.collider.name == "Ammo_PowerUp(Clone)" || hit.collider.name == "Life_PowerUp(Clone)" || hit.collider.name == "MultiShoot_PowerUp(Clone)" || 
                hit.collider.name == "Shield_PowerUp(Clone)" || hit.collider.name == "Speed_PowerUp(Clone)" || hit.collider.name == "Triple_Shot_PowerUp(Clone)")
            {
                if(!_shootPowerUp)
                {
                    StartCoroutine(PowerUpShoot()); //tested & works fine
                }
            }
        }

        else
        {
            return;
        }

        if(_enemyTypeDecider == 2) //works fine but need to decrease the enemy ram distance
        {
            RaycastHit2D hit2 = Physics2D.Raycast(transform.position, Vector2.up, 7f, 1 << 10);
            Debug.DrawRay(transform.position, Vector2.up * 7, Color.blue);

            if(hit2.collider != null)
            {
                if (hit2.collider.name == "Player")
                {
                    Debug.Log("I am hitting the player now");
                    StartCoroutine(PlayerShootUp());

                    //enemy fire upwards.
                }
            }

        }
    }


    // Update is called once per frame
    void Update()
    {
        Movement();
        if(_player.Live > 0)
        {
            _distanceToPlayer = Vector3.Distance(transform.position, _player.transform.position);
        }
        if(_distanceToPlayer < 2.5f)
        {
            RamPlayer();
        }

        //Debug.Log("My Distance to enemy is:" + _distanceToPlayer);
        EnemyFire();
    }

    private void EnemyFire()
    {
        if (Time.time > _canFire && !_isDead)
        {
            //make cooldown timer shorter on the next waves
            _coolDown = Random.Range(3f, 7f);
            _canFire = Time.time + _coolDown;
            GameObject enemyLaser = Instantiate(_enemyLaser, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemy();
                if(_shootPlayerUp)
                {
                    lasers[i].EnemyReverseShot = true;
                }
                if(_enemyTypeDecider == 2)
                {
                    lasers[i].GetComponent<SpriteRenderer>().color = Color.green;
                }

            }
        }
    }

    private void Movement()
    {
        if(!_evading)
        {
            switch (_movementDecider)
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


    public void RamPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, _enemySpeed/2 * Time.deltaTime);
    }
    public void ShieldEnemy()
    {
        switch(_spawnManager.Wave)
        {
            case 1:
                _enemyShield.SetActive(false);
                _shieldOn = false;
                break;
            case 2:
                _shieldDecider = Random.Range(0, 2);
                if (_shieldDecider == 1)
                {
                    _enemyShield.gameObject.SetActive(true);
                    _shieldOn = true;
                }
                else
                {
                    _enemyShield.gameObject.SetActive(false);
                    _shieldOn = false;

                }
                break;
           default:
                _enemyShield.gameObject.SetActive(true);
                _shieldOn = true;
                break;

        }
    }

    public void EvadeShot()
    {
        //Debug.Log("Enemy is trying to avoid it bro"); //works fine
        _evading = true;

        //returns null referance if we shot the enemy
        if(_movementDecider == 1)
        {
           //simply do nothing - dont want to make the game too hard 
        }
        else if (_movementDecider == 2)
        {
            transform.Translate(Vector3.left * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.right * Time.deltaTime); //stand still until the laser goes away
        }
    }

    public void EvadeOver()
    {
        _evading = false;
        //Debug.Log("Evading is over");
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.gameObject.tag == "Player")
        {
             _player.Damage();
           
           if (_shieldOn)
            {
                _enemyShield.gameObject.SetActive(false);
                _shieldOn = false;
           }
           else
           {
                
                _anim.SetTrigger("OnEnemyDeath");
                _enemySpeed = 0;
                _audioSource.Play();
                _spawnManager.ActiveEnemy--;
                Destroy(GetComponent<BoxCollider2D>());
                _isDead = true;
                Destroy(this.gameObject, 0.9f);
           }
        
        }

        else if(other.gameObject.tag == "Laser")
        {
            Destroy(other.gameObject);
            if (_shieldOn)
            {
                _enemyShield.gameObject.SetActive(false);
                _shieldOn = false;
            }
            else 
            {

                 _player.AddScore(10);

                _anim.SetTrigger("OnEnemyDeath");
                _enemySpeed = 0;
                _audioSource.Play();
                _spawnManager.ActiveEnemy--;
                Destroy(GetComponent<BoxCollider2D>());
                _isDead = true;
                Destroy(this.gameObject, 0.9f);

            }
     
        }
    }

    IEnumerator PowerUpShoot()
    {
        _canFire = 0;
        _shootPowerUp = true;
        EnemyFire();
        yield return new WaitForSeconds(2f);
        _shootPowerUp = false;
        yield return new WaitForSeconds(0.5f);
    }

    IEnumerator PlayerShootUp()
    {
        _canFire = 0;
        _shootPlayerUp = true;
        EnemyFire();
        yield return new WaitForSeconds(2f);
        _shootPlayerUp = false;
        yield return new WaitForSeconds(0.5f);
    }
}