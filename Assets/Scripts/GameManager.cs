using UnityEngine;
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

    public int[] dialogsByScenes;

    public GameObject player;

    private int phase;
    private int dialogBySceneIndex;

    private int totalDialogs;

    public GameObject ryder;
    public GameObject commandCenter;
    public GameObject boss1;
    public GameObject boss2;

    public GameObject bossPhaseTwo;
    public GameObject bossPhaseThree;

    public Transform bossSpawn;

    public bool isBossFight;

    public GameObject creditsPanel;

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

        EsconderMenu();

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
        player = GameObject.FindGameObjectWithTag("Player");
        FindFirstObjectByType<VidaDoJogador>().ResetStats();
    }

    public void StartHistoryMode(bool restartScene)
    {
        if (restartScene)
            ClearScene();

        if (phase >= PhaseMax)
        {
            phase = 0;
            dialogBySceneIndex = 0;
        }

        if (dialogBySceneIndex >= dialogsByScenes.Length)
        {
            dialogBySceneIndex = 0;
        }

        if (dialogsByScenes[dialogBySceneIndex] > 0 && !IsGameOver())
        {
            dialogPanel.SetActive(true);
            totalDialogs = dialogsByScenes[dialogBySceneIndex];
            dialogBySceneIndex++;
            NextDialog();
        }

        if (totalDialogs <= 0)
            HistoryMode();

        pontuacaoAtual = 0;
        inimigosDerrotados = 0;
        inimigosDerrotadosMaximo = inimigosParaDerrotar[phase];
        textoDePontuacaoAtual.text = "PONTUAÇÃO: " + pontuacaoAtual;
        enemyStats.SetActive(true);
        enemyStatsText[phase].text = inimigosDerrotados + "/" + inimigosDerrotadosMaximo;
        enemiesStatsByPhase[phase].SetActive(true);
        musicaDoJogo.Play();
        painelDeFinalDeJogo.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player");
        FindFirstObjectByType<VidaDoJogador>().ResetStats();
        EsconderMenu();
    }

    public void HistoryMode()
    {
        Time.timeScale = 1f;

        _isInfiniteMode = false;
        gameStarted = true;
        gameOver = false;
        gamePaused = false;

        dialogPanel.SetActive(false);
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
            switch (phase)
            {
                case 1:
                case 2:
                    Time.timeScale = 0f;
                    gamePaused = true;
                    dialogPanel.SetActive(true);
                    totalDialogs = dialogsByScenes[dialogBySceneIndex];
                    dialogBySceneIndex++;
                    ClearScene();
                    
                    NextDialog();
                    break;
                default:
                    CompletePhase();
                    break;
            }
        }
    }

    private void Dialog(string text, bool showRyder, bool showCommandCenter, bool showBoss1, bool showBoss2)
    {
        ryder.SetActive(showRyder);
        commandCenter.SetActive(showCommandCenter);
        boss1.SetActive(showBoss1);
        boss2.SetActive(showBoss2);

        dialogPanel.SetActive(true);
        dialogPanel.GetComponentInChildren<Text>().text = text;
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
        creditsPanel.SetActive(false);
        gameStarted = false;
        gameOver = false;
        isBossFight = false;
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

    public void CompletePhase()
    {
        if (phase >= PhaseMax || IsInfiniteMode()) return;

        // mostrar o painel de fase completa
        Time.timeScale = 0f;
        musicaDoJogo.Stop();

        painelDeFinalDeJogo.SetActive(true);
        gamePaused = true;
        gameStarted = false;
        gameOver = false;
        isBossFight = false;

        textoDeFinalDeJogo.text = "Fase %p completada!".Replace("%p", (++phase).ToString());

        if (phase < PhaseMax)
        {
            textoDePontuacaoFinal.text = "PONTUAÇÃO: " + pontuacaoAtual;
            textoDeHighScore.text = "INIMIGOS DERROTADOS: " + inimigosDerrotados;

            textoFinalDeFase.text = "Avançar";
        }
        else
        {
            textoDePontuacaoFinal.text = "PONTUAÇÃO: " + pontuacaoAtual;
            textoDeHighScore.text = "FIM DE JOGO! Você chegou ao final do jogo!";

            textoFinalDeFase.text = "Créditos";
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
        ClearScene();
        Time.timeScale = 0f;
        musicaDoJogo.Stop();
        
        menuPanel.SetActive(false);
        creditsPanel.SetActive(true);
        gamePaused = true;
        gameStarted = false;
        gameOver = false;
        isBossFight = false;
    }

    public void ShowTutorial()
    {
        Tutorial.Instance.ShowPanel();
    }

    public void PauseGame()
    {
        if (gamePaused) return;
        
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
        if (!gamePaused) return;
        
        Time.timeScale = 1f;
        gamePaused = false;
        PauseGameButton.gameObject.SetActive(true);

        foreach (var obGameObject in objectsToShowOnPause)
        {
            obGameObject.gameObject.SetActive(false);
        }
    }

    public bool IsLastPhase()
    {
        return phase >= PhaseMax;
    }

    public bool HasNextDialog()
    {
        return totalDialogs > 0;
    }

    public void NextDialog()
    {
        if (totalDialogs <= 0) return;

        switch (dialogBySceneIndex)
        {
            case 1:
                if (totalDialogs == 2)
                    Dialog(
                        "Ryder, colônias estão sob ataque! Proteja os civis e derrube os batedores Kryll. A guerra começou.",
                        false, true, false, false);
                else
                    Dialog("Certo, Comandante. Vou fazer o que for preciso.", true, false, false, false);

                break;
            case 2:
                if (totalDialogs == 3)
                    Dialog(
                        "Interceptamos suprimentos Kryll no cinturão de Marte. Espere resistência pesada. Recupere qualquer dado que encontrar.",
                        false, true, false, false);
                else if (totalDialogs == 2)
                    Dialog("Entendido. Vou me preparar para o combate.", true, false, false, false);
                else
                    Dialog("Ryder, os alvos estão se aproximando. Prepare-se para o combate.", false, true, false,
                        false);
                break;
            case 3:
                if (totalDialogs == 4)
                    Dialog("Droga… isso não é uma nave comum. Preparar mísseis! Isso vai doer.", true, false, false,
                        false);
                else if (totalDialogs == 3)
                    Dialog("Ryder, é um Kryll! Uma nave de guerra. Tome cuidado e a destrua!",
                        false, true, false, false);
                else if (totalDialogs == 2)
                    Dialog("Terráqueos, preparem-se! Vamos acabar com vocês!", false, false, true,
                        false);
                else
                {
                    Dialog("Vemos ver o que você tem, Kryll. Prepare-se para o combate!", true, false,
                        false, false);
                    
                    Instantiate(bossPhaseTwo, bossSpawn.position,
                        Quaternion.Euler(0f, 0f, -90f));
                    
                    Time.timeScale = 1f;
                    gamePaused = false;
                    isBossFight = true;
                }
                break;
            case 4:
                if (totalDialogs == 2)
                    Dialog(
                        "Comandante, parece que essas naves não possuem armas. São apenas batedores. São muito rápidos.",
                        true, false, false, false);
                else
                    Dialog("Ryder, você está indo muito bem. Continue assim!", false, true, false, false);
                break;
            case 5:
                if (totalDialogs == 3)
                    Dialog("A nave-mãe Kryll chegou. É tudo ou nada. Que os céus estejam com você, Ryder.", false, true,
                        false, false);
                else if (totalDialogs == 2)
                    Dialog("Aí está ela… a fonte de tudo. Hora de acabar com isso.", true, false, false, false);
                else
                {
                    Dialog("Vida insignificante. Não escaparão do ciclo da colheita.", false, false, false, true);

                    Instantiate(bossPhaseThree, bossSpawn.position,
                        Quaternion.Euler(0f, 0f, -90f));
                    
                    Time.timeScale = 1f;
                    gamePaused = false;
                    isBossFight = true;
                }

                break;
        }

        totalDialogs--;
    }
}