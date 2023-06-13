using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SeasonService : MonoBehaviour
{
    [SerializeField] private List<Season> seasons;
    [SerializeField] private Button menuButton;
    private void Start()
    {
      seasons.ForEach(s=>s.Init());
        seasons.Where(s=> s.GetSavedPlace == 1).ToList().ForEach(s=>s.isOpen = true);
        var lastWinSeason = seasons.LastOrDefault(s => s.GetSavedPlace == 1);
        if(lastWinSeason != null)
        {
            var indexLastWin = seasons.IndexOf(lastWinSeason);
            if(indexLastWin + 1 < seasons.Count) 
            {
                seasons[indexLastWin + 1].isOpen = true ;
            }
        }
        seasons.ForEach(s => s.RefreshState());
        menuButton.onClick.AddListener(() => { SceneManager.LoadScene("GeneralMenu"); });
    }
}
