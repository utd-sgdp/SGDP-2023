using System;
using System.Collections;
using System.Collections.Generic;
using Game.SoundSystem;
using UnityEngine;

namespace Game
{
    public class Demo : MonoBehaviour
    {
        
        [SerializeField] private SfxReference _audio;
        [SerializeField] private MusicTrack _music;

        private void Start()
        {
            _audio.Play();
            _audio.PlayAtPosition(transform.position);
            _audio.PlayAtParentAndFollow(transform);

            SoundManager.PlaySfx(_audio);
            
            _music.Play();
            _music.Queue();
            SoundManager.PlayMusicNow(_music);
            SoundManager.QueueMusic(_music);
        }
    }
}
