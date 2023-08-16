using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace MR
{
    public enum SoundId
    {
        CellClickGood, CellClickBad, WinScreenOpen, LoseScreenOpen
    }

    public class AudioController : IStartable
    {
        const string SoundEnabledKey = "sound_enabled_key";

        [Inject] SceneData _sceneData;

        [Inject] GameConfig _gameConfig;

        readonly Dictionary<SoundId, List<AudioClip>> _soundMap = new ();

        void IStartable.Start()
        {
            InitSoundMap();
            SetSoundEnabled(IsSoundEnabled());
        }

        public void SetSoundEnabled(bool enabled)
        {
            PlayerPrefs.SetInt(SoundEnabledKey, enabled ? 1 : 0);
            _sceneData.SoundSource.mute = !enabled;
        }

        public bool IsSoundEnabled()
        {
            return PlayerPrefs.GetInt(SoundEnabledKey, 1) == 1;
        }

        public void Play(SoundId soundId)
        {
            var sounds = _soundMap[soundId];
            _sceneData.SoundSource.PlayOneShot(sounds[Random.Range(0, sounds.Count)]);
        }

        void InitSoundMap()
        {
            foreach (var mapping in _gameConfig.AudioMappings)
            {
                if (_soundMap.TryGetValue(mapping.soundId, out var list))
                {
                    list.Add(mapping.audioClip);
                } else {
                    _soundMap[mapping.soundId] = new List<AudioClip>
                    {
                        mapping.audioClip
                    };
                }
            }
        }
    }
}
