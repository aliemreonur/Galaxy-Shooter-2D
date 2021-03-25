using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private float _rotateSpeed = 3.0f;
    [SerializeField] private GameObject _explosion;

    SpawnManager _spawnManager;
    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if(!_spawnManager)
        {
            Debug.LogError("Asteroid could not get the spawn manager");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * _rotateSpeed *Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Laser")
        {
            GameObject explosion = Instantiate(_explosion, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            _spawnManager.StartSpawn();
            Destroy(this.gameObject, 0.1f);
        }
    }
}
