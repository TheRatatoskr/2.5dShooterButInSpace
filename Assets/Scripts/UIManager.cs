using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Life Stuff")]
    [SerializeField] private TMP_Text _pointText;
    [SerializeField] private Image _livesImg;
    [SerializeField] private List<Sprite> _livesSprite;

    [Header("Ammo Stuff")]
    [SerializeField] private TMP_Text _ammoCounter;

    [Header("Game Over Stuff")]
    [SerializeField] private TMP_Text _gameOverText;
    [SerializeField] private string _gameOverVerbage = "Game Over Dude";
    [SerializeField] private float _flickerSpeed = .5f;

    [SerializeField] private TMP_Text _restartText;
    [SerializeField] private string _restartVerbage = "Press 'R' To Restart.";

    [SerializeField] private Image _lightningBoost;


    private bool _isGameOver = false;


    private void Start()
    {
        _gameOverText.gameObject.SetActive(false);
    }

    public void ChangePointText(int newText)
    {
        _pointText.text = newText.ToString();
    }

    public void ChangeLivesSprite(int currentLives)
    {
        _livesImg.sprite = _livesSprite[currentLives];
    }

    public void DisplayGameOverScreen()
    {
        _gameOverText.gameObject.SetActive(true);
        _isGameOver = true;
        _restartText.text = _restartVerbage;
        StartCoroutine(GameOverFlicker());
    }
    public void ChangeAmmoCounter(string newValue)
    {
        _ammoCounter.text = newValue;
    }

    public void ChangeBoostMeter(float fillPercent)
    {
       _lightningBoost.fillAmount = fillPercent;
    }

    IEnumerator GameOverFlicker()
    {
        while (_isGameOver)
        {
            _gameOverText.text = _gameOverVerbage;
            yield return new WaitForSeconds(_flickerSpeed);
            _gameOverText.text = "";
            yield return new WaitForSeconds(_flickerSpeed);
        }
    }
}
