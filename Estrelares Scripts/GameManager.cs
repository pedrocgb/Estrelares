using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    private List<IResetable> _resetableObjects = new List<IResetable>();

    [BoxGroup("Game Over")]
    [SerializeField]
    private bool _lastLevel = false;
    [BoxGroup("Game Over")]
    [SerializeField]
    private int _nextLevelId = 1;

    [BoxGroup("Components")]
    [SerializeField]
    private GameObject _introScreen = null;
    [BoxGroup("Components")]
    [SerializeField]
    private GameObject _tutorialScreen = null;
    [BoxGroup("Components")]
    [SerializeField]
    private Animator _fadePanelAnimator = null;
    [BoxGroup("Components")]
    [SerializeField]
    private List<Collectable> _stars = new List<Collectable>();
    [BoxGroup("Components")]
    [SerializeField]
    private Collectable _masterCollectable = null;
    [BoxGroup("Components")]
    [SerializeField]
    private TextMeshProUGUI _starsUi = null;

    [BoxGroup("Components")]
    [SerializeField]
    private Image[] _playerImage = null;
    [BoxGroup("Components")]
    [SerializeField]
    private Sprite _boySprite = null;
    [BoxGroup("Components")]
    [SerializeField]
    private Sprite _girlSprite = null;
    [BoxGroup("Components")]
    [SerializeField]
    private GameObject _gameOverPanelOne = null;
    [BoxGroup("Components")]
    [SerializeField]
    private GameObject _gameOverPanelTwo = null;
    [BoxGroup("Components")]
    [SerializeField]
    private GameObject _endGamePanel = null;

    //------------------------------------------------------------------

    private void Awake()
    {
        if (instance == null)
            instance = this;        
    }

    private void Start()
    {
        StartCoroutine(playFadeAnimation(false));
        Time.timeScale = 0;

        if (_playerImage != null)
        {
            if (Settings.SelectedCharacter == Character.Boy)
            {
                foreach (Image i in _playerImage)
                {
                    i.sprite = _boySprite;
                }
            }
            else if (Settings.SelectedCharacter == Character.Girl)

            {
                foreach (Image i in _playerImage)
                {
                    i.sprite = _girlSprite;
                }
            }
        }

        updateStarsUi();
    }

    //------------------------------------------------------------------

    public static void AddResetableObect(IResetable Obj)
    {
        if (instance == null)
            return;

        instance.addResetableObject(Obj);
    }

    public static void ResetGame()
    {
        if (instance == null)
            return;

        instance.resetGame();
    }

    public static void GameOver()
    {
        if (instance == null)
            return;

        instance.StartCoroutine(instance.gameOver());
    }

    public static void UpdateStarsUi()
    {
        if (instance == null)
            return;

        instance.updateStarsUi();
    }
    //------------------------------------------------------------------

    public void StartGameButton()
    {
        StartCoroutine(startGame());
    }

    public void ContinueButton()
    {
        _introScreen.SetActive(false);
    }

    public void GameOverButtonOne()
    {
        _gameOverPanelOne.SetActive(false);
        _gameOverPanelTwo.SetActive(true);
    }

    public void GameOverButtonTwo()
    {
        _gameOverPanelTwo.SetActive(false);
        _endGamePanel.SetActive(true);
    }

    public void EndGameButton()
    {
        SceneManager.LoadScene(0);
    }

    private void updateStarsUi()
    {
        _starsUi.text = Settings.GetStarsCollected.ToString();
    }

    private IEnumerator startGame()
    {
        _fadePanelAnimator.gameObject.SetActive(true);
        _fadePanelAnimator.Play("Fade In");

        yield return new WaitForSecondsRealtime(1f);

        _tutorialScreen.SetActive(false);
        _fadePanelAnimator.Play("Fade Out");


        yield return new WaitForSecondsRealtime(1f);

        Time.timeScale = 1;
        _fadePanelAnimator.gameObject.SetActive(false);
    }

    private void addResetableObject(IResetable obj)
    {
        _resetableObjects.Add(obj);
    }

    private void resetGame()
    {
        foreach (IResetable o in _resetableObjects)
        {
            o.ResetObject();

        }

        foreach (Collectable c in _stars)
        {
            c.gameObject.SetActive(true);
        }
        _masterCollectable.gameObject.SetActive(true);
        Settings.ResetStars();
    }

    private IEnumerator gameOver()
    {
        _fadePanelAnimator.gameObject.SetActive(true);
        _fadePanelAnimator.Play("Fade In");
        yield return new WaitForSeconds(1f);

        if (_lastLevel)
        {
            _gameOverPanelOne.SetActive(true);
            _fadePanelAnimator.Play("Fade Out");
            
        }
        else
            SceneManager.LoadSceneAsync(_nextLevelId);
    }

    private IEnumerator playFadeAnimation(bool FadeIn)
    {
        _fadePanelAnimator.gameObject.SetActive(true);
        if (FadeIn)
        {           
            _fadePanelAnimator.Play("Fade In", 0, 0f);
            yield return new WaitForSeconds(1f);

            _fadePanelAnimator.gameObject.SetActive(false);
        }
        else
        {
            _fadePanelAnimator.Play("Fade Out", 0, 0f);
            yield return new WaitForSeconds(1f);

            _fadePanelAnimator.gameObject.SetActive(false);
        }
    }
}
