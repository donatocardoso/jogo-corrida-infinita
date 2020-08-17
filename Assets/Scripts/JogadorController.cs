using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JogadorController : MonoBehaviour
{
    public GameObject chao;
    public float velocidadeChao;
    public Rigidbody jogador;

    public int raia = 0;
    public float distanciaRaia = 1.5f;
    
    public Vector3 posicao;
    private Vector2 posicaoInicial;

    // Start is called before the first frame update
    void Start()
    {
        posicao = jogador.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // teclado
        if (Input.GetKeyDown(KeyCode.RightArrow) && raia < 1) {
            raia += 1;
        } 
        
        if (Input.GetKeyDown(KeyCode.LeftArrow) && raia > -1) {
            raia -= 1;
        }
        
        // mouse
        if(Input.GetMouseButtonDown(0)) {
            posicaoInicial = Input.mousePosition;
        }

        if(Input.GetMouseButtonUp(0)) {
            if (Input.mousePosition.x > posicaoInicial.x && raia < 1) {
                raia += 1;
            } 
            
            if (Input.mousePosition.x < posicaoInicial.x && raia > -1) {
                raia -= 1;
            }
        }

        // touch
        if(Input.touchCount >= 1) {
            if(Input.GetTouch(0).phase == TouchPhase.Began) {
                posicaoInicial = Input.GetTouch(0).position;
            } 
            
            if(Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled) {
                if (Input.GetTouch(0).position.x > posicaoInicial.x && raia < 1) {
                    raia += 1;
                } 
                
                if (Input.GetTouch(0).position.x < posicaoInicial.x && raia > -1) {
                    raia -= 1;
                }
            }
        }

        if (-1 <= raia && raia <= 1) {
            posicao = new Vector3(raia * distanciaRaia, jogador.transform.position.y, jogador.transform.position.z);
        }

        if (jogador.transform.position.x != posicao.x) {
            jogador.transform.position = Vector3.Lerp(jogador.transform.position, posicao, 5 * Time.deltaTime);
        }

        // roda a bola
        jogador.transform.Rotate(velocidadeChao, 0.0f, 0.0f, Space.Self);

        // move o chao
        chao.transform.Translate(0, 0, Time.deltaTime * velocidadeChao * -1);
    }
}
