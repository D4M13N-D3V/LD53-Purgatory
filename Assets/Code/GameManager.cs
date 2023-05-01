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

public class GameManager : MonoBehaviour
{
    public EnumGameState GameState = EnumGameState.MENU;
    public List<Purgatory.Upgrades.UpgradeSciptableObject> StartingUpgrades = new List<UpgradeSciptableObject>();
    private PlayerSaveScriptableObject _playerSave;

    public static GameManager instance;

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
        UpgradeController.instance.RefreshStats();
    }

    private IEnumerator AutoSave()
    {
        _playerSave.Upgrades = UpgradeController.instance.Upgrades;
        _playerSave.Souls = SoulCollectionController.instance.Souls;
        AssetDatabase.SaveAssets();
        yield return new WaitForSeconds(5f);
        StartCoroutine(AutoSave());
    }

    public void SaveGame()
    {
        _playerSave.Upgrades = UpgradeController.instance.Upgrades;
        _playerSave.Souls = SoulCollectionController.instance.Souls;
        AssetDatabase.SaveAssets();
    }

    public void Exit()
    {
        if(GameState!=EnumGameState.MENU)
            SaveGame();
        Application.Quit();
    }

    public void NewGame()
    {
        _playerSave.Upgrades = StartingUpgrades;
        _playerSave.Souls = 0;
        ContinueGame();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Intro_Dialogue", LoadSceneMode.Single);
    }

    public void ContinueGame()
    {
        UpgradeController.instance.Upgrades = _playerSave.Upgrades;
        StartGame();
    }

    public void Credits()
    {

    }

}
