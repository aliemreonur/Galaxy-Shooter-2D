using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField] private float _speed = 3f;
    [SerializeField] private int _powerUpID;

    Player player;
    
    [SerializeField] private AudioClip _audioclip;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        if(player == null)
        {
            Debug.LogError("Power up script could not gather the player script");
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

        if(Input.GetKey(KeyCode.C))
        {
            MoveToPlayer();
        }

    }

    public void MoveToPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, 4f * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.tag == "Player")
        {
            AudioSource.PlayClipAtPoint(_audioclip, transform.position);
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
                case 4:
                    player.AddLife();
                    break;
                case 5:
                    player.AddAmmo();
                    break;
                case 6:
                    player.UltiActive();
                    break;
                case 7:
                    player.Damage();
                    break;
                default:
                    Debug.Log("Error! - CHECK THE POWERUP SCRIPT!");
                    break;
            }
            
            Destroy(this.gameObject, 0.05f);
        }

    }


}
