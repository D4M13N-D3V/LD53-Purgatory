using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Purgatory.Interfaces
{
    public interface IProjectile
    {
        int Damage { get; }
        float Speed { get; }
        void Impact();
        void Launch();
    }
}