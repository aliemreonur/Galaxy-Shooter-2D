using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    //[SerializeField] private Enemy enemy;
    [SerializeField] private GameObject[] enemy;
    [SerializeField] GameObject[] _powerUps;

    [SerializeField] private int _selectedPowerUp;

    Vector3 _posToSpawn, _pos2ToSpawn;
    [SerializeField] float _spawnTime = 3f;
    Player player;
    [SerializeField] Transform enemyContainer;

    private int _ultiCoolDown = 20;
    private float _latestSpawnedUlti = 0;

    private int _numSpawnedEnemy;
    private int _wave = 1;

    private bool _ctdSpawn = true;

    [SerializeField] private int _activeEnemy;

    UIManager _uiManager;

    public int Wave
    {
        get
        {
            return _wave;
        }
    }

    public int ActiveEnemy
    {
        get
        {
            return _activeEnemy;
        }
        set
        {
            _activeEnemy = value;
        }
    }
 
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        if(player == null)
        {
          Debug.LogError("Spawn Manager could not get the player");
        }

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if(_uiManager == null)
        {
            Debug.LogError("Spawn Manager could not get the ui manager");
        }
        
    }

    public void StartSpawn()
    {
        _uiManager.GameStarted();
        player.AddAmmo(); //we make sure that player starts the game with full ammo.
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        while(player.Live > 0 )
        {
            yield return new WaitForSeconds(2f);
            
                _posToSpawn = new Vector3(Random.Range(-9, 9), 7f, 0);
            if(_ctdSpawn)
            {
                int selectedEnemy = Random.Range(0, 3);
                //to spawn the new enemy for 1/3 of the time.
             
                if(selectedEnemy == 1)
                {
                    GameObject spawnedEnemy2 = Instantiate(enemy[1], new Vector3(-10,5,0), Quaternion.identity);
                    //spawnedEnemy.transform.parent = enemyContainer.transform;
                    //***
                    //when this is activated, the object becomes invisible on the game view (but visible on scene view)
                    //***
                }
                else 
                {
                    GameObject spawnedEnemy = Instantiate(enemy[0], _posToSpawn, Quaternion.identity);
                    spawnedEnemy.transform.parent = enemyContainer.transform;
                }
                _numSpawnedEnemy++;
                _activeEnemy++;
            }


            if (_numSpawnedEnemy == 10)
            {
                    _ctdSpawn = false;
                    if (_activeEnemy == 0)
                    {
                        _wave = 2;
                        _uiManager.SetWave(_wave);
                        _ctdSpawn = true;
                        _spawnTime = 1.75f;
                    }
            }

            else if (_numSpawnedEnemy == 25)
            {
                _ctdSpawn = false;
                if (_activeEnemy == 0)
                {
                    _wave = 3;
                    _uiManager.SetWave(_wave);
                    _ctdSpawn = true;
                    _spawnTime = 1.5f;
                }
            }

            else if (_numSpawnedEnemy == 45)
            {
                _ctdSpawn = false;
                if (_activeEnemy == 0)
                {
                    _wave = 4;
                    _uiManager.SetWave(_wave);
                    _ctdSpawn = true;
                    _spawnTime = 1.25f;
                }
            }
            else if (_numSpawnedEnemy == 70)
            {
                _ctdSpawn = false;
                if (_activeEnemy == 0)
                {
                     _wave = 5;
                     _uiManager.SetWave(_wave);
                     //boss level
                }
            }
            yield return new WaitForSeconds(_spawnTime);
        } 
    }
    
    IEnumerator SpawnPowerUpRoutine()
    {
        while(player.Live>0)
        {
            yield return new WaitForSeconds(2f);
            _selectedPowerUp = Random.Range(0, 7);
            _pos2ToSpawn = new Vector3(Random.Range(-9, 9), 7f, 0);
            if(_selectedPowerUp == 5)
            {
                if(Time.time > _latestSpawnedUlti)
                {
                    Instantiate(_powerUps[5], _pos2ToSpawn, Quaternion.identity);
                    _latestSpawnedUlti = Time.time + _ultiCoolDown;
                }
                else
                {
                    _selectedPowerUp = Random.Range(0, 7);
                }
            }
            else
            {
                Instantiate(_powerUps[_selectedPowerUp], _pos2ToSpawn, Quaternion.identity);
            }
            yield return new WaitForSeconds(Random.Range(5f, 7f));    
        }
    }
}
