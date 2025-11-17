using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public TextMeshProUGUI ammoText;

    [SerializeField] GameObject defeatEnemy;
    int currentAmount = 0;

    #region Panel
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject chooseDifficutyPanel;
    #endregion

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
    }


    private void Start()
    {
        ammoText.enabled = false;
        defeatEnemy.transform.GetChild(0).GetComponent<Image>().sprite = GameManager.instance.UIAtlas.GetSprite("Icon 0");
    }

    public void UpdateDefeatEnemy(int amount)
    {
        currentAmount += amount;
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


}
