using UnityEngine;

public class Inimigos : MonoBehaviour
{
    public GameObject laserDoInimigo;
    public Transform[] locaisDoDisparo;
    public GameObject itemParaDropar;
    public GameObject efeitoDeExplosao;

    public float velocidadeDoInimigo;
    public int vidaMaximaDoInimigo;
    public int vidaAtualDoInimigo;
    public int pontosParaDar;
    public int chanceParaDropar;
    public int danoDaNave;

    public float tempoMaximoEntreOsLasers;
    public float tempoAtualDosLasers;

    public bool inimigoAtirador;
    public bool inimigoAtivado;
    public bool stopMove;
    public bool followPlayer;
    public bool isBoss;
    public bool isFinalBoss;

    // Start is called before the first frame update
    void Start()
    {
        inimigoAtivado = false;

        if (GameManager.instance.IsInfiniteMode())
            vidaAtualDoInimigo = 1;
        else
            vidaAtualDoInimigo = vidaMaximaDoInimigo;
    }

    // Update is called once per frame
    void Update()
    {
        MovimentarInimigo();

        if (inimigoAtirador && inimigoAtivado)
        {
            AtirarLaser();
        }
    }

    public void AtivarInimigo()
    {
        inimigoAtivado = true;
    }

    private void MovimentarInimigo()
    {
        if (followPlayer)
        {
            if (GameManager.instance.player)
            {
                float playerY = GameManager.instance.player.transform.position.y;
                float newY = Mathf.MoveTowards(
                    transform.position.y,
                    playerY,
                    velocidadeDoInimigo * Time.deltaTime
                );

                transform.position = new Vector3(
                    transform.position.x,
                    newY,
                    transform.position.z
                );
            }
            return;
        }

        if (!stopMove)
            transform.Translate(Vector3.down * (velocidadeDoInimigo * Time.deltaTime));
    }

    private void AtirarLaser()
    {
        tempoAtualDosLasers -= Time.deltaTime;

        if (tempoAtualDosLasers <= 0)
        {
            foreach (var localDoDisparo in locaisDoDisparo)
            {
                Instantiate(laserDoInimigo, localDoDisparo.position, Quaternion.Euler(0f, 0f, 90f));
            }

            tempoAtualDosLasers = tempoMaximoEntreOsLasers;
        }
    }

    public void MachucarInimigo(int danoParaReceber)
    {
        if (!inimigoAtivado) return;

        vidaAtualDoInimigo -= danoParaReceber;

        if (vidaAtualDoInimigo <= 0)
        {
            GameManager.instance.AumentarPontuacao(pontosParaDar);
            if (!isBoss)
                GameManager.instance.AumentarInimigosDerrotados(1);
            Instantiate(efeitoDeExplosao, transform.position, transform.rotation);
            EfeitosSonoros.instance.somDaExplosao.Play();

            int numeroAleatorio = Random.Range(0, 100);

            if (numeroAleatorio <= chanceParaDropar)
            {
                Instantiate(itemParaDropar, transform.position, Quaternion.Euler(0f, 0f, 0f));
            }

            Destroy(this.gameObject);
            
            if (isBoss)
                GameManager.instance.CompletePhase();
        }
    }

    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if (collisionInfo.gameObject.CompareTag("Player"))
        {
            collisionInfo.gameObject.GetComponent<VidaDoJogador>().MachucarJogador(danoDaNave);
            Instantiate(efeitoDeExplosao, transform.position, transform.rotation);
            EfeitosSonoros.instance.somDaExplosao.Play();

            Destroy(this.gameObject);
        }
    }
}