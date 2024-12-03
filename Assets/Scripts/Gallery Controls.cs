using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Unity.XR.CoreUtils;
using Unity.VisualScripting;

public class GalleryControls : MonoBehaviour
{
    RawImage imageComponent;
    private List<string> imagePaths = new List<string> { };
    private int currentImageIndex = 0;
    bool forceActivate = false;
    Transform GalleryPanel;

    void Start()
    {
        GalleryPanel = GameObject.Find("Gallery Panel").transform;
        imageComponent = GetComponent<RawImage>();
        imageComponent.color = Color.clear;
        GalleryPanel.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TemporarilyActivateGallery()
    {
        StartCoroutine(TemporaryFunction());
    }

    IEnumerator TemporaryFunction()
    {
        GalleryPanel.gameObject.SetActive(true);
        ActivateGallery();
        forceActivate = false;
        yield return new WaitForSeconds(4);
        if (forceActivate == false)
        {
            DeactivateGallery();
            GalleryPanel.gameObject.SetActive(false);
        }
    }

    public void ActivateGallery()
    {
        forceActivate = true;
        // Load images from persistent data path
        string imageFolderPath = Path.Combine(Application.persistentDataPath, "photos");

        DirectoryInfo info = new DirectoryInfo(imageFolderPath);
        FileInfo[] files = info.GetFiles().OrderBy(p => p.CreationTime).ToArray();
        

        foreach (FileInfo file in files)
        {
            imagePaths.Add(file.FullName);
        }

        if (imagePaths.Count == 0)
        {
            Debug.LogError("No images found in: " + imageFolderPath);
            return;
        }

        // Load initial image
        if (imagePaths.Count > 0) {
            imageComponent.color = Color.white;
            LoadImage(imagePaths[imagePaths.Count - 1]);
        }
        else
        {
            imageComponent.color = Color.black;
            imageComponent.texture = null;
        }
    }

    public void DeactivateGallery()
    {
        imageComponent.color = Color.clear;
    }

    private void LoadImage(string imagePath)
    {
        // Load initial image
        byte[] imageData = File.ReadAllBytes(imagePath);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(imageData);
        imageComponent.texture = texture;
    }

    public void NextImage()
    {
        if (currentImageIndex < imagePaths.Count)
        {
            currentImageIndex = currentImageIndex + 1;
            LoadImage(imagePaths[currentImageIndex]);
        }

    }

    public void PreviousImage()
    {
        if (currentImageIndex > 0)
        {
            currentImageIndex = currentImageIndex - 1;
            LoadImage(imagePaths[currentImageIndex]);
        }
    }
}
