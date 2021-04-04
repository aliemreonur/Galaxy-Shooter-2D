using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
    private int _health = 100;
    private float _bossSpeed = 2f;

    int _randomShootValue;

    private bool _onPoint = false;

    [SerializeField] GameObject _bossLaser;
    [SerializeField] GameObject _bossUltiShot;

    Player _player;
    UIManager _uiManager;

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
        transform.position = new Vector3(0, 7, 0);
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
        _uiManager.BossHealth(100);
        _pos = new Vector3(0, 2.5f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void BossMovement()
    {
        if(!_onPoint)
        {
            transform.position = Vector3.MoveTowards(transform.position, _pos, _bossSpeed * Time.deltaTime);

            if(transform.position == _pos)
            {
                _onPoint = true; 
            }
        }
        else
        {
            //2 tane point belirle, bunlar arasında movetowards dene
        }

    }

    void BossFire()
    {
        //normal laser tarzı
        //belirli bir sürede 1 ulti

    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            _player.Damage();   
        }
        else if(other.gameObject.tag == "Laser")
        {
            _randomShootValue = Random.Range(3, 6);
            _health -= _randomShootValue;
            _uiManager.BossHealth(_health);

            if(_health <= 0)
            {
                _health = 0;
                Destroy(this.gameObject,0.5f);
            }
            
        }
        
    }

    IEnumerator GotShotRoutine()
    {
        //color1
        yield return new WaitForSeconds(0.5f);
        //color2
        yield return new WaitForSeconds(0.5f);
        
    }
}
