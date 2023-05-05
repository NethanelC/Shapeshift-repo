using System;
using System.Linq;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Shop : MonoBehaviour
{
    [SerializeField] private EventSystem _eventSystem;
    [SerializeField] private TextMeshProUGUI _textPoints, _textAfterPurchase;
    [SerializeField] private GameObject _buttonOkay, _buttonYes, _buttonNo, _buttonPurchase, _canvasMenu;
    [SerializeField] private Button[] _ammoButtons = new Button[7];
    [SerializeField] private Animator[] _ammoAnimators = new Animator[7];
    [SerializeField] private Button[] _speedButtons = new Button[7];
    [SerializeField] private Animator[] _speedAnimators = new Animator[7];
    [SerializeField] private Button[] _timerButtons = new Button[7];
    [SerializeField] private Animator[] _timerAnimators = new Animator[7];
    [SerializeField] private Button[] _bulletButtons = new Button[5];
    [SerializeField] private Animator[] _bulletAnimators = new Animator[5];
    private Button _selectedButton;
    private string _stringAfterPurchase;
    private int _buttonPrice, _buttonUpgradeKind;
    public void OnEnable()
    {
        int prefsAmmo = PlayerPrefs.GetInt("AmmoButton");
        int prefsSpeed = PlayerPrefs.GetInt("SpeedButton");
        int prefsTimer = PlayerPrefs.GetInt("TimerButton");
        int prefsBullet = PlayerPrefs.GetInt("BulletButton");
        _textPoints.text = PlayerPrefs.GetInt("PlayerPoints").ToString();
        if (prefsAmmo < _ammoButtons.Length)
        {
            _ammoAnimators[prefsSpeed].SetTrigger("Unlockable");
        }
        for (prefsAmmo++; prefsAmmo < _ammoButtons.Length; prefsAmmo++)
        {
            _ammoAnimators[prefsAmmo].SetTrigger("Locked");
        }
        if (prefsSpeed < _speedButtons.Length)
        {
            _speedAnimators[prefsSpeed].SetTrigger("Unlockable");
        }
        for (prefsSpeed++; prefsSpeed < _speedButtons.Length; prefsSpeed++)
        {
            _speedAnimators[prefsSpeed].SetTrigger("Locked");
        }
        if (prefsTimer < _timerButtons.Length)
        {
            _timerAnimators[prefsTimer].SetTrigger("Unlockable");
        } 
        for (prefsTimer++; prefsTimer < _timerButtons.Length; prefsTimer++)
        {
            _timerAnimators[prefsTimer].SetTrigger("Locked");
        }
        if (prefsBullet < _bulletButtons.Length)
        {
            _bulletAnimators[prefsBullet].SetTrigger("Unlockable");
        }
        for (prefsBullet++; prefsBullet < _bulletButtons.Length; prefsBullet++)
        {
            _bulletAnimators[prefsBullet].SetTrigger("Locked");
        }
    }
    private void Update()
    {
        bool escKey = Input.GetKeyDown(KeyCode.Escape);
        if (escKey)
        {
            if (_selectedButton != null)
            {
                UnselectButton();
            }
            else
            {
                gameObject.SetActive(false);
                _canvasMenu.SetActive(true);
            }
        }
    }
    public void PurchaseButton()
    {
        switch (_buttonUpgradeKind)
        {
            case 1:
                int prefsAmmo = PlayerPrefs.GetInt("AmmoButton");
                _buttonPrice = prefsAmmo * 2;
                _stringAfterPurchase = "Ammo upgrade #" + prefsAmmo + "\n\n Cost: " + _buttonPrice;
                break;
            case 2:
                int prefsSpeed = PlayerPrefs.GetInt("SpeedButton");
                _buttonPrice = prefsSpeed * 2;
                _stringAfterPurchase = "Speed upgrade #" + prefsSpeed + "\n\n Cost: " + _buttonPrice;
                break;
            case 3:
                int prefsTimer = PlayerPrefs.GetInt("TimerButton");
                _buttonPrice = prefsTimer * 2;
                _stringAfterPurchase = "Timer upgrade #" + prefsTimer + "\n\n Cost: " + _buttonPrice;
                break;
            case 4:
                int prefsBullet = PlayerPrefs.GetInt("BulletButton");
                _buttonPrice = prefsBullet * 2;
                _stringAfterPurchase = "Bullet upgrade #" + prefsBullet + "\n\n Cost: " + _buttonPrice;
                break;
        }
        if (PlayerPrefs.GetInt("PlayerPoints") >= _buttonPrice)
        {
            _textAfterPurchase.text = "Are you sure you want to buy\n\n" + _stringAfterPurchase;
            _buttonYes.SetActive(true);
            _buttonNo.SetActive(true); 
        }
        else
        {
            _textAfterPurchase.text = "You don't have enough points to buy\n\n" + _stringAfterPurchase;
            _buttonOkay.SetActive(true); 
        }
    }
    public void PurchaseAction()
    {
        PlayerPrefs.SetInt("PlayerPoints", PlayerPrefs.GetInt("PlayerPoints") - _buttonPrice);
        _textPoints.text = PlayerPrefs.GetInt("PlayerPoints").ToString();
        switch (_buttonUpgradeKind)
        {
            case 1:
                PlayerPrefs.SetInt("AmmoButton", PlayerPrefs.GetInt("AmmoButton") + 1);
                int prefsAmmo = PlayerPrefs.GetInt("AmmoButton");
                _ammoAnimators[prefsAmmo - 1].SetTrigger("Unlocked");
                if (prefsAmmo < _ammoButtons.Length)
                {
                    _ammoAnimators[prefsAmmo].SetTrigger("Selected");
                    _ammoButtons[prefsAmmo].Select();
                    SelectedButton(_ammoButtons[prefsAmmo]);
                }
                PlayerPrefs.SetInt("PlayerAmmoUpgrade", PlayerPrefs.GetInt("PlayerAmmoUpgrade") + 1);
                break;
            case 2:
                PlayerPrefs.SetInt("SpeedButton", PlayerPrefs.GetInt("SpeedButton") + 1);
                int prefsSpeed = PlayerPrefs.GetInt("SpeedButton");
                _speedAnimators[prefsSpeed - 1].SetTrigger("Unlocked");
                if (prefsSpeed < _speedButtons.Length)
                {
                    _speedAnimators[prefsSpeed].SetTrigger("Selected");
                    _speedButtons[prefsSpeed].Select();
                    SelectedButton(_speedButtons[prefsSpeed]);
                }
                PlayerPrefs.SetInt("PlayerSpeedUpgrade", PlayerPrefs.GetInt("PlayerSpeedUpgrade") + 1);
                break;
            case 3:
                PlayerPrefs.SetInt("TimerButton", PlayerPrefs.GetInt("TimerButton") + 1);
                int prefsTimer = PlayerPrefs.GetInt("TimerButton");
                _timerAnimators[prefsTimer - 1].SetTrigger("Unlocked");
                if (prefsTimer < _timerButtons.Length)
                {
                    _timerAnimators[prefsTimer].SetTrigger("Selected");
                    _timerButtons[prefsTimer].Select();
                    SelectedButton(_timerButtons[prefsTimer]);
                }
                PlayerPrefs.SetInt("PlayerTimerUpgrade", PlayerPrefs.GetInt("PlayerTimerUpgrade") + 1);
                break;
            case 4:
                PlayerPrefs.SetInt("BulletButton", PlayerPrefs.GetInt("BulletButton") + 1);
                int prefsBullet = PlayerPrefs.GetInt("BulletButton");
                _bulletAnimators[prefsBullet - 1].SetTrigger("Unlocked");
                if (prefsBullet < _bulletButtons.Length)
                {
                    _bulletAnimators[prefsBullet].SetTrigger("Selected");
                    _bulletButtons[prefsBullet].Select();
                    SelectedButton(_bulletButtons[prefsBullet]);
                }
                PlayerPrefs.SetInt("PlayerBulletUpgrade", PlayerPrefs.GetInt("PlayerBulletUpgrade") + 1);
                break;
        }
    }
    public void SelectedButton(Button selectedbutton)
    {
        _selectedButton = selectedbutton;
    }
    public void ThisButtonKind(int buttonkind)
    {
        _buttonUpgradeKind = buttonkind;
    }
    public void UnselectButton()
    {
        if (_selectedButton != null)
        {
            switch (_buttonUpgradeKind)
            {
                case 1:
                    if (PlayerPrefs.GetInt("AmmoButton") < _ammoAnimators.Length)
                    {
                        _ammoAnimators[PlayerPrefs.GetInt("AmmoButton")].SetTrigger("Unlockable");
                    }
                    break;
                case 2:
                    if (PlayerPrefs.GetInt("SpeedButton") < _speedAnimators.Length)
                    {
                        _speedAnimators[PlayerPrefs.GetInt("SpeedButton")].SetTrigger("Unlockable");
                    }  
                    break;
                case 3:
                    if (PlayerPrefs.GetInt("TimerButton") < _timerAnimators.Length)
                    {
                        _timerAnimators[PlayerPrefs.GetInt("TimerButton")].SetTrigger("Unlockable");
                    }
                    break;
                case 4:
                    if (PlayerPrefs.GetInt("BulletButton") < _bulletAnimators.Length)
                    {
                        _bulletAnimators[PlayerPrefs.GetInt("BulletButton")].SetTrigger("Unlockable");
                    }
                    break ;
            }
            _selectedButton = null;
            _eventSystem.SetSelectedGameObject(null);
            _buttonPurchase.SetActive(false);
        }
    }
}
