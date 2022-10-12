using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ShopController : MonoBehaviour
{
    [SerializeField] float shopCooldown;
    private float _minScoreToOpen = 0f;
    [SerializeField] GameObject shopCanvas;
    [SerializeField] TMP_Text scoreTooLowText;
    [SerializeField] TMP_Text shopOwnerNameText;
    [SerializeField] TMP_Text damagePriceText;
    [SerializeField] TMP_Text healthPriceText;
    [SerializeField] TMP_Text movementSpeedText;
    [SerializeField] UnityEngine.UI.Button resumeButton;
    private float _damageBoost = 1.7f;
    private float _damageBoostPrice = 0f;
    private float _damageBoostDuration = 10f;
    private float _healthBoost = 25f;
    private float _healthBoostPrice = 0f;
    private float _healthBoostDuration = 3f;
    private float _movementBoost = 1.3f;
    private float _movementBoostPrice = 0f;
    private float _movementBoostDuration = 5f;
    private Player shopOwner;
    void Start()
    {
        damagePriceText.text = _damageBoostPrice.ToString();
        healthPriceText.text = _healthBoostPrice.ToString();
        movementSpeedText.text = _movementBoostPrice.ToString();
    }

    public void applyBoost(string boostName)
    {
        switch (boostName)
        {
            case "damage":
                if (shopOwner.score >= _damageBoostPrice)
                {
                    shopOwner.damage *= _damageBoost;
                    shopOwner.score -= (int)_damageBoostPrice;
                    shopOwner.activeBoosts["damage"] = _damageBoostDuration;
                }
                break;
            case "health":
                if (shopOwner.score >= _healthBoostPrice)
                {
                    float newHealth = shopOwner.health + _healthBoost > 100 ? 100 : shopOwner.health + _healthBoost;
                    shopOwner.health = newHealth;
                    shopOwner.score -= (int)_healthBoostPrice;
                    shopOwner.activeBoosts["health"] = _healthBoostDuration;
                }
                break;
            case "movementSpeed":
                if (shopOwner.score >= _movementBoostPrice)
                {
                    shopOwner.walkSpeed = _movementBoost;
                    shopOwner.score -= (int)_movementBoostPrice;
                    shopOwner.activeBoosts["movementSpeed"] = _movementBoostDuration;
                }
                break;
        }
    }
    public void checkScore(Player player)
    {
        if (player.score < _minScoreToOpen)
        {
            scoreTooLowText.gameObject.SetActive(true);
            StartCoroutine(GameController.fadeOutText(scoreTooLowText, Color.red));
        }
        else
        {
            shopOwner = player;
            Time.timeScale = 0f;
            shopCanvas.gameObject.SetActive(true);
            shopOwnerNameText.text = player.playerName;
            resumeButton.interactable = false;
        }
    }
    public void continueGame()
    {
        Time.timeScale = 1f;
        shopCanvas.gameObject.SetActive(false);
        resumeButton.interactable = true;
    }

}
