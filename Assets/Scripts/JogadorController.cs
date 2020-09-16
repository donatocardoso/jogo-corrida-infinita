using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class JogadorController : MonoBehaviour
{    
    public Rigidbody jogador;
    public float velocidadeChao;
    public GameObject chao;
    public GameObject obstaculo;
    public GameObject moeda;
    public int estagioAtual = 0;
    public int pontuacao = 0;
    public Text txtPontuacao;

    public int raia = 0;
    public float distanciaRaia = 1.5f;
    
    public Vector3 posicao;
    private Vector2 posicaoInicial;

    // Start is called before the first frame update
    void Start()
    {
        txtPontuacao.text = "0 pts";
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
            jogador.transform.position = Vector3.Lerp(jogador.transform.position, posicao, 5 * 1.2f * Time.deltaTime);
        }

        // roda a bola
        jogador.transform.Rotate(velocidadeChao * 1.2f, 0.0f, 0.0f, Space.Self);

        // move o chao
        chao.transform.Translate(0, 0, Time.deltaTime * velocidadeChao * -1);

        float barreira = Mathf.Floor((chao.transform.position.z / 10) * -2);
        
        Debug.Log("estagio..: " + ((estagioAtual * 10) + 3));
        Debug.Log("barreira.: " + barreira);

        if (((estagioAtual * 10) + 4) < barreira)
        {
            GameObject chao2 = Instantiate(chao);
            float chao2z = (100 * (estagioAtual + 1)) + chao.transform.position.z;

            chao2.transform.SetParent(chao.transform);
            chao2.transform.position = new Vector3(chao.transform.position.x, chao.transform.position.y, chao2z);
            
            estagioAtual++;
            montarCenario();
        }
    }

    void OnCollisionEnter(Collision col) 
    {
        if (col.gameObject.CompareTag("moeda"))
        {
            Destroy(col.gameObject);
            
            pontuacao++;
            txtPontuacao.text = pontuacao + " pts";
        }

        if (col.gameObject.CompareTag("obstaculo"))
        {
            SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
        }
    }

    private void montarCenario() 
    {
        GameObject newChao = Instantiate(chao);
        float newChaoZ = (chao.transform.position.z + (estagioAtual * 110));

        newChao.transform.SetParent(chao.transform);
        newChao.transform.position = new Vector3(chao.transform.position.x, chao.transform.position.y, newChaoZ);
        
        estagioAtual++;

        for (int i = 2; i <= 10; i++) 
        {
            EstagioModel estagio = new EstagioModel();

            instanciaElemento(-1, i, newChao, estagio.Elemento1);
            instanciaElemento(0, i, newChao, estagio.Elemento2);
            instanciaElemento(1, i, newChao, estagio.Elemento3);
        }
    }

    private void instanciaElemento(int raia, int barreira, GameObject newChao, int elemento)
    {
        if (elemento == 0)
        { 
            return;
            GameObject moeda2 = Instantiate(moeda);
    
            float posz = ((newChao.transform.position.z / 10) * barreira) + 10;
        
            moeda2.transform.SetParent(chao.transform);
            moeda2.transform.position = new Vector3(raia * distanciaRaia, 0.6f, posz);
        }
            
        if (elemento == 2)
        { 
            GameObject bloco = Instantiate(obstaculo);
    
            float posz = (newChao.transform.position.z + (10 * barreira));
        
            bloco.transform.SetParent(chao.transform);
            bloco.transform.position = new Vector3(raia * distanciaRaia, 0.6f, posz);
        }
    }
}
