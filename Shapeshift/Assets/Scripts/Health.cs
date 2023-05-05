using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Slider _slider;
    [SerializeField] private Animator _anim;
    [SerializeField] private GameObject _hitEffect, _deathEffect;
    [SerializeField] private TextMeshPro _deathScorePrefab;
    [SerializeField] private List<GameObject> _itemDrops = new();
    [SerializeField] private float _maxHealth;
    [SerializeField] private int _deathScore;
    private float _currentHealth;
    private void Start()
    {
        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "max", PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "max") + _deathScore);
        _currentHealth = _maxHealth;
        if (_slider != null)
        {
            _slider.value = 1;
        }
    }
    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        _anim.SetTrigger("Hit");
        Instantiate(_hitEffect, gameObject.transform.position, Quaternion.identity);
        if (_slider != null)
        {
            FillSlider();
        }
        if (_currentHealth <= 0)
        {
            Die();
        }
    }
    private void FillSlider()
    {
        float finishValue = _maxHealth;
        float currentValue = _currentHealth;
        float fillSliderValue = currentValue / finishValue;
        _slider.value = fillSliderValue;
    }
    private void Die()
    {
        TextMeshPro prefab = Instantiate(_deathScorePrefab,transform.position, Quaternion.identity);
        prefab.canvas.worldCamera = _camera;
        prefab.text = $"+{_deathScore}";
        Destroy(prefab, 2);
        Player.AddScore(_deathScore);
        Instantiate(_deathEffect, transform.position, Quaternion.identity);
        GameObject loot = GenerateLootDrop();   
        if (loot != null)
        {
            Instantiate(loot, transform.position, Quaternion.identity);    
        }
        Destroy(gameObject); 
    }
    public GameObject GenerateLootDrop()
    {
        float totalDropChance = 0f;
        for (int i = 0; i < _itemDrops.Count; i++)
        {
            float dropChance = 1f / (i + 1);
            totalDropChance += dropChance;
        }
        float randomNumber = Random.Range(0f, totalDropChance);
        float currentChance = 0f;
        for (int i = 0; i < _itemDrops.Count; i++)
        {
            float dropChance = 1f / (i + 1);
            currentChance += dropChance;
            if (randomNumber <= currentChance)
            {
                return _itemDrops[i];
            }
        }
        return null;
    }
}
