using UnityEngine;

public class PainelDeDialogo : MonoBehaviour
{
    public void NextDialogue()
    {
        if (GameManager.instance.HasNextDialog())
            GameManager.instance.NextDialog();
        else
            GameManager.instance.HistoryMode();
    }
}