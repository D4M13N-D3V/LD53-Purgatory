using Purgatory.Impl;
using System.Collections;
using UnityEngine;

namespace Purgatory
{
    public class DockSoul : Soul
    {
		public override void CleanupAfterCollection()
        {
            Debug.Log("Dock soul has been collected.");
            Destroy(gameObject);
        }
	}
}