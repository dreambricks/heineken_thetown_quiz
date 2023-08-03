using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionPanel : MonoBehaviour
{

    public Question question;
    public Button[] buttons;
    public Text textUI;
    private List<string> alternativeList = new List<string>();
    public List<Question> questions= new List<Question>();

    void Start()
    {
        
        question = ChooseRandomItem(questions);
        ShuffleAndCopy(question);


        if (buttons.Length != alternativeList.Count)
        {
            Debug.LogError("O n�mero de bot�es n�o corresponde ao n�mero de strings na lista.");
            return;
        }

        textUI.text = question.cantorMusica;

        // Atribui cada string da lista ao texto dos bot�es correspondentes
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].GetComponentInChildren<Text>().text = alternativeList[i];
        }
    }

    private void ShuffleAndCopy(Question question)
    {
        // Cria uma lista tempor�ria para embaralhar os itens
        List<string> tempList = new List<string>(question.alternatives);

        // Embaralha os itens aleatoriamente
        int n = tempList.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            string temp = tempList[k];
            tempList[k] = tempList[n];
            tempList[n] = temp;
        }

        // Copia os itens embaralhados para a lista aleat�ria
        alternativeList.AddRange(tempList);
    }

    private Question ChooseRandomItem(List<Question> lista)
    {
        // Verifica se a lista est� vazia
        if (lista.Count == 0)
        {
            Debug.LogWarning("A lista est� vazia. N�o h� itens para escolher.");
            return null;
        }

        // Gera um �ndice aleat�rio dentro dos limites da lista
        int indiceAleatorio = Random.Range(0, lista.Count);

        // Retorna o item aleat�rio
        return lista[indiceAleatorio];
    }
}
