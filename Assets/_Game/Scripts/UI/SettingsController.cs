using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Game
{
    public class SettingsController : MonoBehaviour
	{
		[SerializeField] private Slider _fovSlider;
		[SerializeField] private Slider _brightnessSlider;
		[SerializeField] private Toggle _screenShake;
		[SerializeField] private TMP_Dropdown _fullscreen;
		[SerializeField] private TMP_Dropdown _resolution;
		[SerializeField] private Toggle _vSync;
		[SerializeField] private TMP_InputField _frameLimit;
		[SerializeField] private Slider _mouseSensitivity;
		[SerializeField] private TMP_Dropdown _controllerVibration;
		[SerializeField] private Slider _controllerSensitivity;
		[SerializeField] private Slider _controllerDeadzone;
		
		private const string Fov = "Fov";
		private const string Brightness = "Brightness";
		private const string ScreenShake = "ScreenShake";
		private const string Fullscreen = "Fullscreen";
		private const string Resolution = "Resolution";
		private const string VSync = "VSync";
		private const string FrameLimit = "FrameLimit";
		private const string MouseSensitivity = "MouseSensitivity";
		private const string ControllerVibration = "ControllerVibration";
		private const string ControllerSensitivity = "ControllerSensitivity";
		private const string ControllerDeadzone = "ControllerDeadzone";
		
		private void Awake()
		{
			SetFov(TryGetDefault(_fovSlider, Fov));
			SetBrightness(TryGetDefault(_brightnessSlider, Brightness));
			SetScreenShake(TryGetDefault(_screenShake, ScreenShake));
			SetFullscreen(TryGetDefault(_fullscreen, Fullscreen));
			SetResolution(TryGetDefault(_resolution, Resolution));
			SetVSync(TryGetDefault(_vSync, VSync));
			SetFrameLimit(TryGetDefault(_frameLimit, FrameLimit));
			SetMouseSensitivity(TryGetDefault(_mouseSensitivity, MouseSensitivity));
			SetControllerVibration(TryGetDefault(_controllerVibration, ControllerVibration));
			SetControllerSensitivity(TryGetDefault(_controllerSensitivity, ControllerSensitivity));
			SetControllerDeadzone(TryGetDefault(_controllerDeadzone, ControllerDeadzone));
			
			_fovSlider.onValueChanged.AddListener(SetFov);
			_brightnessSlider.onValueChanged.AddListener(SetBrightness);
			_screenShake.onValueChanged.AddListener(SetScreenShake);
			_fullscreen.onValueChanged.AddListener(SetFullscreen);
			_resolution.onValueChanged.AddListener(SetResolution);
			_vSync.onValueChanged.AddListener(SetVSync);
			_frameLimit.onValueChanged.AddListener(SetFrameLimit);
			_mouseSensitivity.onValueChanged.AddListener(SetMouseSensitivity);
			_controllerVibration.onValueChanged.AddListener(SetControllerVibration);
			_controllerSensitivity.onValueChanged.AddListener(SetControllerSensitivity);
			_controllerDeadzone.onValueChanged.AddListener(SetControllerDeadzone);
		}
		
		private static float TryGetDefault(Slider s, string key) => PlayerPrefs.HasKey(key) ? PlayerPrefs.GetFloat(key) : s.value;
		private static int TryGetDefault(TMP_Dropdown d, string key) => PlayerPrefs.HasKey(key) ? PlayerPrefs.GetInt(key) : d.value;
		private static int TryGetDefault(TMP_InputField i, string key) => PlayerPrefs.HasKey(key) ? PlayerPrefs.GetInt(key) : int.TryParse(i.text, out int v) ? v : 0;
		private static bool TryGetDefault(Toggle t, string key) => PlayerPrefs.HasKey(key) ? PlayerPrefs.GetInt(key) > 0 : t.isOn;
		
		private void SetFov(float value)
		{
			_fovSlider.value = value;
			PlayerPrefs.SetFloat(Fov, value);
			// TODO: Set Fov
		}
		
		private void SetBrightness(float value)
		{
			_brightnessSlider.value = value;
			PlayerPrefs.SetFloat(Brightness, value);
			// TODO: Set Brightness
		}
		
		private void SetScreenShake(bool value)
		{
			_screenShake.isOn = value;
			PlayerPrefs.SetInt(ScreenShake, value ? 1 : 0);
			// TODO: Set ScreenShake
		}
		
		private void SetFullscreen(int value)
		{
			_fullscreen.value = value;
			PlayerPrefs.SetInt(Fullscreen, value);
			// TODO: Set Fullscreen Mode
		}
		
		private void SetResolution(int value)
		{
			_resolution.value = value;
			PlayerPrefs.SetInt(Resolution, value);
			// TODO: Set Resolution
		}
		
		private void SetVSync(bool value)
		{
			_vSync.isOn = value;
			PlayerPrefs.SetInt(VSync, value ? 1 : 0);
			// TODO: Set VSync
		}
		
		private void SetFrameLimit(string value)
		{
			if (int.TryParse(value, out int v)) SetFrameLimit(v);
		}
		private void SetFrameLimit(int value)
		{
			_frameLimit.text = value.ToString();
			PlayerPrefs.SetInt(FrameLimit, value);
			// TODO: Set FrameLimit
		}
		
		private void SetMouseSensitivity(float value)
		{
			_mouseSensitivity.value = value;
			PlayerPrefs.SetFloat(MouseSensitivity, value);
			// TODO: Set MouseSensitivity
		}
		
		private void SetControllerVibration(int value)
		{
			_controllerVibration.value = value;
			PlayerPrefs.SetInt(ControllerVibration, value);
			// TODO: Set ControllerVibration
		}
		
		private void SetControllerSensitivity(float value)
		{
			_controllerSensitivity.value = value;
			PlayerPrefs.SetFloat(ControllerSensitivity, value);
			// TODO: Set ControllerSensitivity
		}
		
		private void SetControllerDeadzone(float value)
		{
			_controllerDeadzone.value = value;
			PlayerPrefs.SetFloat(ControllerDeadzone, value);
			// TODO: Set ControllerDeadzone
		}
    }
}
