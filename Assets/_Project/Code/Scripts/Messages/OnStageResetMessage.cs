using System;
using UnityEngine;

namespace DrawCar.Level.Messages
{
    public readonly struct OnStageResetMessage : IEquatable<OnStageResetMessage>
    { 
        public Vector3 StagePosition { get; }
        
        public OnStageResetMessage(Vector3 stagePosition)
        {
            StagePosition = stagePosition;
        }

        public bool Equals(OnStageResetMessage other)
        {
            return StagePosition.Equals(other.StagePosition);
        }

        public override bool Equals(object obj)
        {
            return obj is OnStageResetMessage other && Equals(other);
        }

        public override int GetHashCode()
        {
            return StagePosition.GetHashCode();
        }
    }
}