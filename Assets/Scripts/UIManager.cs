using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Text _scoreText;
    Player player;

    [SerializeField] private Sprite[] _liveSprites;
    [SerializeField] private Image _livesImg;

    [SerializeField] private Text _gameOverText;
    [SerializeField] private Text _restartLevelText;

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
        _livesImg.sprite = _liveSprites[lives];
    }

    public void GameOver()
    {  
        _restartLevelText.gameObject.SetActive(true);
        StartCoroutine(GameOverRoutine());
        _gameManager.GameOver();
        
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
}
