using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using System.Collections.Generic;
public class Player : MonoBehaviour
{
    [Header("Walking Attributes")]
    public bool canWalk = true;
    public bool isWalking = true;
    public float playerHorizontalMovement;
    public float walkSpeed = .95f;

    [Header("Kicking attributes")]
    public float kickCooldown = 0f;
    public bool canKick = true;
    public bool isKicking = false;
    public bool kickKeyPressed = false;

    [Header("Punch attributes")]
    public float punchCooldown = 0f;
    public bool canPunch = true;
    public bool isPunching = false;
    public bool punchKeyPressed = false;

    [Header("Jump attributes")]
    public float jumpCooldown = 0f;
    public bool jump = false;

    [Header("Player attributes")]
    public int playerNo;
    public TMP_Text playerNameText;
    public string playerName;
    public float health;
    public int score = 0;
    public Rigidbody2D playerBody;
    public Animator animator;
    public Slider healthSlider;
    public string jumpButtonName;
    public string punchButtonName;
    public string kickButtonName;
    public string movementButtonName;
    public bool isPlayerAtt = false;
    public float damage = 5f;
    [Header("Player boosts")]
    public Dictionary<string, float> activeBoosts;
    public Image damageBoostIcon;
    public Image healthBoostIcon;
    public Image movementSpeedIcon;
    public Dictionary<string, Image> boostImages;
    public static Dictionary<int, string> nameDict = new Dictionary<int, string>(){
        {
            0,""
        },
        {
            1,""
        }
    };
    public int punchCount;
    public int kickCount;

    void Start()
    {
        activeBoosts = new Dictionary<string, float>()
        {
            {
                "damage",0f
            },
            {
                "health",0f
            },
            {
                "movementSpeed",0f
            }
        };
        boostImages = new Dictionary<string, Image>(){
        {
            "damage",damageBoostIcon
        },
        {
            "health",healthBoostIcon
        },
        {
            "movementSpeed",movementSpeedIcon
        }
        };
        playerName = playerNo == 0 ? nameDict[0] : nameDict[1];
        playerNameText.text = playerName;
    }
    public void takeDamage(float damage)
    {
        health = damage > health ? health = 0f : health -= damage;
    }
    public bool isPlayerAttacking()
    {
        isPlayerAtt = isKicking || isPunching;
        return isPlayerAtt;
    }
    public void setHealthSlider(float value)
    {
        healthSlider.value = value;
    }
    public void setAttackCooldowns(float cooldown)
    {
        punchCooldown = kickCooldown = cooldown;
    }
    public bool isGrounded()
    {
        var boxCollider = GetComponent<BoxCollider2D>();
        RaycastHit2D hit = Physics2D.Raycast(boxCollider.bounds.center, Vector2.down, boxCollider.bounds.extents.y + .05f, LayerMask.GetMask("Ground"));
        return hit.collider != null;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>() is not null && gameObject.GetComponent<Player>() is not null)
        {
            Player attacker = gameObject.GetComponent<Player>();
            Player target = other.GetComponent<Player>();
            if (attacker.isPlayerAtt)
            {
                if (attacker.isPunching) attacker.punchCount++;
                if (attacker.isKicking) attacker.kickCount++;
                attacker.score = attacker.score + 5;
                target.takeDamage(attacker.damage);
                target.animator.SetBool("isHurt", true);
                Player[] playerArray = new Player[2];
                playerArray[0] = target;
                playerArray[1] = attacker;
                StartCoroutine("stopHurtAnimationAfterTime", playerArray);
            }
        }
    }
    IEnumerator stopHurtAnimationAfterTime(Player[] playerArray)
    {
        //playerArray[1] daima saldıran playerArray[0] daima hedef!
        playerArray[1].setAttackCooldowns(.375f);
        yield return new WaitForSeconds(.375f);
        playerArray[0].animator.SetBool("isHurt", false);
        yield return new WaitForSeconds(5f);
        playerArray[1].setAttackCooldowns(0f);
    }
}
