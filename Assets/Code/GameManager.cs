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
    public int CurrentEnviroment = 0;
    public int CurrencyAmount = 0;
    public int SoulAmount = 0;

    private PlayerSaveScriptableObject _playerSave;

    public List<Purgatory.Upgrades.UpgradeSciptableObject> StartingUpgrades = new List<UpgradeSciptableObject>();

    public static GameManager instance;

    public MusicManager musicManager;
    private bool fading = false;
    internal List<ProjectileModifier> AvailableModifiers;

    private LevelController levelController;

    private bool inLevel = false;


    private void Awake()
    {

        if (GameManager.instance == null)
            GameManager.instance = this;
        else
            Destroy(this.gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (AssetDatabase.LoadAssetAtPath("Assets/PlayerSave.asset", typeof(PlayerSaveScriptableObject)) == null)
        {
            AssetDatabase.CreateAsset(new PlayerSaveScriptableObject(), "Assets/PlayerSave.asset");
        }
        _playerSave = AssetDatabase.LoadAssetAtPath("Assets/PlayerSave.asset", typeof(PlayerSaveScriptableObject)) as PlayerSaveScriptableObject;

        musicManager.PlayShopMusic();
    }
    public void SetGameState(EnumGameState state)
    {
    }

    public void Save()
    {
        _playerSave.CurrencyAmount = CurrencyAmount;
        _playerSave.CurrentLevel = CurrentEnviroment;
        _playerSave.SoulAmount = SoulAmount;
        _playerSave.Upgrades = UpgradeController.instance.Upgrades;
        AssetDatabase.SaveAssets();
    }

    public void Load()
    {
        CurrencyAmount = _playerSave.CurrencyAmount;
        CurrentEnviroment = _playerSave.CurrentLevel;
        SoulAmount = _playerSave.SoulAmount;
        UpgradeController.instance.Upgrades = _playerSave.Upgrades;
        StartGame();
     }

    public void CompleteLevel()
    {
        SoulAmount = SoulCollectionController.instance.Souls;
        CurrencyAmount = CurrencyController.Instance.CurrencyAmount;
    }

    public void GameOver()
    {
        SoulAmount = SoulCollectionController.instance.Souls;
        var soulsToRemove = SoulAmount-Mathf.RoundToInt(SoulAmount * SoulCollectionController.instance.SoulRetentionRate);
        SoulCollectionController.instance.RemoveSoul(soulsToRemove);
        UpgradeController.instance.RefreshStats();
        LoadScene("Menu");
        CurrentEnviroment = 0;
        musicManager.StopMusic();
    }

    public void LoadScene(string scene)
    {
        Initiate.Fade(scene, Color.black, 1.0f);
        CurrentEnviroment = 0;
    }

    public void Exit()
    {
        Save();
        Application.Quit();
    }


    public void IncrementLevel()
    {
        Save();
        if (CurrentEnviroment < 2)
        {
            CurrentEnviroment++;
            musicManager.ChangeTrack(CurrentEnviroment);
        }
    }
    
    public void NewGame()
    {
        SoulAmount = 0;
        CurrencyAmount = 0;
        CurrentEnviroment = 0;
        CurrencyAmount = 0;
        CurrencyController.Instance.CurrencyAmount = 0;
        UpgradeController.instance.Upgrades = StartingUpgrades;
        UpgradeController.instance.ResetTiers();
        LoadScene(IntroductionSceneName);
//        UpgradeController.instance.WipeNonPermanantUpgrades();
    }

    public void StartGame()
    {
        LoadScene("Level_1");
        UpgradeController.instance.WipeNonPermanantUpgrades();
        inLevel = true;
    }


    public void Credits()
    {

    }

}
