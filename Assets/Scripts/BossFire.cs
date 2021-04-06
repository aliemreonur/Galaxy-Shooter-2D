using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFire : MonoBehaviour
{
    Player _player;
    [SerializeField] float _bossFireSpeed = 4.5f;
    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<Player>().GetComponent<Player>();
        if(_player == null)
        {
            Debug.LogError("Boss projectile could not get the player");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _bossFireSpeed * Time.deltaTime);

        if(transform.position.y < -10)
        {
            Destroy(this.gameObject);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            _player.Damage();
            Destroy(this.gameObject);
        }
    }
}
