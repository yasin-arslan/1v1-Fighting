using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MovementController : MonoBehaviour
{
    /// <summary>
    ///Player scriptine sahip GameObjectler için hareket kontrol scripti.
    ///</summary>
    [SerializeField] TMP_Text timer;
    [SerializeField] Player playerOne;
    [SerializeField] Player playerTwo;
    [SerializeField] GameObject gameScene;
    static Player[] playerArray = new Player[2];

    
    void Start()
    {
        playerArray[0] = playerOne;
        playerArray[1] = playerTwo;
    }
    void Update()
    {
        playerOne.playerHorizontalMovement = Input.GetAxis(playerArray[0].movementButtonName);
        playerTwo.playerHorizontalMovement = Input.GetAxis(playerArray[1].movementButtonName);
        foreach (var player in playerArray)
        {
            handleJumpForPlayer(player);
            handleMovementForPlayer(player);
        }
    }
    IEnumerator stopJumpingAfterTime(Player player)
    {
        yield return new WaitForSeconds(1f);
        player.jump = false;
        player.canWalk = true;
        player.animator.SetBool("canWalk", true);
        player.animator.SetBool("canPunch", true);
        player.animator.SetBool("canPunch", true);
    }
    void walkingRestrictions(Player player, bool condition)
    {
        player.animator.SetBool("isWalking", condition);
        player.animator.SetBool("canPunch", !condition);
        player.animator.SetBool("canKick", !condition);
    }
    void handleJumpForPlayer(Player player)
    {
        if (Input.GetButtonDown(player.jumpButtonName))
        {
            if (player.isGrounded() && !player.isWalking)
            {
                player.canWalk = false;
                player.jump = true;
                player.animator.SetBool("canWalk", false);
                player.animator.SetBool("canPunch", false);
                player.animator.SetBool("canKick", false);
                player.playerBody.velocity = new Vector2(0f, 5f);
                StartCoroutine("stopJumpingAfterTime", player);
            }
        }
    }
    void handleMovementForPlayer(Player player)
    {
        if (player.playerHorizontalMovement == 0f)
        {
            walkingRestrictions(player, false);
            player.animator.SetFloat("movementMultiplier", 1f);
        }
        if (!player.jump && player.canWalk && player.playerHorizontalMovement != 0f)
        {
            walkingRestrictions(player, true);
            player.animator.SetFloat("movementMultiplier", player.playerHorizontalMovement + 1f);
            if (player.playerHorizontalMovement > 0f)
            {
                player.playerBody.velocity = new Vector2(player.walkSpeed, player.playerBody.velocity.y);
            }
            if (player.playerHorizontalMovement < 0f)
            {
                player.playerBody.velocity = new Vector2(-player.walkSpeed, player.playerBody.velocity.y);
            }
        }
    }

}
