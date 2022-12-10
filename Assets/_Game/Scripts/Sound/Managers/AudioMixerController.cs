using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Game.SoundSystem
{
    public class AudioMixerController : MonoBehaviour
    {
	    [SerializeField] private AudioMixer _mixer = null;
	    [SerializeField] private Slider _masterSlider;
	    [SerializeField] private Slider _musicSlider;
	    [SerializeField] private Slider _ambientSlider;
	    [SerializeField] private Slider _sfxSlider;
	    
	    private const string _masterVolume = "MasterVolume";
	    private const string _musicVolume = "MusicVolume";
	    private const string _ambientVolume = "AmbientVolume";
	    private const string _sfxVolume = "SfxVolume";
        
	    private void Awake()
	    {
	    	if (PlayerPrefs.HasKey(_masterVolume)) _masterSlider.value = PlayerPrefs.GetFloat(_masterVolume);
	    	if (PlayerPrefs.HasKey(_musicVolume)) _musicSlider.value = PlayerPrefs.GetFloat(_musicVolume);
	    	if (PlayerPrefs.HasKey(_ambientVolume)) _ambientSlider.value = PlayerPrefs.GetFloat(_ambientVolume);
	    	if (PlayerPrefs.HasKey(_sfxVolume)) _sfxSlider.value = PlayerPrefs.GetFloat(_sfxVolume);
	    	SetMasterVolume(_masterSlider.value);
	    	SetMusicVolume(_musicSlider.value);
	    	SetAmbientVolume(_ambientSlider.value);
	    	SetSfxVolume(_sfxSlider.value);
	    	
	    	_masterSlider.onValueChanged.AddListener(SetMasterVolume);
	    	_musicSlider.onValueChanged.AddListener(SetMusicVolume);
	    	_ambientSlider.onValueChanged.AddListener(SetAmbientVolume);
	    	_sfxSlider.onValueChanged.AddListener(SetSfxVolume);
	    }

	    public void SetMasterVolume(float volume)
	    {
		    _mixer.SetFloat(_masterVolume, ConvertVolume(volume));
		    PlayerPrefs.SetFloat(_masterVolume, volume);
	    }

        public void SetMusicVolume(float volume)
        {
	        _mixer.SetFloat(_musicVolume, ConvertVolume(volume));
	        PlayerPrefs.SetFloat(_musicVolume, volume);
        }
        
	    public void SetAmbientVolume(float volume)
	    {
		    _mixer.SetFloat(_ambientVolume, ConvertVolume(volume));
		    PlayerPrefs.SetFloat(_ambientVolume, volume);
	    }

        public void SetSfxVolume(float volume)
        {
	        _mixer.SetFloat(_sfxVolume, ConvertVolume(volume));
	        PlayerPrefs.SetFloat(_sfxVolume, volume);
        }
        
	    private static float ConvertVolume(float volume) => volume > 0 ? Mathf.Log10(volume) * 20 : -80;
    }
}