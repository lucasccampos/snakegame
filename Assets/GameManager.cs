using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Instância única do GameManager
    private int score; // Score atual
    private int highScore; // Highscore

    public TMP_Text scoreText; // Texto para exibir o score
    public TMP_Text highScoreText; // Texto para exibir o highscore
    public TMP_Text endGameText; // Texto para exibir o highscore

    public bool isGameOver = true; // Flag para indicar se o jogo acabou

    public delegate void GameStartDelegate();
    public static event GameStartDelegate OnGameStart;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Carregar o highscore salvo (caso exista)
        highScore = PlayerPrefs.GetInt("HIGH", 0);
        RefreshText();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
            return;
        }

        if (isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartGame();
            }
        }
    }


    private void StartGame()
    {
        isGameOver = false;
        endGameText.gameObject.SetActive(false);
        ResetScore();

        OnGameStart?.Invoke();
    }

    public void EndGame()
    {
        isGameOver = true;
        endGameText.gameObject.SetActive(true);
    }

    public void ResetScore()
    {
        score = 0;
        RefreshText();
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void AddScore(int points)
    {
        score += points;

        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HIGH", highScore);
        }

        RefreshText();
    }

    public void RefreshText()
    {
        scoreText.text = "SCORE: " + score.ToString();
        highScoreText.text = "HIGH: " + highScore.ToString();
    }
}