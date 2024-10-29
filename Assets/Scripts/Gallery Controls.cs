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
    private RawImage imageComponent;
    private List<string> imagePaths = new List<string> { };
    private int currentImageIndex = 0;


    void Start()
    {
        imageComponent = GetComponent<RawImage>();
        imageComponent.color = Color.clear;
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
        ActivateGallery();
        yield return new WaitForSeconds(4);
        DeactivateGallery();
    }

    public void ActivateGallery()
    {
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

    private void NextImage()
    {
        if (currentImageIndex < imagePaths.Count)
        {
            currentImageIndex = currentImageIndex + 1;
            LoadImage(imagePaths[currentImageIndex]);
        }

    }

    private void PreviousImage()
    {
        if (currentImageIndex > 0)
        {
            currentImageIndex = currentImageIndex - 1;
            LoadImage(imagePaths[currentImageIndex]);
        }
    }
}
