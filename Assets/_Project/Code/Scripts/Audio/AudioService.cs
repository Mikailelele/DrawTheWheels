using System.Collections.Generic;
using System.Threading;
using DrawCar.Data;
using DrawCar.ScriptableObjects;
using DrawCar.Utils;
using DrawCar.YandexIntegration;
using UnityEngine;
using VContainer;
using YG;
using Random = UnityEngine.Random;

namespace DrawCar.Audio
{
    public enum EAudioType
    {
        BGM,
        Button,
        Draw,
        Drive,
    }
    
    public interface IAudioService
    {
        bool IsPaused { get; }
        
        void Play(EAudioType audioType);
        void SetPitch(in EAudioType audioType, in float value);
        void PauseAll(bool isPaused, bool mustSave = true);
        void Init();
    }
    
    public sealed class AudioService : MonoBehaviour, IAudioService
    {
        private Dictionary<EAudioType, AudioSource> _audioSources = new();
        
        private CancellationTokenSource _backgroundMusicLoopCts;
        private AudioSource _cachedAudioSource;
        private IAudioSettings _settings;

        private bool _inFocus;
        
        public bool IsPaused { get; set; }
        
        [Inject]
        private void Construct(IAudioSettings audioSettings)
        {
            _settings = audioSettings;
            
            this.LogInjectSuccess();
        }
        
        private void Awake()
        {
            if (YandexGame.SDKEnabled)
            {
                InitSaves();
            }
        }
        
        private void OnEnable() => YandexGame.GetDataEvent += InitSaves;
        
        private void OnDisable()
        {
            YandexGame.GetDataEvent -= InitSaves;
            
            _backgroundMusicLoopCts?.Cancel();
            _backgroundMusicLoopCts?.Dispose();
        }

        private void InitSaves()
        {
            IsPaused = YandexGame.savesData.IsAudioMuted;
        }
        
        public void Init()
        {
            AudioData audioData;
            AudioSource audioSource;
            
            InitSaves();

            for (int i = 0; i < _settings.AudioData.Length; i++)
            {
                audioData = _settings.AudioData[i];

                audioSource = gameObject.AddComponent<AudioSource>();
                
                audioSource.clip = audioData.AudioClip[Random.Range(0, audioData.AudioClip.Length)];
                audioSource.volume = audioData.DefaultVolume;
                audioSource.pitch = audioData.DefaultPitch;
                audioSource.loop = audioData.Loop;
                if(audioData.PlayOnAwake)
                    audioSource.Play();
                
                if (audioData.Type == EAudioType.BGM)
                {
                    _backgroundMusicLoopCts = new CancellationTokenSource();
                }
                
                _audioSources.Add(audioData.Type, audioSource);
            }
            
            PauseAll(YandexGame.savesData.IsAudioMuted, false);
        }
        
        public void Play(EAudioType audioType)
        {
            if (_audioSources.TryGetValue(audioType, out _cachedAudioSource))
            {
                _cachedAudioSource.Play();
            }
        }

        public void SetPitch(in EAudioType audioType, in float value)
        {
            if (_audioSources.TryGetValue(audioType, out _cachedAudioSource))
            {
                _cachedAudioSource.pitch = value;
            }
        }
        
        public void PauseAll(bool isPaused, bool mustSave = true)
        {
            IsPaused = isPaused;

            foreach (var audioSource in _audioSources.Values)
            {
                if (isPaused)
                {
                    audioSource.Pause();
                }
                else
                {
                    audioSource.UnPause();
                }
                
                audioSource.mute = isPaused;
            }
            
            if (mustSave)
                SavingService.SetBool(ESaveType.IsAudioPaused, isPaused);
        }
    }
}