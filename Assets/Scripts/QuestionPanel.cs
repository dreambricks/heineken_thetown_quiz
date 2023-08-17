using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class QuestionPanel : MonoBehaviour
{

    public Question question;

    [SerializeField] private PlayGame playGame;
    [SerializeField] private YouWinWindow youWinWindow;
    [SerializeField] private YouLoseWindow youLoseWindow;

    [SerializeField] private Points points;

    public Button[] buttons;
    public Image[] nQuestionimages;
    public Text textUI;
    public Text acertouErrou;

    public List<Question> questions = new List<Question>();
    public List<Question> questionPool;
    private int currentQuestionIndex = 0;

    public AudioSource audioSource;
    public AudioClip correct;
    public AudioClip incorrect;
    public AudioClip music;

    public Slider progressBar;
    public float barTotalTime = 15f;
    private Coroutine progressCoroutine;

    private void Start()
    {
        for (int j = 0; j < buttons.Length; j++)
        {
            int buttonIndex = j;  // Variável para capturar o valor de i para uso no listener
            buttons[j].onClick.AddListener(() => StartCoroutine(CheckAnswer(buttonIndex)));
        }

    }

    void OnEnable()
    {
        audioSource.Stop();
        questionPool = new List<Question>();

        AddRandomItems(5);
        
    
        acertouErrou.gameObject.SetActive(false);

        BuildQuestion(currentQuestionIndex);
        audioSource.PlayOneShot(music);
        Debug.Log(music.name);

        points.Show();
        points.hits= 0;
        points.miss= 0;

    }


    private IEnumerator ProgressCoroutine()
    {
        float barCurrentTime = 0f;

        while (barCurrentTime <= barTotalTime)
        {
            float progress = barCurrentTime / barTotalTime;
            progressBar.value = progress;

            barCurrentTime += Time.deltaTime;
            yield return null;
        }

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].GetComponent<Image>().color = Color.red;
        }

        nQuestionimages[currentQuestionIndex].color = Color.red;

        // Encontrar o índice do botão com a resposta correta
        int correctButtonIndex = GetCorrectButtonIndex();
        buttons[correctButtonIndex].GetComponent<Image>().color = Color.green;

        audioSource.PlayOneShot(incorrect);

        acertouErrou.gameObject.SetActive(true);
        acertouErrou.text = "O tempo acabou!";
        acertouErrou.color = Color.red;

        points.miss++;

        DisableButtons();
        yield return new WaitForSeconds(3);
        acertouErrou.gameObject.SetActive(false);

        CheckToContinueQuestions();

    }


    private void BuildQuestion(int questionIndex)
    {
        
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

        music = ChooseRandomItem(question.audios);

        textUI.text = question.cantorMusica;

        // Atribui cada string da lista ao texto dos botões correspondentes
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].GetComponentInChildren<Text>().text = alternativeList[i];
        }

        progressCoroutine = StartCoroutine(ProgressCoroutine());
    }

    private AudioClip ChooseRandomItem(List<AudioClip> lista)
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
        HashSet<Question> temp = new HashSet<Question>();

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

        while (temp.Count != itemCount)
        {

            int randomIndex = Random.Range(0, questions.Count); // Escolhe um índice aleatório
            Question randomItem = questions[randomIndex]; // Pega o item aleatório

            temp.Add(randomItem); // Adiciona o item à lista de destino

        }

        questionPool.AddRange(temp);
    }

    IEnumerator CheckAnswer(int buttonIndex)
    {
        string buttonText = buttons[buttonIndex].GetComponentInChildren<Text>().text;

        if (buttonText == question.answer)
        {
            // Resposta correta
            buttons[buttonIndex].GetComponent<Image>().color = Color.green;
            nQuestionimages[currentQuestionIndex].color= Color.green;

            audioSource.PlayOneShot(correct);

            acertouErrou.gameObject.SetActive(true);
            acertouErrou.text = "Acertou!";
            acertouErrou.color = Color.green;

            points.hits++;
        }
        else
        {
            // Resposta incorreta
            buttons[buttonIndex].GetComponent<Image>().color = Color.red;
            nQuestionimages[currentQuestionIndex].color = Color.red;

            // Encontrar o índice do botão com a resposta correta
            int correctButtonIndex = GetCorrectButtonIndex();
            buttons[correctButtonIndex].GetComponent<Image>().color = Color.green;

            audioSource.PlayOneShot(incorrect);

            acertouErrou.gameObject.SetActive(true);
            acertouErrou.text = "Errou!";
            acertouErrou.color = Color.red;

            points.miss++;

        }

        // Desabilitar os botões após a resposta
        DisableButtons();
        StopCoroutine(progressCoroutine);
        yield return new WaitForSeconds(3);
        acertouErrou.gameObject.SetActive(false);

        CheckToContinueQuestions();

    }

    void CheckToContinueQuestions()
    {
        currentQuestionIndex++;
        if (currentQuestionIndex < questionPool.Count)
        {
            EnableButtons();
            StopCoroutine(progressCoroutine);
            BuildQuestion(currentQuestionIndex);
            audioSource.Stop();
            audioSource.PlayOneShot(music);
            Debug.Log(music.name);
        }
        else
        {
            if (points.hits >= 3)
            {
                SendLog();
                youWinWindow.Show();
            }
            else
            {
                SendLog();
                youLoseWindow.Show();
            }
            EnableButtons();
            currentQuestionIndex = 0;
            for (int i = 0; i < nQuestionimages.Length; i++)
            {
                nQuestionimages[i].color = Color.white;
            }

            playGame.Hide();
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

    private void SendLog()
    {
        string folderPath = Path.Combine(Application.persistentDataPath, "data_logs");

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        DataLog data = new DataLog();

        data.barName = BarConfig.LoadBarName();
        data.status = StatusEnum.CadastroConcluidoJogou.ToString();
        data.hits = points.hits.ToString();
        data.miss= points.miss.ToString();

        string formattedDateTime = System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ");

        data.timePlayed = formattedDateTime;

        string json = JsonUtility.ToJson(data);

        string fileName = string.Format("{0}_{1}_datalog.json", data.barName, data.timePlayed.Replace("-", "").Replace("T", "_").Replace(":", "").Replace("Z", ""));
        
        string filePath = Path.Combine(folderPath, fileName);
        Debug.Log(filePath);
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.Write(json);
        }

    }

}
