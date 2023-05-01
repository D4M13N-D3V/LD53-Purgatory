using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Purgatory.Levels;
namespace Purgatory
{

    public class SceneTransition : MonoBehaviour
    {
        public string SceneName = "Shop_1";
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnCollisionEnter(Collision collision)
        {
            //GameManager.instance.CompleteLevel();
            //GameManager.instance.LoadScene(SceneName);

            GameManager.instance.IncrementLevel();

            this.gameObject.SetActive(false);
        }
    }

}