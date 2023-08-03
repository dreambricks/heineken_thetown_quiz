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
            Debug.LogError("O número de botões não corresponde ao número de strings na lista.");
            return;
        }

        textUI.text = question.cantorMusica;

        // Atribui cada string da lista ao texto dos botões correspondentes
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].GetComponentInChildren<Text>().text = alternativeList[i];
        }
    }

    private void ShuffleAndCopy(Question question)
    {
        // Cria uma lista temporária para embaralhar os itens
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

        // Copia os itens embaralhados para a lista aleatória
        alternativeList.AddRange(tempList);
    }

    private Question ChooseRandomItem(List<Question> lista)
    {
        // Verifica se a lista está vazia
        if (lista.Count == 0)
        {
            Debug.LogWarning("A lista está vazia. Não há itens para escolher.");
            return null;
        }

        // Gera um índice aleatório dentro dos limites da lista
        int indiceAleatorio = Random.Range(0, lista.Count);

        // Retorna o item aleatório
        return lista[indiceAleatorio];
    }
}
