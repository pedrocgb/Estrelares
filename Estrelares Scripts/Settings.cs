using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    private static Settings instance = null;
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private Character _selectedCharacter = Character.None;
    public static Character SelectedCharacter { get { return instance._selectedCharacter; } }

    private int _starsCollected = 0;
    public static int GetStarsCollected { get { return instance._starsCollected; } }

    // ------------------------------------

    public static void SelectCharacter(Character NewCharacter)
    {
        if (instance == null) return;

        instance.selectCharacter(NewCharacter);
    }

    public static void IncrementStars()
    {
        if (instance == null) return;

        instance.incrementStars();
        GameManager.UpdateStarsUi();
    }

    public static void ResetStars()
    {
        if (instance == null) return;

        instance.resetStars();
        GameManager.UpdateStarsUi();
    }

    // ------------------------------------

    private void selectCharacter(Character newCharacter)
    {
        _selectedCharacter = newCharacter;
    }

    private void incrementStars()
    {
        _starsCollected++;
    }

    private void resetStars()
    {
        _starsCollected = 0;
    }
}

public enum Character
{
    None,
    Boy,
    Girl
}