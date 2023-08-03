using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionPanel : MonoBehaviour
{

    public Question question;

    public Button[] buttons;
    public Text textUI;

    public List<Question> questions= new List<Question>();
    public List<Question> questionPool = new List<Question>();
    private int currentQuestionIndex = 0;

    public AudioSource audioSource;
    public AudioClip correct;
    public AudioClip incorrect;

    void OnEnable()
    {
        AddRandomItems(5);
        //question = ChooseRandomItem(questions);

        for (int j = 0; j < buttons.Length; j++)
        {
            int buttonIndex = j;  // Variável para capturar o valor de i para uso no listener
            buttons[j].onClick.AddListener(() => StartCoroutine(CheckAnswer(buttonIndex)));
        }

        BuildQuestion(currentQuestionIndex);

    }


    private void BuildQuestion(int questionIndex)
    {
        Debug.Log(questionIndex);
        question = questionPool[questionIndex];

        List<string> alternativeList = new List<string>();
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

    private void AddRandomItems(int itemCount)
    {
        if (questions.Count == 0)
        {
            Debug.LogWarning("A lista de origem está vazia.");
            return;
        }

        if (itemCount > questions.Count)
        {
            Debug.LogWarning("Não há itens suficientes na lista de origem.");
            itemCount = questions.Count;
        }

        for (int i = 0; i < itemCount; i++)
        {
            int randomIndex = Random.Range(0, questions.Count); // Escolhe um índice aleatório
            Question randomItem = questions[randomIndex]; // Pega o item aleatório

            questionPool.Add(randomItem); // Adiciona o item à lista de destino
            
        }
    }

    IEnumerator CheckAnswer(int buttonIndex)
    {
        string buttonText = buttons[buttonIndex].GetComponentInChildren<Text>().text;

        if (buttonText == question.answer)
        {
            // Resposta correta
            buttons[buttonIndex].GetComponent<Image>().color = Color.green;

            audioSource.PlayOneShot(correct);
        }
        else
        {
            // Resposta incorreta
            buttons[buttonIndex].GetComponent<Image>().color = Color.red;
            // Encontrar o índice do botão com a resposta correta
            int correctButtonIndex = GetCorrectButtonIndex();
            buttons[correctButtonIndex].GetComponent<Image>().color = Color.green;

            audioSource.PlayOneShot(incorrect);
        }

        // Desabilitar os botões após a resposta
        DisableButtons();
        yield return new WaitForSeconds(3);


        currentQuestionIndex++;
        if (currentQuestionIndex < questionPool.Count)
        {
            EnableButtons();
            BuildQuestion(currentQuestionIndex);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    int GetCorrectButtonIndex()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i].GetComponentInChildren<Text>().text == question.answer)
            {
                return i;
            }
        }
        return -1;  // Retorna -1 se não encontrar a resposta correta (tratar isso conforme sua necessidade)
    }
    
    void DisableButtons()
    {
        // Desabilitar todos os botões após uma resposta
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }
    }

    void EnableButtons()
    {
        // Desabilitar todos os botões após uma resposta
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = true;
            Color buttonColor = buttons[i].GetComponent<Image>().color;
            buttonColor.a = 0f;
            buttons[i].GetComponent<Image>().color = buttonColor;
        }
    }

}
