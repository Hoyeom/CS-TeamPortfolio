
    using System;
    using System.Collections.Generic;
    using Runtime.Utils;
    using UnityEngine;

    public class SoundManager
    {
        private AudioSource[] _audioSources = new AudioSource[Enum.GetValues(typeof(Define.Sound)).Length];

        private Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();

        public void Initialize()
        {
            GameObject root = GameObject.Find("@Sounds");
            if (root == null)
            {
                root = new GameObject($"@Sound");
                root.transform.parent = GameObject.Find(Managers.DEFAULT_NAME).transform;
                
                string[] soundName = Enum.GetNames(typeof(Define.Sound));
                for (int i = 0; i < soundName.Length; i++)
                {
                    GameObject go = new GameObject($"{soundName[i]}");
                    _audioSources[i] = go.GetOrAddComponent<AudioSource>();
                    go.transform.parent = root.transform;
                }
                
                _audioSources[(int) Define.Sound.Bgm].loop = true;
            }
        }

        public void Clear()
        {
            foreach (AudioSource audioSource in _audioSources)
            {
                audioSource.clip = null;
                audioSource.Stop();
            }
            
            _audioClips.Clear();
        }

        public void Play(string path, Define.Sound type = Define.Sound.Effect, float pitch = 1.0f)
        {
            AudioClip audioClip = GetOrAddAudioClip(path, type);
            Play(audioClip, type, pitch);
        }

        public void Play(AudioClip audioClip, Define.Sound type = Define.Sound.Effect, float pitch = 1.0f)
        {
            if(audioClip == null)
                return;

            AudioSource audioSource = _audioSources[(int) type];
            if (type == Define.Sound.Bgm)
            {
                if(audioSource.isPlaying)
                    audioSource.Stop();

                audioSource.pitch = pitch;
                audioSource.clip = audioClip;
                audioSource.Play();
            }
            else
            {
                audioSource.pitch = pitch;
                audioSource.PlayOneShot(audioClip);
            }
        }

        AudioClip GetOrAddAudioClip(string path, Define.Sound type = Define.Sound.Effect)
        {
            if (path.Contains("Sounds/") == false)
                path = $"Sounds/{path}";
            AudioClip audioClip = null;

            if (type == Define.Sound.Bgm)
                audioClip = Managers.Resource.Load<AudioClip>(path);
            else if(_audioClips.TryGetValue(path,out audioClip) == false)
            {
                audioClip = Managers.Resource.Load<AudioClip>(path);
                _audioClips.Add(path, audioClip);
            }

            if (audioClip == null)
                Debug.Log($"AudioClip Missing [PATH: {path}]");

            return audioClip;
        }

    }
