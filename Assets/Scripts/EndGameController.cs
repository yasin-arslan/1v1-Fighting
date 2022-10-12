using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EndGameController : MonoBehaviour
{
    [SerializeField] TMP_Text winnerNameText;
    [SerializeField] TMP_Text loserNameText;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text punchText;
    [SerializeField] TMP_Text kickText;
    Player loser = GameController.loser;
    Player winner = GameController.winner;
    void Start()
    {
        winnerNameText.text = winner.playerName;
        loserNameText.text = loser.playerName;
        scoreText.text = winner.score.ToString();
        punchText.text = winner.punchCount.ToString();
        kickText.text = winner.kickCount.ToString();
    }
    public void exitGame(){
        Application.Quit();
    }
    public void restartGame(){
        SceneManager.LoadScene("MainMenu");
    }
}
