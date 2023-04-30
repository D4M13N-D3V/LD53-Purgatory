using Purgatory.Impl;
using System.Collections;
using UnityEngine;

namespace Purgatory
{
    public class DockSoul : Soul
    {
        public DockSoul()
        {
        }

        public override void Collected()
        {
            Debug.Log("Dock soul has been collected.");
            Destroy(gameObject);
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}