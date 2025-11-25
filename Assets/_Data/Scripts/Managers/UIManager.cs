using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public PlayerControls controls;
    private PlayerController player;

    public TextMeshProUGUI ammoText;

    [SerializeField] GameObject defeatEnemy;
    int currentAmount;

    #region Panel
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject chooseDifficutyPanel;
    [SerializeField] private GameObject skillTreePanel;
    #endregion

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;

        player = FindAnyObjectByType<PlayerController>();
    }


    private void Start()
    {
        currentAmount = 5000;
        var text = defeatEnemy.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        text.text = currentAmount.ToString();

        ammoText.enabled = false;
        defeatEnemy.transform.GetChild(0).GetComponent<Image>().sprite = GameManager.instance.UIAtlas.GetSprite("Icon 0");
    }

    private void Update()
    {
        AssignInputEvents();
    }

    public void UpdateDefeatEnemy(int amount)
    {
        currentAmount += amount;
        var text = defeatEnemy.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        text.text = currentAmount.ToString();
    }

    public void MinusDefeatEnemy(int amount)
    {
        currentAmount -= amount;
        var text = defeatEnemy.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        text.text = currentAmount.ToString();
    }

    public void ShowSettingsPanel()
    {
        settingsPanel.SetActive(true);
        GameManager.instance.GamePause();
    }

    public void HideSettingsPanel()
    {
        settingsPanel.SetActive(false);
        GameManager.instance.GameResume();
    }

    public void ShowChooseDifficultyPanel()
    {
        settingsPanel.SetActive(false);
        chooseDifficutyPanel.SetActive(true);
        GameManager.instance.GamePause();
    }

    public void HideChooseDifficultyPanel()
    {
        ShowSettingsPanel();
        chooseDifficutyPanel.SetActive(false);
    }

    public int GetCurrentDefeatEnemy() => currentAmount;

    public void ShowSkillTreePanel()
    {
        skillTreePanel.SetActive(true);
        GameManager.instance.GamePause();
    }

    public void HideSkillTreePanel()
    {
        skillTreePanel.SetActive(false);
        GameManager.instance.GameResume();
    }

    private void AssignInputEvents()
    {
        controls = player.controls;

        controls.UI.Skill.performed += ctx =>
        {
            if (skillTreePanel.activeSelf)
            {
                HideSkillTreePanel();
            }
            else
                ShowSkillTreePanel();
        };

    }
}
