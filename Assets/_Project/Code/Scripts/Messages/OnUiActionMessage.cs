using System;

namespace DrawCar.Ui.Messages
{
    public enum EUiActionType
    {
        OpenDrawCanvas,
        CloseDrawCanvas,
        OpenLeaderboard,
        CloseLeaderboard,
        RestartStage,
        RestartGame,
        Draw,
        MuteAudio,
        SwitchRestartToReplay,
    }
    
    public readonly struct OnUiActionMessage : IEquatable<OnUiActionMessage>
    {
        public EUiActionType Type { get; }
        public bool ByButton { get; }

        public OnUiActionMessage(EUiActionType type, bool byButton = false)
        {
            Type = type;
            ByButton = byButton;
        }

        public bool Equals(OnUiActionMessage other)
        {
            return Type == other.Type && ByButton == other.ByButton;
        }

        public override bool Equals(object obj)
        {
            return obj is OnUiActionMessage other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine((int)Type, ByButton);
        }
    }
}