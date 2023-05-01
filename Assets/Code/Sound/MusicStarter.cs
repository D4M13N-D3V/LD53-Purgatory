using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicStarter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.musicManager.ChangeTrack(0);   
        GameObject.Destroy(this.gameObject);
    }
}
