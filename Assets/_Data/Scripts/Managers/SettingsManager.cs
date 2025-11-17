using System;
using Unity.VisualScripting;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager instance;

    [Header("Elements")]
    private PlayerControls controls;
    [SerializeField] private PlayerController player;

    private bool isSettingOpen = false;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

    }

    private void Start()
    {
        AssignInputEvents();
    }

    private void ToggleSettings()
    {
        isSettingOpen = !isSettingOpen;

        if (isSettingOpen)
            UIManager.instance.ShowSettingsPanel();
        else
            UIManager.instance.HideSettingsPanel();
    }


    private void AssignInputEvents()
    {
        controls = player.controls;

        controls.UI.Settings.performed += ctx => ToggleSettings();
    }

}
