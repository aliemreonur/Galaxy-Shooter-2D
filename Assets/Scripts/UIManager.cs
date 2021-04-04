using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Text _scoreText;
    [SerializeField] Slider _slider;
    [SerializeField] Slider _bossHealth;
    Player player;

    [SerializeField] private Sprite[] _liveSprites;
    [SerializeField] private Image _livesImg;

    [SerializeField] private Text _gameOverText;
    [SerializeField] private Text _restartLevelText;
    [SerializeField] private Text _ammmoText;
    [SerializeField] private Text _waveNo;
    [SerializeField] private Text _startToShoot;

    private bool _isGameStarted = false;

    GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        if(!player)
        {
            Debug.LogError("UI Manager Could not get the player");
        }
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if(!_gameManager)
        {
            Debug.LogError("UI manager could not get the scene manager");
        }
        _scoreText.text = "Score : " + 0 ;
        _livesImg.sprite = _liveSprites[3];
        _gameOverText.gameObject.SetActive(false);
        _restartLevelText.gameObject.SetActive(false);

        StartCoroutine(StartToShootRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        //_scoreText.text = "Score : " + player.Score;
        
    }

    public void UpdateScore(int _score)
    {
        _scoreText.text = "Score " + _score;
    }

    public void UpdateLives(int lives)
    {
        if(lives >= 0)
        {
            _livesImg.sprite = _liveSprites[lives];
        }
        
    }

    public void GameOver()
    {  
        _restartLevelText.gameObject.SetActive(true);
        StartCoroutine(GameOverRoutine());
        _gameManager.GameOver();
        
    }

    public void UpdateAmmo()
    {
        _ammmoText.text = player.Ammo.ToString() + "/15";
        if(player.Ammo <= 0)
        {
            StartCoroutine(AmmoOutRoutine());
        }
    }

    public void SetThruster(float _thrustAmount)
    {
        _slider.value = _thrustAmount;
    }

    public void BossHealth(int bossHealth)
    {
        _bossHealth.gameObject.SetActive(true);
        _bossHealth.value = bossHealth;
    }

    public void SetWave(int waveNumber)
    {
        _waveNo.text = "Wave :" + waveNumber ;
    }

    public void GameStarted()
    {
        _isGameStarted = true;
        //_startToShoot.gameObject.SetActive(false);
    }

    IEnumerator GameOverRoutine()
    {
        while(true)
        {
            _gameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
           
    }


    IEnumerator AmmoOutRoutine()
    {
        while(player.Ammo <= 0)
        {
            _ammmoText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            _ammmoText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
        }
      
    }

    IEnumerator StartToShootRoutine()
    {
        while(!_isGameStarted)
        {
            yield return new WaitForSeconds(0.5f);
            _startToShoot.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            _startToShoot.gameObject.SetActive(false);
        }
    }
}
