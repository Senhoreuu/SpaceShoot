using UnityEngine;

public class GeradorDeObjetos : MonoBehaviour
{
    public GameObject[] objetosParaSpawnar;
    public Transform[] pontosDeSpawn;

    public float tempoMaximoEntreOsSpawns;
    public float tempoAtualDosSpawns;

    public GameObject enemyPhaseOne;
    public GameObject enemyPhaseTwo;
    public GameObject enemyPhaseThree;

    // Start is called before the first frame update
    void Start()
    {
        tempoAtualDosSpawns = tempoMaximoEntreOsSpawns;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.IsGameStarted() || GameManager.instance.IsGamePaused() || GameManager.instance.isBossFight) return;
        
        tempoAtualDosSpawns -= Time.deltaTime;

        if(tempoAtualDosSpawns <= 0)
        {
            SpawnarObjeto();
        }
    }

    private void SpawnarObjeto()
    {
        int pontoAleatorio = Random.Range(0, pontosDeSpawn.Length);
        GameObject objetoAleatorio = null;
        
        if (GameManager.instance.IsInfiniteMode())
        {
            objetoAleatorio = objetosParaSpawnar[Random.Range(0, objetosParaSpawnar.Length)];
        }
        else
        {
            objetoAleatorio = GameManager.instance.CurrentPhase() switch
            {
                0 => enemyPhaseOne,
                1 => enemyPhaseTwo,
                2 => enemyPhaseThree,
                _ => objetosParaSpawnar[Random.Range(0, objetosParaSpawnar.Length)]
            };
        }

        if (!objetoAleatorio) return;
        
        Instantiate(objetoAleatorio, pontosDeSpawn[pontoAleatorio].position,
            Quaternion.Euler(0f, 0f, -90f));
        tempoAtualDosSpawns = tempoMaximoEntreOsSpawns;
    }
}
