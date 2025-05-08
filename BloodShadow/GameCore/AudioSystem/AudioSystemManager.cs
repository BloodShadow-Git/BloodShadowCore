using System;

namespace BloodShadow.GameCore.AudioSystem
{
    public abstract class AudioSystemManager<TSource>
    {
        public abstract event Action OnAudioStarted;
        public abstract event Action OnAudioStartStopping;
        public abstract event Action OnAudioStopped;
        protected abstract TSource AudioSource { get; }
        public abstract void Play();
        public abstract void Play(uint soundID);
        public abstract void Play(uint playlistID, uint soundID);
        public abstract void PlayRandom();
        public abstract void PlayRandom(uint playlistID);
        public abstract void PlaylistPlay();
        public abstract void PlaylistPlay(uint playlistID);
        public abstract void PlaylistPlayCycle();
        public abstract void PlaylistPlayCycle(uint playlistID);
        public abstract void PlaylistPlayRandom();
        public abstract void PlaylistPlayRandom(uint playlistID);
        public abstract void Stop();
    }
}
