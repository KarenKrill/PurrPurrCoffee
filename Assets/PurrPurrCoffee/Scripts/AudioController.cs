using PurrPurrCoffee.Abstractions;
using UnityEngine;

namespace PurrPurrCoffee
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioController : MonoBehaviour, IAudioController
    {
        public void PlayBackgroundTheme(BackgroundTheme backgroundTheme)
        {
            if (_audioSource.isPlaying)
            {
                _audioSource.Stop();
            }
            switch (backgroundTheme)
            {
                case BackgroundTheme.Relax:
                    _audioSource.clip = _relaxTheme;
                    break;
                case BackgroundTheme.Fear:
                    _audioSource.clip = _fearTheme;
                    break;
                case BackgroundTheme.Chasing:
                    _audioSource.clip = _chasingTheme;
                    break;
                case BackgroundTheme.Death:
                    _audioSource.clip = _deathTheme;
                    break;
                case BackgroundTheme.Subtitles:
                    _audioSource.clip = _subtitilesTheme;
                    break;
                default:
                    break;
            }
            _audioSource.Play();
        }
        public void StopBackgroundTheme()
        {
            _audioSource.Stop();
        }
        public void PlayEffect(SoundEffect soundEffect)
        {
            switch(soundEffect)
            {
                case SoundEffect.BloodHit:
                    if (_fleshHit == null)
                    {
                        Debug.LogWarning("Flesh hit sound effect is not assigned.");
                    }
                    else
                    {
                        _audioSource.PlayOneShot(_fleshHit);
                    }
                    break;
                default:
                    Debug.LogWarning("Sound effect not recognized.");
                    break;
            }
            
        }

        [SerializeField]
        private AudioClip _relaxTheme;
        [SerializeField]
        private AudioClip _fearTheme;
        [SerializeField]
        private AudioClip _chasingTheme;
        [SerializeField]
        private AudioClip _deathTheme;
        [SerializeField]
        private AudioClip _subtitilesTheme;
        [SerializeField]
        private AudioClip _fleshHit;
        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }
    }
}
