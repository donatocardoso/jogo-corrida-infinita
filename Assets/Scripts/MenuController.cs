using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void abrirJogo() {
        SceneManager.LoadScene("Jogo", LoadSceneMode.Single);
    }

    void abrirMenu() {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }

    void abrirGameOver() {
        SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
    }
}
