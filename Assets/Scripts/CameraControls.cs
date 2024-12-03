using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Rendering;
using UnityEngine.UI;
using System.Linq;
using Unity.XR.CoreUtils;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit;

public class CameraControls : MonoBehaviour
{
    public static Camera cmra;
    Texture2D photo;
    XRGrabInteractable interactable;
    XRBaseInputInteractor interactor;

    // Start is called before the first frame update
    void Start()
    {
        cmra = GetComponent<Camera>();
        interactable = GetComponentInParent<XRGrabInteractable>();
    }

    public void PickUpCamera()
    {
        interactor = interactable.interactorsSelecting[0] as XRBaseInputInteractor;
    }

    public void LetGoOfCamera()
    {
        
    }

    public void TakePhoto()
    {
        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var stringChars = new char[8];
        var random = new System.Random();

        for (int i = 0; i < stringChars.Length; i++)
        {
            stringChars[i] = chars[random.Next(chars.Length)];
        }

        var filename_suffix = new String(stringChars);

        RenderTexture rTex = cmra.activeTexture;
        photo = new Texture2D(rTex.width, rTex.height, TextureFormat.RGB24, false);

        var old_rt = RenderTexture.active;
        RenderTexture.active = rTex;
        photo.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        photo.Apply();
        RenderTexture.active = old_rt;

        if (interactable.isSelected)
        {
            interactor.SendHapticImpulse(0.2f, 0.2f);
        }

        byte[] bytes = photo.EncodeToPNG();
        Directory.CreateDirectory(Application.persistentDataPath + "/photos");
        File.WriteAllBytes(Application.persistentDataPath + "/photos/" + "photo_" + filename_suffix + ".png", bytes);
    }
}
