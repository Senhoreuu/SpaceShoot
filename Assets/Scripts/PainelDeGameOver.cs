using UnityEngine;

public class PainelDeGameOver : MonoBehaviour
{
    public void ReiniciarJogo()
    {
        if (GameManager.instance.IsInfiniteMode())
            GameManager.instance.StartInfiniteMode(true);
        else
            GameManager.instance.StartHistoryMode(true);
    }

    public void NextPhase()
    {
        if (GameManager.instance.IsLastPhase())
            GameManager.instance.ShowCredits();
        else
            GameManager.instance.StartHistoryMode(true);
    }

    public void VoltarParaMenu()
    {
        GameManager.instance.MostrarMenu();
    }
}