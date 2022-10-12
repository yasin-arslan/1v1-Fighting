using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{
    /// <summary>
    ///Oyunun durumunu kontrol eden script.
    ///</summary>
    [SerializeField] Player playerOne;
    [SerializeField] Player playerTwo;
    [SerializeField] TMP_Text playerOneScore;
    [SerializeField] TMP_Text playerTwoScore;
    private float gameTimer = 120;
    [SerializeField] TMP_Text timerText;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject openVolumeObject;
    [SerializeField] GameObject muteVolumeObject;
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] UnityEngine.UI.Button resumeButton;
    [SerializeField] UnityEngine.UI.Button playerOneShopButton;
    [SerializeField] UnityEngine.UI.Button playerTwoShopButton;
    static List<Player> players = new List<Player>();
    public static Player winner;
    public static Player loser;
    void Start()
    {
        players.Add(playerOne);
        players.Add(playerTwo);
        timerText.text = gameTimer.ToString();
    }
    void Update()
    {
        checkIfGameOver(gameTimer);
        if (gameTimer > 0)
        {
            gameTimer -= Time.deltaTime;
        }
        playerOneScore.text = playerOne.score.ToString();
        playerTwoScore.text = playerTwo.score.ToString();
        timerText.text = ((int)gameTimer).ToString();
        players.ForEach(p =>
        {
            if (p.health <= 0)
            {
                loser = p;
                int winnerIndex = players.IndexOf(p) == 1 ? 0 : 1;
                winner = players[winnerIndex];
                finishGame();
            }
            p.setHealthSlider(p.health);
            foreach (KeyValuePair<string, float> boost in p.activeBoosts)
            {
                //Debug.LogFormat("{0} isimli Player için {1} isimli boostun kalan süresi: {2}", player, boost.Key, boost.Value);
                if (Time.timeScale > 0f && boost.Value > 0f)
                {   
                    StartCoroutine(startDecreasingEffectTime(p, boost.Key, boost.Value));
                    p.boostImages[boost.Key].gameObject.SetActive(true);
                }
            }
        });
    }

    IEnumerator startDecreasingEffectTime(Player player, string key, float value)
    {
        yield return new WaitForSeconds(1f);
        value -= 1;
        if (value == 0f) { player.boostImages[key].gameObject.SetActive(false); }
        if (player.activeBoosts.ContainsKey(key))
        {
            player.activeBoosts[key] = value;
        }
    }
    public static Player findPlayerByName(string playerName)
    {
        return players.Find(player => player.name == playerName);
    }
    void checkIfGameOver(float timeLeft)
    {
        if (timeLeft <= 0)
        {
            Time.timeScale = 0f;
            finishGame();
        }
    }
    void finishGame()
    {
        SceneManager.LoadScene("EndGame");
    }

    public void pauseGame()
    {
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        resumeButton.interactable = false;
        playerOneShopButton.interactable = false;
        playerTwoShopButton.interactable = false;
        checkSoundLevel();
    }
    public void resumeGame()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        resumeButton.interactable = true;
        playerOneShopButton.interactable = true;
        playerTwoShopButton.interactable = true;
    }
    public void exitGame()
    {
        Application.Quit();
    }
    public void muteGame()
    {
        audioMixer.SetFloat("Volume", -80f);
    }
    public void unMuteGame()
    {
        audioMixer.SetFloat("Volume", 10f);
    }
    public void openSettingsMenu()
    {
        SceneManager.LoadScene("Settings");
    }
    public void checkSoundLevel()
    {
        float value;
        bool result = audioMixer.GetFloat("Volume", out value);
        if (result)
        {
            if (value > -80f)
            {
                muteVolumeObject.SetActive(true);
                openVolumeObject.SetActive(false);
            }
            else
            {
                openVolumeObject.SetActive(true);
                muteVolumeObject.SetActive(false);
            }
        }
    }
    public static IEnumerator fadeOutText(TMP_Text textToFadeOut, Color color)
    {
        float duration = 0.9f;
        float currentTime = 0f;
        while (currentTime < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, currentTime / duration);
            textToFadeOut.color = new Color(color.r, color.g, color.b, alpha);
            currentTime += Time.deltaTime;
            yield return null;
        }
        yield break;
    }
}
