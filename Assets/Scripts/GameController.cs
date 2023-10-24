using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    [SerializeField]
    private Sprite bgImage;
    
    public Sprite[] puzzles;

    public List<Sprite> gamePuzzles = new List<Sprite>();

    public List<Button> btns = new List<Button>();

    private bool firstGuess, secondGuess;
    private int countGuesses;
    private int countCorrectGuesses;
    private int gameGuesses;

    private int firstGuessIndex, secondGuessIndex;

    private string firstGuessPuzzle, secondGuessPuzzle;

    public bool vezJogador;

    public int[] jogadas = new int[2];
    public int pontosjogador = 0;
    public int pontosrobo = 0;

    //private int[] tabuleiro;

    [SerializeField]
    private SpeekE speekE;

    private void Awake()
    {
        puzzles = Resources.LoadAll<Sprite>("Sprites/Candy");
    }


    private void Start()
    {
        vezJogador = true;
        GetButtons();
        AddListeneers();
        AddGamePuzzles();
        Shuffle(gamePuzzles);
        gameGuesses = gamePuzzles.Count / 2;
    }

    void Update(){
        if(!vezJogador){

            PickAPuzzle();

            PickAPuzzle();
        }
    }



        void GetButtons()
    {
            GameObject[] objects = GameObject.FindGameObjectsWithTag("PuzzleButton");

            for (int i = 0; i < objects.Length; i++)
            {
                btns.Add(objects[i].GetComponent<Button>());
                btns[i].image.sprite = bgImage;
            }
    }

    void AddGamePuzzles()
    {
        /*
            Essa é a função responsavel por gerar o tabuleiros com os pares
        */
        int looper = btns.Count;
        int index = 0;

        speekE.tabuleiro = new int[looper];

        for (int i=0; i< looper; i++)
        {
            if (index == looper /2)
            {
                index = 0;
            }
            gamePuzzles.Add(puzzles[index]);
            speekE.tabuleiro[i] = index;

            index++;
        }
    }

    void AddListeneers()
    {
        foreach (Button btn in btns)
        {
            btn.onClick.AddListener(() => PickAPuzzle());
        }
    }

    public void PickAPuzzle()
    {
        if(!firstGuess && !secondGuess){
            int chanceAcertar = Mathf.FloorToInt(Random.Range(1, 4));
            if(chanceAcertar == 1 || countCorrectGuesses == gameGuesses-1){
                Debug.Log("ACERTAR");
                jogadas = speekE.gerarDicaCorreta();
            }else{
                Debug.Log("ERRAR");
                jogadas = speekE.gerarDicaFalsa();

                
            }
            
        }
        
        
        if (!firstGuess)
        {
            firstGuess = true;
            //mf: firstguessindex recebe o id da carta que vai ser selecionada
            if(vezJogador){
                firstGuessIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
            }else{
                firstGuessIndex = jogadas[0];
            }
            Debug.Log($"primeiro escolhido {firstGuessIndex}");
            

            firstGuessPuzzle = gamePuzzles[firstGuessIndex].name;

            btns[firstGuessIndex].image.sprite = gamePuzzles[firstGuessIndex];

        } else if (!secondGuess)
        {
            secondGuess = true;
            if(vezJogador){
                secondGuessIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
            }else{
                secondGuessIndex = jogadas[1];
            }
            
            Debug.Log($"segundo escolhido {secondGuessIndex}");
            secondGuessPuzzle = gamePuzzles[secondGuessIndex].name;

            btns[secondGuessIndex].image.sprite = gamePuzzles[secondGuessIndex];

            if(firstGuessPuzzle == secondGuessPuzzle){
                Debug.Log("Cartas corretas");
                /* speekE.reacao(true); */
                speekE.tabuleiro[firstGuessIndex] = -1;
                speekE.tabuleiro[secondGuessIndex] = -1;
                if(vezJogador){
                    pontosjogador += 1;
                }else{
                    pontosrobo += 1;
                }
            } else {
                Debug.Log("Tente outra vez");
                if(vezJogador){
                    vezJogador = false;
                }else{
                    vezJogador = true;
                }
                //speekE.reacao(false);
            }
            

            countGuesses++;

            StartCoroutine(CheckIfThePuzzlesMatch());

        }
    }
    IEnumerator CheckIfThePuzzlesMatch()
    {
        yield return new WaitForSeconds(1f);
        if (firstGuessPuzzle == secondGuessPuzzle)
        {
            yield return new WaitForSeconds(.5f);

            btns[firstGuessIndex].interactable = false;
            btns[secondGuessIndex].interactable = false;

            btns[firstGuessIndex].image.color = new Color(1, 1, 1, 1);
            btns[secondGuessIndex].image.color = new Color(1, 1, 1, 1);

            CheckIfTheGameIsFinished();

        } else
        {
            yield return new WaitForSeconds(.5f);

            btns[firstGuessIndex].image.sprite = bgImage;
            btns[secondGuessIndex].image.sprite = bgImage;
        }

        yield return new WaitForSeconds(.5f);

        firstGuess = secondGuess = false;

    }

    void CheckIfTheGameIsFinished()
    {
        countCorrectGuesses++;

        if (countCorrectGuesses == gameGuesses)
        {
            Debug.Log("Fim do Jogo");
            Debug.Log("Foram " + countGuesses + " tentativas para Terminar");
            if (pontosjogador > pontosrobo)
            {
                Debug.Log($"Jogador venceu com {pontosjogador} pontos contra {pontosrobo} pontos de Roboldo");
            }
            if (pontosjogador < pontosrobo)
            {
                Debug.Log($"Roboldo venceu com {pontosrobo} pontos contra {pontosjogador} pontos do Jogador");
            }
            if (pontosjogador == pontosrobo)
            {
                Debug.Log($"Jogo empatado ambos com {pontosjogador}");
            }

        }
    }

    void Shuffle(List<Sprite> list) // funcao randomica
    {
        for (int i=0; i < list.Count; i++)
       {
            Sprite temp = list[i];
            int inttemp = speekE.tabuleiro[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            speekE.tabuleiro[i] = speekE.tabuleiro[randomIndex];
            list[randomIndex] = temp;
            speekE.tabuleiro[randomIndex] = inttemp;
        }
    }

} // GameController
