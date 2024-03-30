using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private Animator _fadePanel = null;
    [SerializeField]
    private Button[] _buttons = null;

    public void StartGame()
    {
        StartCoroutine(LoadLevel(1, false));
    }

    #region Load Level
    private IEnumerator LoadLevel(int LevelId, bool Exit)
    {
        _fadePanel.gameObject.SetActive(true);
        _fadePanel.Play("Fade In");

        foreach (Button b in _buttons)
        {
            b.interactable = false;
        }
        yield return new WaitForSeconds(1f);

        if (Exit)
            Application.Quit();
        else
            StartCoroutine(LoadAsynchronously(LevelId));

    }

    private IEnumerator LoadAsynchronously(int Id)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(Id);

        while(!operation.isDone)
        {
            yield return null;
        }
    }
    #endregion

    public void SelectBoyButton()
    {
        Settings.SelectCharacter(Character.Boy);
        _buttons[0].interactable = true;
        _buttons[2].interactable = false;

        _buttons[3].interactable = true;
    }

    public void SelectGirlButton()
    {
        Settings.SelectCharacter(Character.Girl);
        _buttons[0].interactable = true;
        _buttons[3].interactable = false;

        _buttons[2].interactable = true;
    }

    public void ExitGame()
    {
        StartCoroutine(LoadLevel(0, true));
    }
}
