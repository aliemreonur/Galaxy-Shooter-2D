using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private Enemy enemy;
    Vector3 _posToSpawn;
    [SerializeField] float _spawnTime = 3f;
    Player player;
 
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        if(player)
        {
            StartCoroutine(SpawnRoutine());
        }
        
    }

    // Update is called once per frame
    void Update()
    {
       
        
    }

    IEnumerator SpawnRoutine()
    {
        while(player.Live > 0)
        {
            _posToSpawn = new Vector3(Random.Range(-9, 9), 6.20f, 0);
            Instantiate(enemy, _posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(_spawnTime);
        } 
    }

}
