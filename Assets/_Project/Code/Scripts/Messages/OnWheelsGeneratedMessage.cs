using System;
using UnityEngine;

namespace DrawCar.Drawing.Messages
{
    public readonly struct OnWheelGeneratedMessage : IEquatable<OnWheelGeneratedMessage>
    {
        public GameObject[] GeneratedWheels { get; }

        public OnWheelGeneratedMessage(GameObject[] generatedWheels)
        {
            GeneratedWheels = generatedWheels;
        }

        public bool Equals(OnWheelGeneratedMessage other)
        {
            return Equals(GeneratedWheels, other.GeneratedWheels);
        }

        public override bool Equals(object obj)
        {
            return obj is OnWheelGeneratedMessage other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (GeneratedWheels != null ? GeneratedWheels.GetHashCode() : 0);
        }
    }
}