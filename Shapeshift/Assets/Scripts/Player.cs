using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Player : MonoBehaviour
{    
    [SerializeField] private float _levelTimer;
    [SerializeField] private ParticleSystem _particleDeath;
    [SerializeField] private Transform _transformGun;
    [SerializeField] private Rigidbody2D _rbGun;
    [SerializeField] private Dupe _dupePrefab;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private PolygonCollider2D _collider;
    [SerializeField] private Camera _camera;
    [SerializeField] private Animator _animTimer;
    [SerializeField] private GameObject _panelLose, _panelEsc, _panelFinish;
    [SerializeField] private Image _leftImage, _midImage, _rightImage, _cooldownImage, _bulletImage;
    [SerializeField] private TextMeshProUGUI _timerText, _ammoText, _finishTimerText;
    [SerializeField] private int[] _ammoUpgrades = new int[7];
    [SerializeField] private float[] _speedUpgrades = new float[7];
    [SerializeField] private float[] _timerUpgrades = new float[7];
    [SerializeField] private Rigidbody2D[] _bulletUpgrades = new Rigidbody2D[5];
    [SerializeField] private SpriteRenderer[] _bulletImages = new SpriteRenderer[5];
    private static readonly List <Sprite> shifters_Sprites = new();
    private static readonly List <Vector2[]> shifters_Colliders = new();
    private static Image _leftImageS, _rightImageS;
    private static TextMeshProUGUI _timerTextS, _ammoTextS;
    private static int _score;
    private static int _playerAmmo;
    private static float _realTimer;
    private static int _shiftersIndex;
    private readonly float shifters_Cooldown = 2;
    private readonly float _speedBullet = 20;
    private float _playerSpeed, _playerTimer, _maximumTimer;
    private string _timerDecimalString;
    private bool shifters_OnCooldown;
    private bool _isDead;
    private bool _isPaused;
    private Rigidbody2D _bulletPrefab;
    private Vector3 mousePos;
    private Vector2 movement;
    private void Start()
    {
        _leftImageS = _leftImage;
        _rightImageS = _rightImage;
        _ammoTextS = _ammoText;
        _timerTextS = _timerText;
        shifters_Sprites.Clear();
        shifters_Colliders.Clear();
        shifters_Sprites.Add(_spriteRenderer.sprite);
        shifters_Colliders.Add(_collider.points);
        _collider.points = shifters_Colliders[_shiftersIndex];
        _spriteRenderer.sprite = shifters_Sprites[_shiftersIndex];
        _playerAmmo = _ammoUpgrades[PlayerPrefs.GetInt("PlayerAmmoUpgrade")];
        _playerSpeed = _speedUpgrades[PlayerPrefs.GetInt("PlayerSpeedUpgrade")];
        _playerTimer = _timerUpgrades[PlayerPrefs.GetInt("PlayerTimerUpgrade")];
        _bulletPrefab = _bulletUpgrades[PlayerPrefs.GetInt("PlayerBulletUpgrade")];
        _bulletImage.sprite = _bulletImages[PlayerPrefs.GetInt("PlayerBulletUpgrade")].sprite;
        _maximumTimer = _levelTimer + _playerTimer;
        _realTimer = _maximumTimer;
        _timerText.text = $"{_realTimer:F0}";
        _ammoText.text = $"{_playerAmmo}";
        StartCoroutine(Timer());
    }
    private void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        bool mouseButton = Input.GetMouseButtonDown(0);
        bool shiftButton = Input.GetKeyDown(KeyCode.LeftShift);
        bool spaceButton = Input.GetKeyDown(KeyCode.Space);
        bool escKey = Input.GetKeyDown(KeyCode.Escape);
        bool anyKey = Input.anyKeyDown;
        mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
        _cooldownImage.fillAmount -= 1 / shifters_Cooldown * Time.deltaTime;
        if (!_isDead)
        {
            if (mouseButton)
            {
                Shoot();
            }
            else if (spaceButton)
            {
                Duplicate();
            }   
            else if (shiftButton)
            {
                StartCoroutine(Shift());
            }
            else if (escKey)
            {
                PauseUnpause();
            }
            if (_realTimer > 0)
            {
                _realTimer -= Time.deltaTime;
                _timerText.text = _realTimer.ToString(_timerDecimalString);
            }
            else
            {   
                _realTimer = 0; 
            }  
        }
        else
        {
            if (anyKey)
            {
                Respawn();
            } 
        }
    }
    private void FixedUpdate()
    {
        if (!_isDead)
        {
            transform.Translate(_playerSpeed * Time.fixedDeltaTime * movement);
            _transformGun.position = transform.position;
            Vector2 lookDirection = mousePos - _transformGun.position;
            float lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
            _rbGun.rotation = lookAngle;
        }        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collObject = collision.gameObject;
        if (collObject.layer == 11)
        {
            StartCoroutine(Die());
        }
        else if (collObject.layer == 14)
        {
            _isDead = true;
            _rbGun.gameObject.SetActive(false);
            _finishTimerText.text = $"{_realTimer:F2}s left";
            _panelFinish.SetActive(true);
            string sceneName = SceneManager.GetActiveScene().name;
            if (_score > PlayerPrefs.GetInt(sceneName))
            {
                int scoreDelta = _score - PlayerPrefs.GetInt(sceneName);
                PlayerPrefs.SetInt(sceneName, _score); 
                PlayerPrefs.SetInt("PlayerPoints", PlayerPrefs.GetInt("PlayerPoints") + scoreDelta);
            } 
            if (!PlayerPrefs.HasKey(sceneName + "Timer") || _realTimer > PlayerPrefs.GetFloat(sceneName + "Timer"))
            {
                PlayerPrefs.SetFloat(sceneName + "Timer", _realTimer);
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.layer == 8)
        {
            StartCoroutine(Die());
        }
    }
    private void Shoot()
    {
        if (_isPaused || _playerAmmo == 0)
        {
            return;
        }
        Instantiate(_bulletPrefab, _transformGun.position, _transformGun.rotation).AddForce(_speedBullet * _transformGun.up, ForceMode2D.Impulse);;
        _playerAmmo--;
        _ammoText.text = $"{_playerAmmo}";
    }
    private void Duplicate()
    {
        if (_isPaused || shifters_Sprites.Count <= 1)
        {
            return;
        }
        Instantiate(_dupePrefab, transform.position, Quaternion.identity).SetSpriteAndCollider(_spriteRenderer.sprite, _collider.points);
        shifters_Sprites.Remove(shifters_Sprites[_shiftersIndex]);
        shifters_Colliders.Remove(shifters_Colliders[_shiftersIndex]);
        _shiftersIndex++;
        if (_shiftersIndex >= shifters_Sprites.Count)
        {
            _shiftersIndex = 0;
        }
        _midImage.sprite = shifters_Sprites[_shiftersIndex];
        _spriteRenderer.sprite = shifters_Sprites[_shiftersIndex];
        _collider.points = shifters_Colliders[_shiftersIndex];
        Shapedoor.ShiftSprite(_spriteRenderer.sprite);
        if (shifters_Sprites.Count == 2)
        {
            _leftImage.gameObject.SetActive(false);
            _rightImage.sprite = GetNextSprite();
        }
        else if (shifters_Sprites.Count == 1)
        {
            _rightImage.gameObject.SetActive(false);
        }
    }

    private IEnumerator Shift()
    {
        if (shifters_OnCooldown || _isPaused || shifters_Sprites.Count < 2)
        {
            yield break;
        }
        shifters_OnCooldown = true;
        _cooldownImage.fillAmount = 1;
        _shiftersIndex++;
        if (_shiftersIndex >= shifters_Sprites.Count)
        {
            _shiftersIndex = 0;
        }
        _rightImage.sprite = GetNextSprite();
        _leftImage.sprite = GetNextNextSprite();
        _midImage.sprite = shifters_Sprites[_shiftersIndex];
        _spriteRenderer.sprite = shifters_Sprites[_shiftersIndex];
        _collider.points = shifters_Colliders[_shiftersIndex];
        Shapedoor.ShiftSprite(_spriteRenderer.sprite);
        yield return new WaitForSeconds(shifters_Cooldown);
        shifters_OnCooldown = false;
    }
    private IEnumerator Timer()
    {
        _timerDecimalString = "F0";
        yield return new WaitForSeconds((_maximumTimer / 2) - 0.5f);
        if (!_isDead)
        {
            _timerText.color = Color.yellow;
        }
        yield return new WaitForSeconds((_maximumTimer / 2) -10f);
        if (!_isDead)
        {
            _timerText.color = Color.red;
            _animTimer.SetTrigger("BelowTen");
        }
        yield return new WaitForSeconds(0.5f);
        _timerDecimalString = "F2";
        yield return new WaitForSeconds(10);
        _timerText.text = $"{_realTimer:F0}";
        _animTimer.SetTrigger("Idle");
        if (_isDead)
        {
            StartCoroutine(Die());
        }
    }
    private IEnumerator Die()
    {
        _isDead = true;
        _spriteRenderer.enabled = false;
        _collider.enabled = false;
        _rbGun.gameObject.SetActive(false);
        _panelLose.SetActive(true);
        _particleDeath.Play();
        yield return new WaitForSeconds(2);
        _isPaused = true;
    }
    public void Respawn()
    {
        if (_isPaused)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            _isPaused = false;
        }
    }
    private void PauseUnpause()
    {
        if (!_panelEsc.activeSelf)
        {
            TimePause();
            _panelEsc.SetActive(true);
            _isPaused = true;
        }
        else
        {
            TimeUnPause();
            _panelEsc.SetActive(false);
            _isPaused = false;
        }
    }
    public void TimePause()
    {
        Time.timeScale = 0;
    }
    public void TimeUnPause()
    {
        Time.timeScale = 1;
    }
    public void MenuScene()
    {
        SceneManager.LoadScene("Menu");
    }
    public void NextLevelScene()
    {
        if (SceneManager.GetActiveScene().name == "Level2")
        {
            MenuScene();
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        } 
    }
    private static Sprite GetNextSprite()
    {
        int nextSpriteIndex = _shiftersIndex + 1;
        if (nextSpriteIndex >= shifters_Sprites.Count)
            nextSpriteIndex = 0;
        return shifters_Sprites[nextSpriteIndex];
    }
    private static Sprite GetNextNextSprite()
    {
        int nextNextSpriteIndex = _shiftersIndex + 2;
        if (nextNextSpriteIndex >= shifters_Sprites.Count)
            nextNextSpriteIndex -= shifters_Sprites.Count;
        return shifters_Sprites[nextNextSpriteIndex];
    }
    public static void AddScore(int score)
    {
        _score += score;
    }
    public static void AddSpriteCollider(Sprite sprite, Vector2[] points)
    {
        shifters_Sprites.Add(sprite);
        shifters_Colliders.Add(points);
        _rightImageS.sprite = GetNextSprite();
        if (shifters_Sprites.Count == 2)
        {
            _rightImageS.gameObject.SetActive(true);
        }
        else if (shifters_Sprites.Count == 3)
        {
            _leftImageS.sprite = GetNextNextSprite();
            _leftImageS.gameObject.SetActive(true);
        } 
    }
    public static void AddAmmo(int ammo)
    {
        _playerAmmo += ammo;
        _ammoTextS.text = $"{_playerAmmo}";
    }
    public static void AddTime(int time)
    {
        _realTimer += time;
        _timerTextS.text = $"{_realTimer}";
    }
}