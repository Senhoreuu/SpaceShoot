using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItensColetaveis : MonoBehaviour
{
    public bool itemDeEscudo;
    public bool itemDeLaserDuplo;
    public bool itemDeVida;

    public int vidaParaDar;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(GameManager.instance.IsGameStarted() && other.gameObject.CompareTag("Player") && !GameManager.instance.IsGamePaused())
        {
            if(itemDeEscudo)
            {
                other.gameObject.GetComponent<VidaDoJogador>().AtivarEscudo();
            }

            if(itemDeLaserDuplo)
            {
                other.gameObject.GetComponent<ControleDoJogador>().temLaserDuplo = false;

                other.gameObject.GetComponent<ControleDoJogador>().tempoAtualDosLasersDuplos = other.gameObject.GetComponent<ControleDoJogador>().tempoMaximoDosLasersDuplos;

                other.gameObject.GetComponent<ControleDoJogador>().temLaserDuplo = true;
            }

            if(itemDeVida)
            {
                other.gameObject.GetComponent<VidaDoJogador>().GanharVida(vidaParaDar);
            }

            Destroy(this.gameObject);
        }
    }

}
