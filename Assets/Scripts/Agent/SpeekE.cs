﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Crosstales.RTVoice;
using Crosstales.RTVoice.Model;


public class SpeekE : MonoBehaviour{

    public AudioSource som;            // Audio da Voz
    private Animator anim;             // Controle da animação
    public GameObject ballonImage;     // Imagem do balão
    public Text textBallon;            // Texto no balão
    private string animState;          // Guarda a animação atual
    Frase estadoAtual = new Frase();   // Objeto estado
    Arquivos ctrArq;                   // Objeto para acesso a L/E nos arquivos
    public int[] tabuleiro;
    int dica1, dica2;

    void Start(){
        anim = GetComponent<Animator>();   // Pegando a Animator para controle das animações
        ballonImage.SetActive(false);      // esconde a imagem do balão
        som = GetComponent<AudioSource>();

        ctrArq = new Arquivos("1");        // Criando novo arquivo com ID informado
        print(ctrArq.filePath);
    }

    public int[] usadas = new int[12];
    
    public void Speek(){
        // Quando essa função é chamada o TextToSpeech falará a frase contidade em estadoAtual.msg

        //Speaker.Speak(text.text, null, Speaker.VoiceForName("Microsoft Daniel"));
        Speaker.Speak(estadoAtual.msg, som, Speaker.VoiceForName("Microsoft Daniel"));
        ctrArq.registra(estadoAtual.msg, estadoAtual.codigo, estadoAtual.emocao);
    }

    void Update(){
        
        // Controle da animação
        animCtr();  // Ativando balão de animação
    }

    public void setPassivo(){
        anim.SetBool(animState, false);
        anim.SetBool("Passivo_1", true);
    }

    public void startAnimation(){
        anim.SetBool(animState, true);
        anim.SetBool("Passivo_1", false);
    }

    private void animCtr(){
        /*
            Responsavel por ativar o balão com a menssagem do Agente.
        */

        if(som.isPlaying == true){
            ballonImage.SetActive(true);
        }
        if(som.isPlaying == false){
            ballonImage.SetActive(false);
        }
    }

    private void setConf(){
        /*
            Essa função é responsavel por receber um objeto frase e setar a frase no balão, na voz do robo e definir a animação
        */
        
        Debug.Log(estadoAtual.msg);
        textBallon.text = estadoAtual.msg;
        animState = estadoAtual.emocao;
        
    }

    public void reacao(bool rec){
        /*
            Função receber um sentimento e escolhe aleatoriamente uma frase no banco de frases com o sentimento correspondente
        */

        if (rec == true){
            estadoAtual = ctrArq.pickUpEmocao("Alegre");
        }else{

            //int sentimento = (int)Random.Range(1, 3);
            int sentimento = 2;
            if(sentimento == 1){
                estadoAtual = ctrArq.pickUpEmocao("Triste");
            }else if(sentimento == 2){
                estadoAtual = ctrArq.pickUpEmocao("Bravo");
            }else if(sentimento == 3){
                estadoAtual = ctrArq.pickUpEmocao("Vergonha");
            }else{
                estadoAtual = ctrArq.pickUpEmocao("Triste");
            }
        }
        if(estadoAtual != null){
            //setConf();

            startAnimation();
        }
    }

    public int[] gerarDicaCorreta(){
        int dica1, dica2;
        int[] dicas = new int[2];

        for(int i=0;i<tabuleiro.Length;i++){
            dica1 = tabuleiro[i];
            /* Debug.Log($"tabuleiro I  {tabuleiro[i]}"); */

            if(dica1 != -1){
                for(int j=i+1;j<tabuleiro.Length;j++){
                    dica2 = tabuleiro[j];
                    /* Debug.Log($"taubuleiro J {tabuleiro[j]}"); */

                    if(dica1 == dica2){
                        dicas[0] = i;
                        dicas[1] = j;
                        this.dica1 = i+1;
                        this.dica2 = j+1;
                        //falarDica();
                        return dicas;
                    }
                }
            }
        }
        dicas[0] = 0;
        dicas[1] = 0;
        return dicas;
    }

    public int[] gerarDicaFalsa(){
        int[] dicas = new int[2];
        int escolha1 = (int)Random.Range(1, tabuleiro.Length);
        int escolha2 = escolha1;
        while(this.tabuleiro[escolha1] == this.tabuleiro[escolha2] || this.tabuleiro[escolha1] == -1 || this.tabuleiro[escolha2] == -1){
            escolha2 = (int)Random.Range(1, tabuleiro.Length);
            escolha1 = (int)Random.Range(1, tabuleiro.Length);
        }

        dica1 = escolha1;
        dica2 = escolha2;
        //falarDica();
        dicas[0] = dica1;
        dicas[1] = dica2;
        return dicas;
    }
    public void falarDica(string frase, bool personalizado){

        estadoAtual = ctrArq.getFrases("2");
        Debug.Log("arquivo speek: "+estadoAtual.msg);
        estadoAtual.msg = "Esperimente virar a carta";

        estadoAtual.msg = estadoAtual.msg+ " " +dica1.ToString()+ " e a "+ dica2.ToString()+ ".";
        Debug.Log(estadoAtual.msg);
        if (personalizado)
        {
            estadoAtual.msg = frase;
        }

        setConf();
        Speek();
        //startAnimation();
    }
}