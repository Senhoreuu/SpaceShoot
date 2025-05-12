using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private bool gameStarted = false;
    private bool gameOver = false;
    private bool gamePaused = false;

    public AudioSource musicaDoJogo;
    public AudioSource musicaDeGameOver;

    public Text textoDePontuacaoAtual;
    public GameObject painelDeFinalDeJogo;
    public Text textoDeFinalDeJogo;
    public Text textoDePontuacaoFinal;
    public Text textoDeHighScore;

    public int pontuacaoAtual;

    public int inimigosDerrotados;
    public int inimigosDerrotadosMaximo;
    public int[] inimigosParaDerrotar;

    private bool _isInfiniteMode;

    public GameObject menuPanel;

    public GameObject enemyStats;
    public GameObject[] enemiesStatsByPhase;
    public Text[] enemyStatsText;

    public Button PauseGameButton;

    public GameObject[] objectsToShowOnStart;
    public GameObject[] objectsToShowOnPause;

    public Button restartGameButton;
    public Button nextPhaseButton;
    public Text textoFinalDeFase;

    public GameObject dialogPanel;

    public string[] dialogsPlayer;
    public string[] dialogsSecondary;
    
    private int phase;
    private int dialogIndex;
    private const int PhaseMax = 3;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        ClearScene();
        MostrarMenu();
    }

    public void StartInfiniteMode(bool restartScene)
    {
        if (restartScene)
            ClearScene();
        
        Time.timeScale = 1f;

        _isInfiniteMode = true;
        gameStarted = true;
        gameOver = false;
        gamePaused = false;
        pontuacaoAtual = 0;
        inimigosDerrotados = 0;
        textoDePontuacaoAtual.text = "PONTUAÇÃO: " + pontuacaoAtual;
        musicaDoJogo.Play();
        painelDeFinalDeJogo.SetActive(false);
        FindFirstObjectByType<VidaDoJogador>().ResetStats();
        EsconderMenu();
    }

    public void StartHistoryMode(bool restartScene)
    {
        if (restartScene)
            ClearScene();

        if (phase >= PhaseMax) phase = 0;
        
        Time.timeScale = 1f;

        _isInfiniteMode = false;
        gameStarted = true;
        gameOver = false;
        gamePaused = false;
        pontuacaoAtual = 0;
        inimigosDerrotados = 0;
        inimigosDerrotadosMaximo = inimigosParaDerrotar[phase];
        textoDePontuacaoAtual.text = "PONTUAÇÃO: " + pontuacaoAtual;
        enemyStats.SetActive(true);
        enemyStatsText[phase].text = inimigosDerrotados + "/" + inimigosDerrotadosMaximo;
        enemiesStatsByPhase[phase].SetActive(true);
        musicaDoJogo.Play();
        painelDeFinalDeJogo.SetActive(false);
        FindFirstObjectByType<VidaDoJogador>().ResetStats();
        EsconderMenu();
    }

    public void AumentarPontuacao(int pontosParaGanhar)
    {
        pontuacaoAtual += pontosParaGanhar;
        textoDePontuacaoAtual.text = "PONTUAÇÃO: " + pontuacaoAtual;
    }

    public void AumentarInimigosDerrotados(int value)
    {
        inimigosDerrotados += value;
        
        enemyStatsText[phase].text = inimigosDerrotados + "/" + inimigosDerrotadosMaximo;
        
        if (inimigosDerrotados >= inimigosDerrotadosMaximo)
        {
            CompletePhase();
        }
    }

    private void StartFinalBoss()
    {
        
    }

    private void Dialog(string text, bool isPlayerDialog)
    {
        
    }
    
    public bool IsInfiniteMode()
    {
        return _isInfiniteMode;
    }

    public void MostrarMenu()
    {
        Time.timeScale = 0f;
        musicaDeGameOver.Stop();
        musicaDoJogo.Play();
        painelDeFinalDeJogo.SetActive(false);

        menuPanel.SetActive(true);
        gameStarted = false;
        gameOver = false;
        pontuacaoAtual = 0;
        inimigosDerrotados = 0;
        textoDePontuacaoAtual.text = "PONTUAÇÃO: " + pontuacaoAtual;
        FindFirstObjectByType<VidaDoJogador>().ResetStats();
        ClearScene();
        
        foreach (var obGameObject in objectsToShowOnStart)
        {
            obGameObject.gameObject.SetActive(false);
        }
        
        foreach (var obGameObject in objectsToShowOnPause)
        {
            obGameObject.gameObject.SetActive(false);
        }
    }

    public void EsconderMenu()
    {
        foreach (var obGameObject in objectsToShowOnStart)
        {
            obGameObject.gameObject.SetActive(true);
        }
        
        foreach (var obGameObject in objectsToShowOnPause)
        {
            obGameObject.gameObject.SetActive(false);
        }
        
        menuPanel.SetActive(false);
    }

    private void CompletePhase()
    {
        if (phase >= PhaseMax || IsInfiniteMode()) return;
        
        // mostrar o painel de fase completa
        Time.timeScale = 0f;
        musicaDoJogo.Stop();
        
        painelDeFinalDeJogo.SetActive(true);
        gameOver = true;
        gameStarted = false;
        
        textoDeFinalDeJogo.text = "Fase %p completada!".Replace("%p", (++phase).ToString());
        
        if (phase < PhaseMax)
        {
            textoDePontuacaoFinal.text = "PONTUAÇÃO: " + pontuacaoAtual;
            textoDeHighScore.text = "INIMIGOS DERROTADOS: " + inimigosDerrotados;
            
            textoDeFinalDeJogo.text = "Avançar";
        }
        else
        {
            textoDePontuacaoFinal.text = "PONTUAÇÃO: " + pontuacaoAtual;
            textoDeHighScore.text = "FIM DE JOGO! Você chegou ao final do jogo!";

            textoDeFinalDeJogo.text = "Créditos";
        }
        
        restartGameButton.gameObject.SetActive(false);
        nextPhaseButton.gameObject.SetActive(true);
    }

    public void GameOver()
    {
        if (!gameStarted) return;
        
        Time.timeScale = 0f;
        musicaDoJogo.Stop();
        musicaDeGameOver.Play();

        painelDeFinalDeJogo.SetActive(true);

        gameOver = true;
        gameStarted = false;

        textoDeFinalDeJogo.text = "GAME OVER";

        if (IsInfiniteMode())
        {
            textoDePontuacaoFinal.text = "PONTUAÇÃO: " + pontuacaoAtual;

            if (pontuacaoAtual > PlayerPrefs.GetInt("HighScore"))
            {
                PlayerPrefs.SetInt("HighScore", pontuacaoAtual);
            }

            textoDeHighScore.text = "HIGHSCORE: " + PlayerPrefs.GetInt("HighScore");
        }
        else
        {
            textoDePontuacaoFinal.text = "PONTUAÇÃO: " + pontuacaoAtual;
            textoDeHighScore.text = "INIMIGOS DERROTADOS: " + inimigosDerrotados;
        }
        
        restartGameButton.gameObject.SetActive(true);
        nextPhaseButton.gameObject.SetActive(false);
    }
    
    public int CurrentPhase()
    {
        return phase;
    }

    public bool IsGameStarted()
    {
        return gameStarted;
    }
    
    public bool IsGameOver()
    {
        return gameOver;
    }
    
    public bool IsGamePaused()
    {
        return gamePaused;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ClearScene()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Inimigo"))
        {
            Destroy(obj);
        }

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Item"))
        {
            Destroy(obj);
        }

        foreach (GameObject obj in enemiesStatsByPhase)
        {
            obj.SetActive(false);
        }
        
        enemyStats.SetActive(false);
        Tutorial.Instance.tutorialPanel.SetActive(false);
    }
    
    public void ShowCredits()
    {
    }

    public void ShowTutorial()
    {
        Tutorial.Instance.ShowPanel();
    }
    
    public void PauseGame()
    {
        Time.timeScale = 0f;
        gamePaused = true;
        PauseGameButton.gameObject.SetActive(false);
        
        foreach (var obGameObject in objectsToShowOnPause)
        {
            obGameObject.gameObject.SetActive(true);
        }
    }
    
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        gamePaused = false;
        PauseGameButton.gameObject.SetActive(true);
        
        foreach (var obGameObject in objectsToShowOnPause)
        {
            obGameObject.gameObject.SetActive(false);
        }
    }

    public bool isLastPhase()
    {
        return phase >= PhaseMax;
    }
}