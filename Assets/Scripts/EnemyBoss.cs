using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
    private int _health = 100;
    private float _bossSpeed = 2f;

    float _canFire = 0;
    float _fireCoolDown = 5f;

    int _randomShootValue;

    private bool _onPoint = false;
    private bool _ultiOn;
    private bool _ulti1Over;
    private bool _gotHit;
    private bool _alive = true;

    [SerializeField] GameObject _bossLaser;
    //[SerializeField] GameObject _bossUltiShot;

    Player _player;
    UIManager _uiManager;

    SpriteRenderer _spriteRenderer;
    [SerializeField] GameObject _explosion;


    Vector3 _pos;

    public int Health
    {
        get
        {
            return _health;
        }
        set
        {
            _health = value;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 8, 0);
        _player = FindObjectOfType<Player>().GetComponent<Player>();
        if(_player == null)
        {
            Debug.LogError("Boss could not get the player script");
        }
        _uiManager = FindObjectOfType<Canvas>().GetComponent<UIManager>();
        if(_uiManager == null)
        {
            Debug.LogError("Boss could not get the uimanager script");
        }
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if(_spriteRenderer == null)
        {
            Debug.LogError("Enemy boss could not get its sprite renderer");
        }


        _uiManager.BossHealth(100);
        _pos = new Vector3(0, 2, 0);

    }

    // Update is called once per frame
    void Update()
    {
        BossMovement();
        if(_onPoint) BossFire();
        if(_health <= 50 && !_ulti1Over )
        {
            _ultiOn = true;
        }

    }

    void BossMovement()
    {
        if(!_onPoint)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(0,2,0), _bossSpeed * Time.deltaTime);

            if(transform.position == _pos)
            {
                _onPoint = true;
                _player.BossAmmoUnlimited();
            }
        }
    }

    void BossFire()
    {
        if(Time.time > _canFire && _alive)
        {
            if(!_ultiOn)
            {
                Instantiate(_bossLaser, new Vector2(Mathf.Clamp(_player.transform.position.x, -5f, 5f), 2), Quaternion.identity);
                _canFire = Time.time + _fireCoolDown;
            }
            else
            {
                StartCoroutine(UltiFireRoutine());
                _canFire = Time.time + _fireCoolDown;
            }
        }

    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player" && _onPoint)
        {
            _player.Damage();   
        }
        else if(other.gameObject.tag == "Laser" && _onPoint)
        {
            _gotHit = true;
            Destroy(other.gameObject);
            StartCoroutine(GotShotRoutine());
            _randomShootValue = Random.Range(3, 6);
            _health -= _randomShootValue;
            _uiManager.BossHealth(_health);

            if(_health <= 0)
            {
                _health = 0;
                _alive = false;
                StartCoroutine(ExplodeRoutine());
                Destroy(this.gameObject,6f);
            }
            
        }
        
    }

    IEnumerator GotShotRoutine()
    {
        if(_gotHit)
        {
            for(int i = 0; i<3; i++)
            {
                _spriteRenderer.color = Color.red;
                yield return new WaitForSeconds(0.15f);
                _spriteRenderer.color = Color.white;
                yield return new WaitForSeconds(0.15f);
            }
            _gotHit = false;
        }

    }

    IEnumerator ExplodeRoutine()
    {
        for (int i = 0; i < 7; i++)
        {
            Instantiate(_explosion, new Vector3(-3 + i, Random.Range(1,4), 0), Quaternion.identity);
            yield return new WaitForSeconds(1f);
        }

    }


    IEnumerator UltiFireRoutine()
    {
        while(_ultiOn)
        {
            for(int a =0; a<2; a++)
            {
                for (int i = -5; i < 5; i++)
                {
                    Instantiate(_bossLaser, new Vector2(i, -1), Quaternion.identity);
                    yield return new WaitForSeconds(0.25f);
                }

            }
            _ultiOn = false;
            _ulti1Over = true;
        }
    }
}
