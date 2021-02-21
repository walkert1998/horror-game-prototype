using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhoneCamera : MonoBehaviour
{
    public int resolutionWidth = 1920;
    public int resolutionHeight = 1080;
    public bool cameraActive = false;
    public bool takePicture = false;
    public bool scanning = false;
    public Camera myCamera;
    public Quaternion originalRotation;
    public Phone phone;
    public PicturesApp picturesApp;
    public RenderTexture horizontalTexture;
    public RenderTexture verticalTexture;
    public Slider zoomSlider;
    public Slider scanSlider;
    public GameObject scanHighlight;
    public QRCode currentCode;
    public float scanPercent;
    // Start is called before the first frame update
    void Start()
    {
        resolutionWidth = myCamera.targetTexture.width;
        resolutionHeight = myCamera.targetTexture.height;
        zoomSlider.value = 0;
        scanSlider.value = 0;
        scanHighlight.SetActive(false);
        scanSlider.gameObject.SetActive(false);
        scanPercent = 0;
        //DeactivateCamera();
        cameraActive = false;
        myCamera.enabled = false;
        myCamera.fieldOfView = 80;
        StopScan();
        //originalRotation = myCamera.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (cameraActive)
        {
            if (Input.GetMouseButtonDown(0) && !scanning)
            {
                takePicture = true;
            }
            else if (scanning)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    StopScan();
                }
                else if (Input.GetMouseButton(0))
                {
                    Scan();
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    StopScan();
                }
            }
            if (Input.GetAxis("Mouse ScrollWheel") > 0 && myCamera.fieldOfView > 30)
            {
                myCamera.fieldOfView -= 5;
                zoomSlider.value++;
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0 && myCamera.fieldOfView < 80)
            {
                myCamera.fieldOfView += 5;
                zoomSlider.value--;
            }
        }
    }

    void LateUpdate()
    {
        if (takePicture)
        {
            Texture2D picture = new Texture2D(resolutionWidth, resolutionHeight, TextureFormat.RGB24, false);
            myCamera.Render();
            RenderTexture.active = myCamera.targetTexture;
            picture.ReadPixels(new Rect(0, 0, resolutionWidth, resolutionHeight), 0, 0);
            if (phone.focusState == PhoneFocusState.HorizontalFocused)
            {
                picture = rotate(picture);
            }
            else
            {
                picture = rotate(picture);
                picture = rotate(picture);
                picture = rotate(picture);
                picture = rotate(picture);
            }
            picturesApp.AddPicture(picture, phone.focusState == PhoneFocusState.HorizontalFocused);
            //byte[] bytes = picture.EncodeToPNG();
            //string fileName = ScreenShotName(resolutionWidth, resolutionHeight);
            //System.IO.File.WriteAllBytes(fileName, bytes);
            Debug.Log("Picture taken");
            takePicture = false;
        }
    }

    public Texture2D rotate(Texture2D t)
    {
        Texture2D newTexture = new Texture2D(t.height, t.width, t.format, false);

        for (int i = 0; i < t.width; i++)
        {
            for (int j = 0; j < t.height; j++)
            {
                newTexture.SetPixel(j, i, t.GetPixel(i, t.height - j + 1));
            }
        }
        newTexture.Apply();
        return newTexture;
    }

    public void TakePicture()
    {
        Texture2D picture = new Texture2D(resolutionWidth, resolutionHeight, TextureFormat.RGB24, false);
        myCamera.Render();
        RenderTexture.active = myCamera.targetTexture;
        picture.ReadPixels(new Rect(0, 0, resolutionWidth, resolutionHeight), 0, 0);
        byte[] bytes = picture.EncodeToPNG();
        string fileName = ScreenShotName(resolutionWidth, resolutionHeight);
        //System.IO.FileInfo file = new System.IO.FileInfo(fileName);
        //file.Directory.Create();
        System.IO.File.WriteAllBytes(fileName, bytes);
        Debug.Log("Picture taken");
    }

    public static string ScreenShotName(int width, int height)
    {
        return string.Format("{0}/Resources/Pictures/screen_{1}x{2}_{3}.png",
                             Application.dataPath,
                             width, height,
                             System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
    }

    public void ActivateCamera()
    {
        cameraActive = true;
        myCamera.enabled = true;
        DynamicCursor.ChangeCursor_Static(CursorType.None);
        DynamicCursor.HideCursor_Static();
        PlayerInteraction.UnlockInteraction();
    }

    public void DeactivateCamera()
    {
        PlayerInteraction.LockInteraction();
        DynamicCursor.ChangeCursor_Static(CursorType.None);
        cameraActive = false;
        myCamera.enabled = false;
        myCamera.fieldOfView = 80;
        zoomSlider.value = 0;
        StopScan();
    }

    public void StartScanning(QRCode code)
    {
        scanning = true;
        currentCode = code;
        Debug.Log("Starting scan");
        scanHighlight.SetActive(true);
    }

    public void Scan()
    {
        scanSlider.gameObject.SetActive(true);
        scanPercent += Time.deltaTime;
        scanSlider.value = scanPercent;
        if (scanPercent >= scanSlider.maxValue)
        {
            currentCode.scanned = true;
            if (currentCode.qrNote != null)
            {
                phone.GetComponent<NotesApp>().AddNote(currentCode.qrNote);
            }
            if (currentCode.qrPicture != null)
            {
                picturesApp.AddPicture(currentCode.qrPicture, true);
            }
            StopScan();
        }
    }

    public void StopScan()
    {
        //Debug.Log("Stop scan");
        scanning = false;
        scanPercent = 0.0f;
        scanSlider.value = 0;
        currentCode = null;
        scanSlider.gameObject.SetActive(false);
        scanHighlight.SetActive(false);
    }
}
