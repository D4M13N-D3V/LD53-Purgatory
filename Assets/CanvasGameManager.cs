using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasGameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        GameManager.instance.StartGame();
    }

    public void NewGame()
    {
        GameManager.instance.NewGame();
    }
}
