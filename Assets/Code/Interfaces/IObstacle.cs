using System.Collections;
using UnityEngine;

namespace Purgatory.Interfaces
{
    public interface IObstacle
    {
        public int Damage { get; }
        public void Impact();
    }
}