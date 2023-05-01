using Purgatory.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Purgatory.Upgrades;
using UnityEditor;
using Assets;
using System;
using Purgatory.Player;
using UnityEngine.SceneManagement;
using Purgatory.Levels;
using Purgatory.Player.Projectiles;

public class GameManager : MonoBehaviour
{
    public EnumGameState GameState = EnumGameState.MENU;
    public string IntroductionSceneName = "Intro_Dialogue";
    public string ReturnSceneName = "Return_Dialogue";
    public int CurrentLevel = 0;
    public int CurrencyAmount = 0;
    public int SoulAmount = 0;
    public List<Purgatory.Upgrades.UpgradeSciptableObject> StartingUpgrades = new List<UpgradeSciptableObject>();
    private PlayerSaveScriptableObject _playerSave;

    public static GameManager instance;
    private bool fading = false;
    internal List<GameObject> AvailableProjectiles;

    public GameManager()
    {
        if (instance == null)
            instance = this;
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {

        if (AssetDatabase.LoadAssetAtPath("Assets/PlayerSave.asset", typeof(PlayerSaveScriptableObject))==null)
        {
            AssetDatabase.CreateAsset(new PlayerSaveScriptableObject(), "Assets/PlayerSave.asset");
        }
        _playerSave = AssetDatabase.LoadAssetAtPath("Assets/PlayerSave.asset", typeof(PlayerSaveScriptableObject)) as PlayerSaveScriptableObject;
    }

    public void SetGameState(EnumGameState state)
    {
        GameState = state;
        if(state != EnumGameState.MENU && state != EnumGameState.DIALOGUE && state != EnumGameState.STORE)
             UpgradeController.instance.RefreshStats();
    }

    public void CompleteLevel()
    {
        SoulAmount = SoulCollectionController.instance.Souls;
        CurrencyAmount = CurrencyController.Instance.CurrencyAmount;
    }

    public void GameOver()
    {
        SoulAmount = SoulCollectionController.instance.Souls;
        CurrencyAmount = CurrencyController.Instance.CurrencyAmount;
        var soulsToRemove = SoulAmount-Mathf.RoundToInt(SoulAmount * SoulCollectionController.instance.SoulRetentionRate);
        SoulCollectionController.instance.RemoveSoul(soulsToRemove);
        UpgradeController.instance.RefreshStats();
        _playerSave.Upgrades = UpgradeController.instance.Upgrades;
        LoadScene("Death_Shop");
    }

    public void LoadScene(string scene)
    {
        Initiate.Fade(scene, Color.black, 1.0f);
    }

    public void Exit()
    {
            Application.Quit();
    }

    public void NewGame()
    {
        _playerSave.Upgrades = StartingUpgrades;
        _playerSave.Souls = 0;
        _playerSave.CurrencyAmount = 0;
        _playerSave.CurrentLevel = 0;
        UpgradeController.instance.Upgrades = _playerSave.Upgrades;
        LoadScene(IntroductionSceneName);
    }

    public void NextRun()
    {
        CurrentLevel = 0;
        LoadScene(IntroductionSceneName);
    }

    public void StartGame()
    {
        LoadScene("Level_1");
        UpgradeController.instance.RefreshStats();
    }


    public void Credits()
    {

    }

}
