using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Player Reporting")]
    [SerializeField] private TMP_Text _pointText;
    [SerializeField] private Image _livesImg;
    [SerializeField] private List<Sprite> _livesSprite;

    [SerializeField] private Image _lightningBoost;

    [Header("Ammo Stuff")]
    [SerializeField] private TMP_Text _ammoCounter;

    [Header("Game Over Stuff")]
    [SerializeField] private TMP_Text _gameOverText;
    [SerializeField] private string _gameOverVerbage = "Game Over Dude";
    [SerializeField] private float _flickerSpeed = .5f;

    [SerializeField] private TMP_Text _restartText;
    [SerializeField] private string _restartVerbage = "Press 'R' To Restart.";

    [Header("Wave Information")]
    [SerializeField] private TMP_Text _waveText;
    [SerializeField] private TMP_Text _enemiesRemainingText;

    [SerializeField] private GameObject _warningText;
    [SerializeField] private float _warningFlashTime = 1.0f;
    private bool _warningFlash = false;

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

    public void ChangeEnemyRemainingText(string newValue)
    {
        _enemiesRemainingText.text = newValue;
    }

    public void ChangeCurrentWaveCounter(string newValue)
    {
        _waveText.text = newValue;
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

    public void LockedBoostMeter()
    {
        _lightningBoost.fillAmount = .99f;

        Debug.Log(_lightningBoost.fillAmount.ToString());


    }

    public void UnlockBoostMeter()
    {
        _lightningBoost.fillAmount = 0f;

    }

    public void StartTheWarningScreen()
    {
        _warningFlash = true;
        StartCoroutine(FlashTheWarningScreen());

    }
    public void StopTheWarningScreen()
    {
        _warningFlash = false;
    }
    IEnumerator FlashTheWarningScreen()
    {
        while (_warningFlash) 
        {
            _warningText.SetActive(true);
            yield return new WaitForSeconds(_warningFlashTime);
            _warningText.SetActive(false);
            yield return new WaitForSeconds(_warningFlashTime);

        }
    }
}
