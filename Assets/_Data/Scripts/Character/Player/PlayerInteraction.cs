using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    public PlayerControls controls;

    [Header("Exp Slider Setting")]
    [SerializeField] private List<float> expTable;
    [SerializeField] private ItemData exp;
    [SerializeField] private Slider expSlider;
    [SerializeField] private TextMeshProUGUI expText;
    [SerializeField] private TextMeshProUGUI levelText;
    private float _currentExp;
    private int _currentLevel;
    private float _expToNextLevel;

    [Header("Bullet")]
    [SerializeField] private Bullet bullet;

    [Header("Potion")]
    [SerializeField] private PlayerController player;

    [Header("Chest")]
    private ChestController nearbyChest;

    [Header("Select TakeDamaged")]
    [SerializeField] private AvailableUpgrade availableWeapon;

    [Header("Speed Item")]
    [SerializeField] private PlayerDash playerMovement;

    private void Start()
    {
        _currentLevel = 1;
        _currentExp = exp.value;
        _expToNextLevel = expTable[0];
        expSlider.value = _currentExp;
        expSlider.maxValue = _expToNextLevel;
        expText.text = $"{_currentExp}/{_expToNextLevel}";
        levelText.text = $"Level {_currentLevel}";
    }

    private void Update()
    {
        AssignInputEvents();


        ResetLevelUpBar();
    }

    private void ResetLevelUpBar()
    {
        if (_currentExp >= _expToNextLevel && _currentLevel < expTable.Count)
        {
            _currentExp -= _expToNextLevel;
            expSlider.DOValue(_currentExp, 0.2f).SetEase(Ease.Linear);

            LevelUp();
        }
    }

    void AddExp(float amountExp)
    {
        _currentExp += amountExp;
        _currentExp = Mathf.Clamp(_currentExp, 0, _expToNextLevel);
        expText.text = $"{_currentExp}/{_expToNextLevel}";
        expSlider.DOValue(_currentExp, 0.2f).SetEase(Ease.Linear);
    }

    void LevelUp()
    {
        _currentLevel++;
        _expToNextLevel = GetExpToNextLevel();
        expSlider.maxValue = _expToNextLevel;
        expText.text = $"{_currentExp}/{_expToNextLevel}";
        levelText.text = $"Level {_currentLevel}";
        ShowSelectWeaponPanel();

        SoundManager.instance.SetSoundState(SoundType.levelup);
    }

    private float GetExpToNextLevel()
    {
        if (_currentLevel < expTable.Count)
            return expTable[_currentLevel - 1];
        else
            return expTable[expTable.Count - 1];
    }

    private void ShowSelectWeaponPanel()
    {
        availableWeapon.SetWeaponAvailable();
        GameManager.instance.GamePause();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;

        if (collision.CompareTag("Exp1"))
        {
            if (collision.TryGetComponent<ExpController>(out var exp))
            {
                AddExp(exp.itemData.value);
            }
            ObjectPool.instance.DelayReturnToPool(collision.gameObject);
        }

        else if (collision.CompareTag("Exp2"))
        {
            if (collision.TryGetComponent<ExpController>(out var exp))
            {
                AddExp(exp.itemData.value);
            }
            ObjectPool.instance.DelayReturnToPool(collision.gameObject);
        }

        else if (collision.CompareTag("Exp3"))
        {
            if (collision.TryGetComponent<ExpController>(out var exp))
            {
                AddExp(exp.itemData.value);
            }
            ObjectPool.instance.DelayReturnToPool(collision.gameObject);
        }

        else if (collision.CompareTag("BulletItem"))
        {
            if (collision.TryGetComponent<BulletItemController>(out var bulletItem))
            {
                bullet.AddAmmo(bulletItem.itemData);
            }
            ObjectPool.instance.DelayReturnToPool(collision.gameObject);
        }

        else if (collision.CompareTag("Potion"))
        {
            if (collision.TryGetComponent<PotionController>(out var potion))
            {
                player.Heal(potion.itemData.value);
            }
            ObjectPool.instance.DelayReturnToPool(collision.gameObject);
        }

        else if (collision.CompareTag("Chest"))
        {
            if (collision.TryGetComponent<ChestController>(out var chest))
            {
                if (nearbyChest == null)
                    nearbyChest = chest;
            }
        }

        else if (collision.CompareTag("Speed"))
        {
            if (collision.TryGetComponent<SpeedItemController>(out var speed))
            {
                if (!player.IsBoostedSpeed())
                    StartCoroutine(player.BoostSpeed(speed.itemData.value, speed.itemData.timeLimit));
            }
            ObjectPool.instance.DelayReturnToPool(collision.gameObject);
        }

        else if (collision.CompareTag("Magnet"))
        {
            if (collision.TryGetComponent<MagnetController>(out var mag))
            {
                StartCoroutine(mag.MagnetEffect());
            }
            ObjectPool.instance.DelayReturnToPool(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Chest"))
        {
            if (nearbyChest != null && collision.GetComponent<ChestController>() == nearbyChest)
                nearbyChest = null;
        }
    }

    void UseChest()
    {
        if (nearbyChest != null && !nearbyChest.isOpened)
        {
            nearbyChest.OpenChest();
            nearbyChest = null;
        }
    }

    private void AssignInputEvents()
    {
        controls = player.controls;

        controls.Player.Interaction.performed += ctx => UseChest();
    }
}
