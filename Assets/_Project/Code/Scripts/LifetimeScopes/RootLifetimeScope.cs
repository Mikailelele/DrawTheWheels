using Code.Scripts.YandexIntegration;
using DrawCar.Ad;
using DrawCar.Car.Input;
using DrawCar.Drawing.Messages;
using DrawCar.Level.Messages;
using DrawCar.ScriptableObjects;
using DrawCar.Ui.Messages;
using MessagePipe;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace DrawCar.LifetimeScopes
{
    public sealed class RootLifetimeScope : LifetimeScope
    {
        [SerializeField] private GameSettingsSo _settings;
        
        private IContainerBuilder _builder;

        protected override void Configure(IContainerBuilder builder)
        {
            _builder = builder;
            
            ConfigureMessagePipe();
            
            RegisterSettingsSo();
            RegisterInputService();
        }

        private void ConfigureMessagePipe()
        {
            var options = _builder.RegisterMessagePipe();

            _builder.RegisterBuildCallback(c => GlobalMessagePipe.SetProvider(c.AsServiceProvider()));
            
            RegisterMessageBrokers(options);
        }

        private void RegisterMessageBrokers(MessagePipeOptions options)
        {
            _builder.RegisterMessageBroker<OnWheelGeneratedMessage>(options);
            _builder.RegisterMessageBroker<OnUiActionMessage>(options);
            _builder.RegisterMessageBroker<OnStageResetMessage>(options);
        }
        
        private void RegisterSettingsSo()
        {
            _builder.RegisterInstance(_settings)
                .AsImplementedInterfaces()
                .AsSelf();
        }
        
        private void RegisterInputService()
        {
            _builder.RegisterEntryPoint<InputService>()
                .As<IInputService>();
        }
    }
}
