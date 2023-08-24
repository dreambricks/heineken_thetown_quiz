using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


public class BarConfig : MonoBehaviour
{
    public Button saveButton;
    public Dropdown barName;

    private void Start()
    {
        saveButton.onClick.AddListener(SaveConfig);
        string barNameOption = LoadBarName();
        SetDropdownValueByName(barName, barNameOption);
    }

    private void SaveConfig()
    {
        string folderPath = Path.Combine(Application.persistentDataPath, "barData");

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        BarData data = new BarData();

        int selectedIndex = barName.value;
        data.name = barName.options[selectedIndex].text;
        Debug.Log(data.name);

        string json = JsonUtility.ToJson(data);

        string filePath = Path.Combine(folderPath, "barData.json");

        File.WriteAllText(filePath, json);

        Hide();
    }

    private void SetDropdownValueByName(Dropdown dropdown, string optionName)
    {
        int optionIndex = -1;

        // Loop through the options to find the index with the specified name
        for (int i = 0; i < dropdown.options.Count; i++)
        {
            if (dropdown.options[i].text == optionName)
            {
                optionIndex = i;
                break;
            }
        }

        // Set the value of the Dropdown based on the found index
        if (optionIndex != -1)
        {
            dropdown.value = optionIndex;
        }
        else
        {
            dropdown.value = 0;
        }
    }

    public static string LoadBarName()
    {
        string folderPath = Path.Combine(Application.persistentDataPath, "barData");

        string filePath = Path.Combine(folderPath, "barData.json");

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            BarData data = JsonUtility.FromJson<BarData>(json);

            return data.name;


        }
        else { return ""; }
    }




    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}

public class BarData
{
    public string name;
}
