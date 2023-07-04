using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _pointText;
    [SerializeField] private Image _livesImg;
    [SerializeField] private List<Sprite> _livesSprite;

    [SerializeField] private TMP_Text _gameOverText;
    [SerializeField] private string _gameOverVerbage = "Game Over Dude";
    [SerializeField] private float _flickerSpeed = .5f;

    [SerializeField] private TMP_Text _restartText;
    [SerializeField] private string _restartVerbage = "Press 'R' To Restart.";

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
        _livesImg.sprite = _livesSprite[currentLives-1];
    }

    public void DisplayGameOverScreen()
    {
        _gameOverText.gameObject.SetActive(true);
        _isGameOver = true;
        _restartText.text = _restartVerbage;
        StartCoroutine(GameOverFlicker());
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
