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
    public GameObject diamante;
    public int estagioAtual = -1;
    public int pontuacao = 0;
    public Text txtPontuacao;

    public int raia = 0;
    public float distanciaRaia = 1.5f;
    
    public Vector3 posicao;

    public AudioSource somPonto;
    public AudioSource somColisao;
    public AudioSource somTema1;
    public AudioSource somTema2;

    private Vector2 posicaoInicial;
    private bool pulando = false;
    private bool pegouDiamante = false;

    // Start is called before the first frame update
    void Start()
    {
        somPonto = GetComponents<AudioSource>()[0];
        somColisao = GetComponents<AudioSource>()[1];
        somTema1 = GetComponents<AudioSource>()[2];
        somTema2 = GetComponents<AudioSource>()[3];

        txtPontuacao.text = "0 pts";

        posicao = jogador.transform.position;

        montarCenario();
    }

    // Update is called once per frame
    void Update()
    {
        bool pular = false;

        // teclado
        if (Input.GetKeyDown(KeyCode.RightArrow) && raia < 1)
        {
            raia += 1;
        } 
        
        if (Input.GetKeyDown(KeyCode.LeftArrow) && raia > -1)
        {
            raia -= 1;
        }

        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            pular = true;
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

            if(Input.mousePosition.y > posicaoInicial.y)
            {
                pular = true;
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

                if(Input.GetTouch(0).position.y > posicaoInicial.y)
                {
                    pular = true;
                }
            }
        }

        if (-1 <= raia && raia <= 1)
        {
            posicao = new Vector3(raia * distanciaRaia, jogador.transform.position.y, jogador.transform.position.z);
        }

        if(pular)
        {
            pulando = true;
        }

        if(pulando)
        {
            if(jogador.transform.position.y < 4f)
            {
                posicao.y = 4.5f;
                jogador.transform.position = Vector3.Lerp(jogador.transform.position, posicao, 3 * Time.deltaTime);
            }
            else
            {
                pulando = false;
            }
        }
        else if(!pulando && jogador.transform.position.y > 0.5f)
        {
            posicao.y = 0.5f;
            jogador.transform.position = Vector3.Lerp(jogador.transform.position, posicao, 3 * Time.deltaTime);
        }
        else if (jogador.transform.position.x != posicao.x)
        {
            jogador.transform.position = Vector3.Lerp(jogador.transform.position, posicao, 8 * Time.deltaTime);
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

        if (col.gameObject.CompareTag("diamante"))
        {
            if(!pegouDiamante)
            {
                StopAllAudio();
                somTema2.Play();
            }

            somPonto.Play();
            Destroy(col.gameObject);
            
            pontuacao += 3;
            txtPontuacao.text = pontuacao + " pts";
            pegouDiamante = true;
        }

        if (col.gameObject.CompareTag("obstaculo"))
        {
            somColisao.Play();
            
            new WaitForSeconds(5);

            SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
        }
    }

    private void StopAllAudio()
    {
        AudioSource[] audios = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];

        foreach( AudioSource audio in audios)
        {
            audio.Stop();
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
                if(estagioAtual == 6 && i == 1)
                {
                    // Cria diamante
                    instanciaElemento(0, i, newCampoZ, 4);
                }
                else if(estagioAtual == 8 && pegouDiamante)
                {
                    // Cria diamantes
                    instanciaElemento(-1, i, newCampoZ, 4);
                    instanciaElemento(0, i, newCampoZ, 4);
                    instanciaElemento(1, i, newCampoZ, 4);
                }
                else
                {
                    EstagioModel estagio = new EstagioModel();

                    instanciaElemento(-1, i, newCampoZ, estagio.Elemento1);
                    instanciaElemento(0, i, newCampoZ, estagio.Elemento2);
                    instanciaElemento(1, i, newCampoZ, estagio.Elemento3);
                }
            }

            estagioAtual++;

            if(pegouDiamante)
            {
                pegouDiamante = false;
            }
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

        if (elemento == 4)
        { 
            GameObject cristal = Instantiate(diamante);

            cristal.transform.SetParent(cenario.transform);
            cristal.transform.position = new Vector3(raia * distanciaRaia, 1f, posz);
        }
    }
}
