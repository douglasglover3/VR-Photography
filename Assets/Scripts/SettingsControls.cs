using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class SettingsControls : MonoBehaviour
{
    Transform SettingsPanel;
    Transform GalleryPanel;
    string[] ShutterSpeedValues = { "1/2", "1/4", "1/8", "1/15", "1/30", "1/60", "1/125", "1/250", "1/500", "1/1000", "1/2000", "1/4000", "1/8000" };
    float[] ShutterSpeedNumbers = {1, 1, 1, 0.8f, 0.6f, 0.4f, 0.2f, 0, 0, 0, 0, 0, 0};
    float[] ShutterSpeedExposure = { 2, 1, 0.5f, 0.25f, 0.1f, 0.05f, 0, -0.05f, -0.1f, -0.25f, -0.5f, -1, -2};
    string[] ApertureValues = { "f/1.4", "f/2.8", "f/5.6", "f/8", "f/11", "f/22"};
    float[] ApertureNumbers = { 1.4f, 2.8f, 5.6f, 8, 11, 22};
    float[] ApertureExposure = { -2, -1, -0.5f, 0, 0.5f, 1};
    string[] ISOValues = { "100", "200", "400", "800", "1600", "3000" };
    float[] ISONumbers = {0.0f, 0.0f, 0.0f, 0.3f, 0.6f, 1.0f };
    float[] ISOExposure = { -2, -1, -0.5f, 0, 0.5f, 1 };
    int CurrentShutterSpeed = 6;
    int CurrentAperture = 3;
    int CurrentISO = 3;
    Volume GlobalVolume;
    DepthOfField DoF;
    MotionBlur Motionblur;
    FilmGrain Grain;
    ColorAdjustments Color;

    // Start is called before the first frame update
    void Start()
    {
        SettingsPanel = GetComponent<Transform>();
        GalleryPanel = GameObject.Find("Gallery Panel").transform;
        GlobalVolume = GameObject.Find("Global Volume").GetComponent<Volume>();
        GlobalVolume.profile.TryGet<DepthOfField>(out DoF);
        GlobalVolume.profile.TryGet<MotionBlur>(out Motionblur);
        GlobalVolume.profile.TryGet<FilmGrain>(out Grain);
        GlobalVolume.profile.TryGet<ColorAdjustments>(out Color);
    }

    public void OnChangeShutterSpeed()
    {
        Transform ShutterSpeed = SettingsPanel.Find("Shutter Speed");
        Slider slider = ShutterSpeed.GetComponentInChildren<Slider>();
        Text text = ShutterSpeed.GetComponentInChildren<Text>();
        text.text = ShutterSpeedValues[(int)slider.value];
        CurrentShutterSpeed = (int)slider.value;
        Color.postExposure.value = ShutterSpeedExposure[CurrentShutterSpeed] + ApertureExposure[CurrentAperture] + ISOExposure[CurrentISO];
    }

    public void OnChangeAperture()
    {
        Transform Aperture = SettingsPanel.Find("Aperture");
        Slider slider = Aperture.GetComponentInChildren<Slider>();
        Text text = Aperture.GetComponentInChildren<Text>();
        text.text = ApertureValues[(int)slider.value];
        DoF.aperture.value = ApertureNumbers[(int)slider.value];
        CurrentAperture = (int)slider.value;
        Color.postExposure.value = ShutterSpeedExposure[CurrentShutterSpeed] + ApertureExposure[CurrentAperture] + ISOExposure[CurrentISO];
    }

    public void OnChangeFocus()
    {
        DoF.focusDistance.value = 1;
    }

    public void OnChangeISO()
    {
        Transform ISO = SettingsPanel.Find("ISO");
        Slider slider = ISO.GetComponentInChildren<Slider>();
        Text text = ISO.GetComponentInChildren<Text>();
        text.text = ISOValues[(int)slider.value];
        Grain.intensity.value = ISONumbers[(int)slider.value];
        CurrentISO = (int)slider.value;
        Color.postExposure.value = ShutterSpeedExposure[CurrentShutterSpeed] + ApertureExposure[CurrentAperture] + ISOExposure[CurrentISO];
    }

    public void PressBack()
    {
        GalleryPanel.gameObject.SetActive(false);
        SettingsPanel.gameObject.SetActive(false);
    }

    public void PressGallery()
    {
        GalleryPanel.gameObject.SetActive(true);
        SettingsPanel.gameObject.SetActive(false);
    }

    public void PressSettings()
    {
        GalleryPanel.gameObject.SetActive(false);
        SettingsPanel.gameObject.SetActive(true);
    }
}
