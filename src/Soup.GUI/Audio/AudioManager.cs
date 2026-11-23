using Soup.Core;
using Soup.Core.API;
using NAudio.SoundFile;
using NAudio.Wave;
using NAudio.Wave.Alsa;
using System;

namespace Soup.GUI.Audio;

public class AudioManager
{
    private Core.Soup soup;
    private SoundFileReader audioFile = null;
    private IWavePlayer outputDevice = null;
    public AudioManager(Core.Soup soup)
    {
        this.soup = soup;

        soup.Player.Cleanup += Dispose;
        soup.Player.LoadNew += LoadNew;
        soup.Player.PlayStateChanged += PlayStateChanged;
    }

    private void PlayStateChanged()
    {
        if (outputDevice == null) return;

        if (soup.Player.PlayState == PlayState.Playing && outputDevice.PlaybackState == PlaybackState.Paused)
        {
            outputDevice.Play();
        } 
        else if (soup.Player.PlayState == PlayState.Paused && outputDevice.PlaybackState == PlaybackState.Playing)
        {
            outputDevice.Pause();
        }
    }

    private void LoadNew(Song song)
    {
        audioFile = new SoundFileReader(song.AudioPath);
        outputDevice = GetOutputDevice();

        outputDevice.Init(audioFile);

        outputDevice.PlaybackStopped += SongOver;

        soup.Player.SongState = SongState.Loaded;
        outputDevice.Play();
        soup.Player.PlayState = PlayState.Playing;
    }

    private void SongOver(object s, StoppedEventArgs e)
    {
        Console.WriteLine("song finished!");
        soup.Player.SongOver();
    }

    public void Dispose()
    {
        Console.WriteLine("AudioManager Disposing");
        if (outputDevice != null)
        {
            void disposeDevice()
            {
                Console.WriteLine("playback stopped in dispose");
                outputDevice?.Dispose();
                outputDevice = null;

                audioFile?.Dispose();
                audioFile = null;

                soup.Player.ReadyForLoad();
            }

            if (outputDevice.PlaybackState == PlaybackState.Stopped)
            {
                disposeDevice();
            } else
            {
                outputDevice.PlaybackStopped -= SongOver;
                outputDevice.PlaybackStopped += (s, e) => { disposeDevice(); };
                outputDevice.Stop();
            }
        } else
        {
            audioFile?.Dispose();
            audioFile = null;

            soup.Player.ReadyForLoad();
        }
    }

#pragma warning disable CA1416 // Validate platform compatibility
    private IWavePlayer GetOutputDevice()
    {
    #if LINUX
        return new AlsaOut();
    #elif WINDOWS
        return new WasapiPlayerBuilder().Build();
    #elif MACOS
        return new CoreAudioPlayer();
    #endif
    }
#pragma warning restore CA1416 // Validate platform compatibility
}