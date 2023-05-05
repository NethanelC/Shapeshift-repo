using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelSelection : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _levelsText, _pointsAndTimerText;
    [SerializeField] private GameObject _backButton, _forwardButton;
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private int _maxLevel;
    private int _selectedLevel = 1;
    private float _width;
    private void Start()
    {
        var selectedLevel = "Level" + _selectedLevel;
        if (PlayerPrefs.HasKey(selectedLevel))
        {
            _pointsAndTimerText.text = $"{PlayerPrefs.GetInt(selectedLevel)}/{PlayerPrefs.GetInt(selectedLevel + "max")}";
        }
        if (PlayerPrefs.HasKey(selectedLevel + "Timer"))
        {
            _pointsAndTimerText.text += $" - {PlayerPrefs.GetFloat(selectedLevel + "Timer"):F2}s";
        }
        if (_selectedLevel == 1)
        {
            _backButton.SetActive(false);
        }
        else
        {
            _backButton.SetActive(true);
        }
        _width = _rectTransform.rect.width;
    }
    public void IncreaseLevel()
    {
        _selectedLevel++;
        _rectTransform.position -= new Vector3(_width / (_maxLevel + 2), 0, 0);
        ChangeLevel();
    }
    public void LowerLevel()
    {
        _selectedLevel--;
        _rectTransform.position += new Vector3(_width / (_maxLevel + 2), 0, 0);
        ChangeLevel();
    }
    private void ChangeLevel()
    { 
        if (_selectedLevel == 1)
        {
            _backButton.SetActive(false);
        }
        else
        {
            _backButton.SetActive(true);
        }
        if (_selectedLevel == _maxLevel)
        {
            _forwardButton.SetActive(false);
        }
        else
        {
            _forwardButton.SetActive(true);
        }
        _levelsText.text = "Level " + _selectedLevel;
        var selectedLevel = "Level" + _selectedLevel;
        if (PlayerPrefs.HasKey(selectedLevel))
        {
            _pointsAndTimerText.text = $"{PlayerPrefs.GetInt(selectedLevel)}/{PlayerPrefs.GetInt(selectedLevel + "max")}";
        }
        else
        {
            _pointsAndTimerText.text = null;
        }
        if (PlayerPrefs.HasKey(selectedLevel + "Timer"))
        {
            _pointsAndTimerText.text += $" - {PlayerPrefs.GetFloat(selectedLevel + "Timer"):F2}s";
        }
    }
    public void LoadLevel()
    {
        SceneManager.LoadScene("Level" + _selectedLevel);
    }
}
