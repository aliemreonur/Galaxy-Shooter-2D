using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private Enemy enemy;
    [SerializeField] GameObject[] _powerUps;

    private int _selectedPowerUp;

    Vector3 _posToSpawn, _pos2ToSpawn;
    [SerializeField] float _spawnTime = 3f;
    Player player;
    [SerializeField] Transform enemyContainer;
 
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        if(player == null)
        {
          Debug.LogError("Spawn Manager could not get the player");
        }
        
    }

    public void StartSpawn()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }

    // Update is called once per frame
    void Update()
    {
       
        
    }

    IEnumerator SpawnEnemyRoutine()
    {
        while(player.Live > 0 )
        {
            yield return new WaitForSeconds(2f);
            _posToSpawn = new Vector3(Random.Range(-9, 9), 7.20f, 0);
            Enemy spawnedEnemy = Instantiate(enemy, _posToSpawn, Quaternion.identity);
            spawnedEnemy.transform.parent = enemyContainer.transform;
            yield return new WaitForSeconds(_spawnTime);
        } 
    }

    IEnumerator SpawnPowerUpRoutine()
    {
        while(player.Live>0)
        {
            yield return new WaitForSeconds(2f);
            _selectedPowerUp = Random.Range(0, 3);
            _pos2ToSpawn = new Vector3(Random.Range(-9, 9), 7.20f, 0);
            Instantiate(_powerUps[_selectedPowerUp], _pos2ToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(5f, 7f));    
        }
    }
}
