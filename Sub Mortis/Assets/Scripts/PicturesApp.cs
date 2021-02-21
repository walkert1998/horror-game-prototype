using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PicturesApp : MonoBehaviour
{
    public GameObject imagePreviewPrefab;
    public GameObject imageZoomView;
    public Transform pictureGrid;
    public RawImage selectedPicture;
    public int currentPictureIndex = 0;
    public List<Picture> pictures;
    // Start is called before the first frame update
    void Start()
    {
        imageZoomView.SetActive(false);
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}

    public void AddPicture(Texture2D picture, bool horizontalFocused)
    {
        Picture picObj = new Picture(picture, horizontalFocused);
        pictures.Add(picObj);
        //if (gameObject.activeSelf)
        //{
            Notification.DisplayNewNotification("1 new Picture!", AppNotificationType.Pictures);
        //}
        GameObject newPicture = Instantiate(imagePreviewPrefab, pictureGrid);
        newPicture.GetComponentInChildren<RawImage>().texture = picture;
        if (horizontalFocused)
        {
            newPicture.GetComponentInChildren<RawImage>().rectTransform.sizeDelta = new Vector2(256, 128);
        }
        else
        {
            newPicture.GetComponentInChildren<RawImage>().rectTransform.sizeDelta = new Vector2(128, 256);
        }
        newPicture.GetComponent<Button>().onClick.AddListener(
            () =>
            {
                SelectPicture(picObj);
                currentPictureIndex = pictures.IndexOf(picObj);
                imageZoomView.SetActive(true);
            }
        );
    }

    public void SelectPicture(Picture picture)
    {
        if (picture.horizontal)
        {
            selectedPicture.rectTransform.sizeDelta = new Vector2(1564, 880);
        }
        else
        {
            selectedPicture.rectTransform.sizeDelta = new Vector2(495, 880);
        }
        selectedPicture.texture = picture.image;
        imageZoomView.transform.GetChild(0).localScale = new Vector3(1, 1, 1);
    }

    public void BackToAllPictures()
    {
        imageZoomView.SetActive(false);
        imageZoomView.transform.GetChild(0).localScale = new Vector3(1, 1, 1);
    }

    public void NextPicture()
    {
        if (currentPictureIndex < pictures.Count - 1)
        {
            currentPictureIndex++;
            SelectPicture(pictures[currentPictureIndex]);
        }
    }

    public void PreviousPicture()
    {
        if (currentPictureIndex > 0)
        {
            currentPictureIndex--;
            SelectPicture(pictures[currentPictureIndex]);
        }
    }
}
