using System.Collections;
using UnityEngine;

namespace Purgatory.Interfaces
{
    public interface IMovementBehaviour
    {
        float Speed { get; }
        float TurnSpeed { get; }
    }
}