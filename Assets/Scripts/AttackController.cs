using System.Collections;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    [SerializeField] GameObject _playerOne;
    [SerializeField] GameObject _playerTwo;
    private Player[] players = new Player[2];

    void Start()
    {
        players[0] = _playerOne.GetComponent<Player>();
        players[1] = _playerTwo.GetComponent<Player>();
    }
    // Update is called once per frame
    void Update()
    {
        foreach (var player in players)
        {
            handlePunchForPlayer(player);
            handleKickForPlayer(player);
        }
    }
    /// <summary>
    ///Player scriptine sahip objeler için yumruk atma durumunu kontrol ediyor.
    ///</summary>
    /// <param name="player">Yumruk atan oyuncu.</param>
    void handlePunchForPlayer(Player player)
    {
        if (Input.GetButton(player.punchButtonName))
        {
            player.punchKeyPressed = true;
            if (player.isGrounded() && player.punchCooldown == 0f && player.punchKeyPressed)
            {
                player.animator.SetBool("canPunch", true);
                player.animator.SetBool("isPunching", true);
                player.animator.SetBool("punchKeyPressed", true);
                player.canPunch = false;
                player.isPlayerAtt = true;
                player.isPunching = true;
                player.punchCooldown = 5f;
                StartCoroutine("punchCooldownTimer", player);
            }
        }
    }
    /// <summary>
    ///Yumruk spamını engellemek için bekleme süresine sokan fonksiyon.
    ///</summary>
    /// <param name="player">Yumruk attıktan sonra bekleme süresine girecek oyuncu.</param>
    IEnumerator punchCooldownTimer(Player player)
    {
        yield return new WaitForSeconds(.750f);
        player.animator.SetBool("canPunch", true);
        player.animator.SetBool("isPunching", false);
        player.animator.SetBool("punchKeyPressed", false);
        player.canPunch = true;
        player.isPunching = false;
        player.isPlayerAtt = false;
        player.punchCooldown = 0f;
        player.punchKeyPressed = false;

    }
    /// <summary>
    ///Player scriptine sahip objeler için tekme atma durumunu kontrol ediyor.
    ///</summary>
    /// <param name="player">Tekme atan oyuncu.</param>
    void handleKickForPlayer(Player player)
    {
        if (Input.GetButton(player.kickButtonName))
        {
            player.kickKeyPressed = true;
            if (player.isGrounded() && player.kickCooldown == 0f && player.kickKeyPressed)
            {
                player.animator.SetBool("canKick", true);
                player.animator.SetBool("isKicking", true);
                player.animator.SetBool("kickKeyPressed", true);
                player.canKick = false;
                player.isPlayerAtt = true;
                player.isKicking = true;
                player.kickCooldown = 5f;
                StartCoroutine("kickCooldownTimer", player);
            }
        }
    }
    /// <summary>
    ///Tekme spamını engellemek için bekleme süresine sokan fonksiyon.
    ///</summary>
    /// <param name="player">Tekme attıktan sonra bekleme süresine girecek oyuncu.</param>
    IEnumerator kickCooldownTimer(Player player)
    {
        yield return new WaitForSeconds(.375f);
        player.animator.SetBool("canKick", true);
        player.animator.SetBool("isKicking", false);
        player.animator.SetBool("kickKeyPressed", false);
        player.canKick = true;
        player.isKicking = false;
        player.kickKeyPressed = false;
        player.isPlayerAtt = false;
        yield return new WaitForSeconds(player.kickCooldown - .5f);
        player.kickCooldown = 0f;
    }
}
