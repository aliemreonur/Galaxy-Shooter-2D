using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField] private float _speed = 3f;
    [SerializeField] private int _powerUpID;

    Player player;

    AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        if(!player)
        {
            Debug.LogError("Power up script could not gather the player script");
        }

        _audioSource = GetComponent<AudioSource>();
        if(_audioSource == null)
        {
            Debug.LogError("Power Up Script could not get the audio source, it is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if(transform.position.y < - 8)
        {
            Destroy(this.gameObject);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.tag == "Player")
        {
            _audioSource.Play();
            switch (_powerUpID)
            {
                case 1:
                    player.TripleShotActive();
                    break;
                case 2:
                    player.SpeedActive();
                    break;
                case 3:
                    player.ShieldActive();
                    break;
                default:
                    Debug.Log("Error! - CHECK!");
                    break;
            }
            
            Destroy(this.gameObject, 0.25f);
        }

    }

}
