using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public static Tutorial Instance;
    
    public GameObject tutorialPanel;
    public GameObject[] pagesObjects;
    private int _currentPageIndex;
    private const int TotalPages = 3;
    
    void Awake()
    {
        Instance = this;
    }

    public void Navigate(int pageIndex)
    {
        if (pageIndex is < 0 or >= TotalPages) return;
        
        for (var i = 0; i < pagesObjects.Length; i++)
        {
            pagesObjects[i].SetActive(i == _currentPageIndex);
        }
    }

    public void NextTutorialPage()
    {
        _currentPageIndex = Mathf.Clamp(_currentPageIndex + 1, 0, TotalPages - 1);

        Navigate(_currentPageIndex);
    }

    public void PreviousTutorialPage()
    {
        _currentPageIndex = Mathf.Clamp(_currentPageIndex - 1, 0, TotalPages);

        Navigate(_currentPageIndex);
    }

    public void ShowPanel()
    {
        tutorialPanel.SetActive(!tutorialPanel.activeSelf);
        Navigate(_currentPageIndex);
    }
}