using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Purgatory.Interfaces
{
    public interface IProjectile
    {
        int Damage { get; set; }
        float Speed { get; set; }
        void Impact();
        void Launch();
    }
}