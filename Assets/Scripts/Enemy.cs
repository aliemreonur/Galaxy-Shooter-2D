using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float _enemySpeed = 4f;
    Player _player;
    Animator _anim;
    
    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        if(!_anim)
        {
            Debug.LogError("HEY! Enemy is missing its animator bro");
        }
        _player = FindObjectOfType<Player>().GetComponent<Player>();

        if(!_player)
        {
            Debug.LogError("Enemy could not get the player - NULL CHECK");
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);
        
        if(transform.position.y < -5.5f)
        {
            float randomX = Random.Range(-9, 9);
            transform.position = new Vector3(randomX, 5.5f, 0);
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
            Destroy(this.gameObject, 0.5f);
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
            Destroy(this.gameObject, 0.5f);
        }
    }
}
