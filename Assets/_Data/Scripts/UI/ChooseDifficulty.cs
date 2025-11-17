using System.Collections.Generic;
using UnityEngine;

public class ChooseDifficulty : MonoBehaviour
{
    [SerializeField] List<DifficultData> list = new();
    [SerializeField] private SpawnEnemy spawnEnemy;

    public void ChooseDifficult(string diff)
    {
        var difficult = list.Find(d => d.difficult == diff);
        if (difficult == null)
        {
            Debug.LogError("Difficult not found");
            return;
        }

        GameManager.instance.currentDifficultData = difficult;
        PlayerPrefs.SetString("Difficulty", diff);
        PlayerPrefs.Save();
        GameManager.instance.PlayGame();
    }

    public void ChooseEz() => ChooseDifficult("Easy");
    public void ChooseNormal() => ChooseDifficult("Normal");
    public void ChooseHard() => ChooseDifficult("Hard");

    //public void ChooseAsia() => ChooseDifficult("Asia");
}
