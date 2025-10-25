using UnityEngine;

namespace PurrPurrCoffee.Abstractions
{
    public interface IAudioController
    {
        void PlayBackgroundTheme(BackgroundTheme backgroundTheme);
        void StopBackgroundTheme();
        void PlayEffect(SoundEffect soundEffect);
    }
    public enum BackgroundTheme
    {
        Relax,
        Fear,
        Chasing,
        Death,
        Subtitles
    }
    public enum SoundEffect
    {
        BloodHit
    }
}
