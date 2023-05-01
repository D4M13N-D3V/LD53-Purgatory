using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviromentTransitionTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void OnTriggerEnter(Collider collider) 
    {
        if (collider.gameObject.tag == "Player")
        {
            GameManager.instance.IncrementLevel();
            this.gameObject.SetActive(false);
        }
    }
}
