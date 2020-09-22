using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class JogadorController : MonoBehaviour
{    
    public Rigidbody jogador;
    public float velocidadeCenario;
    public GameObject cenario;
    public GameObject campo;
    public GameObject obstaculo;
    public GameObject moeda;
    public int estagioAtual = -1;
    public int pontuacao = 0;
    public Text txtPontuacao;

    public int raia = 0;
    public float distanciaRaia = 1.5f;
    
    public Vector3 posicao;

    public AudioSource somPonto;
    public AudioSource somColisao;

    private Vector2 posicaoInicial;

    // Start is called before the first frame update
    void Start()
    {
        txtPontuacao.text = "0 pts";

        somPonto = GetComponents<AudioSource>()[0];
        somColisao = GetComponents<AudioSource>()[1];

        posicao = jogador.transform.position;

        montarCenario();
    }

    // Update is called once per frame
    void Update()
    {
        // teclado
        if (Input.GetKeyDown(KeyCode.RightArrow) && raia < 1)
        {
            raia += 1;
        } 
        
        if (Input.GetKeyDown(KeyCode.LeftArrow) && raia > -1)
        {
            raia -= 1;
        }
        
        // mouse
        if(Input.GetMouseButtonDown(0))
        {
            posicaoInicial = Input.mousePosition;
        }

        if(Input.GetMouseButtonUp(0))
        {
            if (Input.mousePosition.x > posicaoInicial.x && raia < 1)
            {
                raia += 1;
            } 
            
            if (Input.mousePosition.x < posicaoInicial.x && raia > -1)
            {
                raia -= 1;
            }
        }

        // touch
        if(Input.touchCount >= 1)
        {
            if(Input.GetTouch(0).phase == TouchPhase.Began)
            {
                posicaoInicial = Input.GetTouch(0).position;
            } 
            
            if(Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled)
            {
                if (Input.GetTouch(0).position.x > posicaoInicial.x && raia < 1)
                {
                    raia += 1;
                } 
                
                if (Input.GetTouch(0).position.x < posicaoInicial.x && raia > -1)
                {
                    raia -= 1;
                }
            }
        }

        if (-1 <= raia && raia <= 1)
        {
            posicao = new Vector3(raia * distanciaRaia, jogador.transform.position.y, jogador.transform.position.z);
        }

        if (jogador.transform.position.x != posicao.x)
        {
            jogador.transform.position = Vector3.Lerp(jogador.transform.position, posicao, 6 * Time.deltaTime);
        }

        var velocidade = (velocidadeCenario * ((estagioAtual * 0.2f) + 1));

        // roda a bola
        jogador.transform.Rotate(velocidade * 1.2f, 0.0f, 0.0f, Space.Self);

        // move o cenario
        cenario.transform.Translate(0, 0, Time.deltaTime * velocidade * -1);

        montarCenario();
    }

    void OnCollisionEnter(Collision col) 
    {
        if (col.gameObject.CompareTag("moeda"))
        {
            somPonto.Play();

            Destroy(col.gameObject);
            
            pontuacao++;
            txtPontuacao.text = pontuacao + " pts";
        }

        if (col.gameObject.CompareTag("obstaculo"))
        {
            somColisao.Play();

            SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
        }
    }

    private void montarCenario() 
    {
        float barreira = Mathf.Floor((cenario.transform.position.z / 10) * -2);

        if ((estagioAtual * 100) < (cenario.transform.position.z * -1))
        {
            GameObject newCampo = Instantiate(campo);

            float newCampoZ = (cenario.transform.position.z + ((estagioAtual + 1) * 100));

            newCampo.transform.SetParent(cenario.transform);
            newCampo.transform.position = new Vector3(cenario.transform.position.x, cenario.transform.position.y, newCampoZ);
            
            for (int i = (estagioAtual < 1 ? 3 : 1); i <= 10; i++) 
            {
                EstagioModel estagio = new EstagioModel();

                instanciaElemento(-1, i, newCampoZ, estagio.Elemento1);
                instanciaElemento(0, i, newCampoZ, estagio.Elemento2);
                instanciaElemento(1, i, newCampoZ, estagio.Elemento3);
            }

            estagioAtual++;
        }
    }

    private void instanciaElemento(int raia, int barreira, float newCampoZ, int elemento)
    {
        float posz = (newCampoZ + (10 * barreira));

        if (elemento == 0)
        {
            GameObject ponto = Instantiate(moeda);
    
            ponto.transform.SetParent(cenario.transform);
            ponto.transform.position = new Vector3(raia * distanciaRaia, 0.6f, posz);
        }
            
        if (elemento == 2)
        { 
            GameObject bloco = Instantiate(obstaculo);

            bloco.transform.SetParent(cenario.transform);
            bloco.transform.position = new Vector3(raia * distanciaRaia, 0.6f, posz);
        }
    }
}
