using UnityEngine;

public class LaserDoJogador : MonoBehaviour
{
    public GameObject impactoDoLaserDoJogador;

    public float velocidadeDoLaser;
    public int danoParaDar;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.IsGameStarted() && !GameManager.instance.IsGamePaused())
            MovimentarLaser();
    }

    private void MovimentarLaser()
    {
        transform.Translate(Vector3.up * (velocidadeDoLaser * Time.deltaTime));
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Inimigo"))
        {
            Inimigos inimigo = other.gameObject.GetComponent<Inimigos>();
            inimigo.MachucarInimigo(danoParaDar);

            Instantiate(impactoDoLaserDoJogador, transform.position, transform.rotation);
            EfeitosSonoros.instance.somDeImpacto.Play();

            Destroy(this.gameObject);
        }
    }
}