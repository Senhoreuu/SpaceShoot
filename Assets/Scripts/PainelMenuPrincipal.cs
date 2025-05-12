using UnityEngine;
using UnityEngine.UI;

public class PainelMenuPrincipal : MonoBehaviour
{
    public Button buttonMuteGame;
    public Button buttonUnmuteGame;
    public void StartGame()
    {
        GameManager.instance.StartInfiniteMode(true);
    }
    
    public void QuitGame()
    {
        GameManager.instance.ExitGame();
    }

    public void ShowTutorial()
    {
        GameManager.instance.ShowTutorial();
    }

    public void StartHistoryMode()
    {
        GameManager.instance.StartHistoryMode(true);
    }

    public void MuteSounds()
    {
        GameManager.instance.musicaDeGameOver.volume = 0;
        GameManager.instance.musicaDoJogo.volume = 0;
        
        buttonMuteGame.gameObject.SetActive(false);
        buttonUnmuteGame.gameObject.SetActive(true);
    }
    
    public void UnmuteSounds()
    {
        GameManager.instance.musicaDeGameOver.volume = 40;
        GameManager.instance.musicaDoJogo.volume = 10;
        
        buttonMuteGame.gameObject.SetActive(true);
        buttonUnmuteGame.gameObject.SetActive(false);
    }
}