using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prefs : MonoBehaviour
{
    private void Awake()
    {
       //PlayerPrefs.DeleteAll();
        if (PlayerPrefs.GetInt("FirstTime") == 0)
        {
            PlayerPrefs.SetInt("FirstTime", 1);
            PlayerPrefs.SetInt("AmmoButton", 1);
            PlayerPrefs.SetInt("SpeedButton", 1);
            PlayerPrefs.SetInt("TimerButton", 1);
            PlayerPrefs.SetInt("BulletButton", 1);
            PlayerPrefs.SetInt("PlayerPoints", 10);
            PlayerPrefs.SetInt("PlayerAmmoUpgrade", 0);
            PlayerPrefs.SetInt("PlayerSpeedUpgrade", 0);
            PlayerPrefs.SetInt("PlayerTimerUpgrade", 0);
            PlayerPrefs.SetInt("PlayerBulletUpgrade", 0);
        }
    }
}
