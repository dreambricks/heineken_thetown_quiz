using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class DateMask : MonoBehaviour
{
    public InputField inputField;

    private void Start()
    {
        if (inputField != null)
        {
            inputField.onValueChanged.AddListener(OnInputValueChanged);
        
        }
    }

    private void OnInputValueChanged(string newValue)
    {
        string maskedValue = ApplyDateMask(newValue);
        MoveCursorToEnd();
        inputField.text = maskedValue;
    }

    private string ApplyDateMask(string input)
    {
        string cleanedInput = Regex.Replace(input, @"[^\d]", "");
        string formattedInput = Regex.Replace(cleanedInput, @"^(\d{2})(\d{2})(\d{4}).*", "$1/$2/$3");
        return formattedInput;
    }

    private void MoveCursorToEnd()
    {
        inputField.caretPosition = inputField.text.Length;
    }   
}
