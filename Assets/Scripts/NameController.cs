using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class NameController : MonoBehaviour
{
    private string playerOneName;
    private string playerTwoName;
    [SerializeField] TMP_InputField playerOneInput;
    [SerializeField] TMP_InputField playerTwoInput;
    [SerializeField] TMP_Text errorText;
    public void setNames()
    {
        if (String.IsNullOrEmpty(playerOneInput.text) || String.IsNullOrEmpty(playerTwoInput.text))
        {
            errorText.text = "bos isim girilemez!";
            StartCoroutine(GameController.fadeOutText(errorText, Color.red));
        }
        else
        {
            if (playerOneInput.text == playerTwoInput.text)
            {
                errorText.text = "iki isim aynı olamaz!";
                StartCoroutine(GameController.fadeOutText(errorText, Color.red));
            }
            else
            {
                if (playerOneInput.text.Length < 3 || playerTwoInput.text.Length < 3)
                {
                    errorText.text = "karakter ismi 3'den az olamaz!";
                    StartCoroutine(GameController.fadeOutText(errorText, Color.red));
                }
                else
                {
                    playerOneName = playerOneInput.text;
                    playerTwoName = playerTwoInput.text;
                    Player.nameDict[0] = playerOneInput.text;
                    Player.nameDict[1] = playerTwoInput.text;
                    SceneManager.LoadScene("Game");
                }

            }
        }
    }
}
