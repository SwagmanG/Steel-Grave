using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField] private Image heatBar;

    [SerializeField] private TMP_Text score;
    [SerializeField] private TMP_Text highScore;

    [SerializeField] private GameObject[] Layouts;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

    }

    private void Start()
    {
        ActivateStart();
    }

    // layouts
    private enum MenuLayouts
    {
        Start = 0,
        Game = 1,
        Pause = 2,
        End = 3,
    }

    public GameObject startFirstButton;
    public GameObject pauseFirstButton;
    public GameObject endFirstButton;


    public void OnStartButton()
    {
        Debug.Log("Start Button");
        GameManager.Instance.OnStartGame();
        ActivateGame();
    }
    /*public void OnRestartButton()
    {
        Debug.Log("Restart Button");
        GameManager.Instance.OnResetGame();
        ActivateGame();
    }*/
    public void OnPauseButton()
    {
        Debug.Log("Pause Button");
        GameManager.Instance.OnOptions();
        ActivatePause();
    }

    public void OnBackButton()
    {
        Debug.Log("Back Button");
        GameManager.Instance.OnOptions();
        ActivateStart();
    }

    public void OnQuitButton()
    {
        Debug.Log("Quit Button");
        GameManager.Instance.OnQuitGame();
    }

    // LAYOUTS
    private void SetLayout(MenuLayouts layout)
    {
        for (int i = 0; i < Layouts.Length; i++)
        {
            Layouts[i].SetActive((int)layout == i);
        }
        Debug.Log($"Setting layout {layout}");
    }

    // GAME STATES
    public void ActivateStart()
    {
        SetLayout(MenuLayouts.Start); 
        EventSystem.current.SetSelectedGameObject(startFirstButton);
    }

    public void ActivateGame()
    {
        SetLayout(MenuLayouts.Game);
    }
    public void ActivatePause()
    {
        SetLayout(MenuLayouts.Pause);
        EventSystem.current.SetSelectedGameObject(pauseFirstButton);
    }

    public void ActivateEnd()
    {
        SetLayout(MenuLayouts.End);
        EventSystem.current.SetSelectedGameObject(endFirstButton);
    }

    // UI ELEMENTS
    public void SetHeatFill(float _fill)
    {
        heatBar.fillAmount = _fill;
    }

    public void SetScore(int _score, int _highScore)
    {
        score.text = _score.ToString();
        highScore.text = _highScore.ToString();
    }

}
