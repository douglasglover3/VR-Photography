using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class CameraControls : MonoBehaviour
{
    public static Camera cmra;
    Texture2D photo;
    XRGrabInteractable interactable;
    XRBaseInputInteractor interactor;
    Transform tfm;
    Transform pocket_tfm;
    Transform player_camera_tfm;
    bool camera_pocketed = false;

    // Start is called before the first frame update
    void Start()
    {
        cmra = GetComponent<Camera>();
        tfm = GetComponent<Transform>().parent;
        pocket_tfm = GameObject.Find("Pocket").transform;
        player_camera_tfm = pocket_tfm.parent.Find("Camera Offset").transform.Find("Main Camera").transform;
        interactable = GetComponentInParent<XRGrabInteractable>();
    }

    void Update()
    {
        if (camera_pocketed)
        {
            pocket_tfm.localRotation = Quaternion.Euler(0, player_camera_tfm.localRotation.eulerAngles.y, 0);
            tfm.localPosition = new Vector3(0, 0, 0.2f);
        }
    }

    public void PickUpCamera()
    {
        tfm.parent = null;
        tfm.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        camera_pocketed = false;
    }

    public void LetGoOfCamera()
    {
        tfm.parent = pocket_tfm;
        tfm.localPosition = new Vector3(0, 0, 0);
        tfm.localRotation = new Quaternion(0, 0, 0, 0);
        tfm.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        camera_pocketed = true;
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
