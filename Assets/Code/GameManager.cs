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
    
    public List<Purgatory.Upgrades.UpgradeSciptableObject> StartingUpgrades = new List<UpgradeSciptableObject>();

    public static GameManager instance;

    public MusicManager musicManager;
    private bool fading = false;
    internal List<ProjectileModifier> AvailableModifiers;

    private LevelController levelController;

    private bool inLevel = false;

    public GameManager()
    {
        if (instance == null)
            instance = this;
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start(){
        musicManager.PlayShopMusic();
    }
    public void SetGameState(EnumGameState state)
    {
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
        LoadScene("Death_Shop");
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
            Application.Quit();
    }


    public void IncrementLevel()
    {
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
        UpgradeController.instance.Upgrades = StartingUpgrades;
        LoadScene(IntroductionSceneName);
//        UpgradeController.instance.WipeNonPermanantUpgrades();
    }

    public void NextRun()
    {
        CurrentEnviroment = 0;   
        LoadScene(IntroductionSceneName);
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
