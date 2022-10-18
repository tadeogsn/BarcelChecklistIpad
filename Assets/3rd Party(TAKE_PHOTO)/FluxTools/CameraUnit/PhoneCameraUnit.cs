using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PhoneCameraUnit : MonoBehaviour
{
    //variables estaticas para enviar valores a otro script y que reciba el valor de forma asyncorna
    public static PhoneCameraUnit serv;
    public static PhoneCameraUnit Instance { get { return serv;} }
    public byte[] ResultByteScreenshot; 
    ///////
    [Header("camera feed")]
    public PhoneCameraRenderer CameraFeed;
    
    [Header("camera record")]
    public Animator CameraFlickerScreen;
    private string flickerAnimationTrigger="FlickerNow";
    public float timeOfFlickerEffect = 0.5f;
    public string AlbumName = "your app name";
    public string FileNameOnGallery = "app_camera"; //without ending file type

    private Texture2D lastShotTaken;

    /// <summary>
    /// user just asked to make a screenshot (save the screen). we are doing a flicker animation
    /// </summary>
    public UnityEvent ShotStarted;

    /// <summary>
    /// system waits for the end of the rendering frame. we are a moment before actually taking the snapshot
    /// use this place to remove all ui elements you dont want to appear in the shot
    /// </summary>
    public UnityEvent RightBeforeTakingShot;

    /// <summary>
    /// finished making the screenshot. it takes a few moments. 
    /// use this to return the ui elements removed for the shot. and to send the result
    /// </summary>
    [System.Serializable]
    public class ValueTexture2DChanged : UnityEvent<Texture2D> { };
    public ValueTexture2DChanged CameraShotTaken;

    //TADEO
    public Image vistaprevia;

    public Image pantallaCompleta;

    ///tadeo
      private void Awake(){
        if(serv!=null && serv != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            serv = this;
        }
    }

    void Start()
    {
        CameraFeed.OnCameraInitSuccessfully.AddListener(OnCameraFeedInitSuccessfully);
        //StartCoroutine(sendFile(lastShotTaken.EncodeToPNG()));
    }

    private void OnCameraFeedInitSuccessfully()
    {
        //match size of flicker plane to size of camera 
        CameraFlickerScreen.transform.localScale = new Vector3(CameraFeed.transform.localScale.x, CameraFeed.transform.localScale.y, 1);
    }

    public void TakeShot()
    {
        ShotStarted.Invoke();

        CameraFlickerScreen.SetTrigger(flickerAnimationTrigger);

        // Take a screenshot and save it to Gallery/Photos
        StartCoroutine(TakeScreenshotAndSave());
    }

    private IEnumerator TakeScreenshotAndSave()
    {
        //finish flicker animation
        yield return new WaitForSeconds(timeOfFlickerEffect);

        RightBeforeTakingShot.Invoke();

        //finish render
        yield return new WaitForEndOfFrame();

        lastShotTaken = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        lastShotTaken.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        
        if (lastShotTaken != null) lastShotTaken.Apply();
        
        CameraShotTaken.Invoke(lastShotTaken);

        SaveTexture(lastShotTaken);

        Debug.Log("tamaño de la imagen"+lastShotTaken);
        //tadeo-------------------------
        //cpnvertimos la textura2D a bytes
        byte[] bytes = lastShotTaken.EncodeToPNG();
        
        //creamos una nueva textura para la vista previa
        Texture2D texture = new Texture2D(0, 0);

        //usamos la nueva textura para cargar los bytes 
        texture.LoadImage(bytes);

        //es la parte donde le damos tamaño a la textura que recbimos 
        Rect rect = new Rect(0, 0, texture.width, texture.height);


        //VISTA PREVIA
        //activamos la vista previa
        vistaprevia.enabled = true;

        //aqui creamos el sprite para que pueda ser compatible con la textura(vista previa)
        vistaprevia.sprite = Sprite.Create(texture, rect, new Vector2(1f, 1f));
        //FIN VISTA PREVIA

        //creamos uno con el mismo resulta pero para la pantalla completa
        pantallaCompleta.sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));

        //preubas para enviar el byte recibido a el script de checklist
        if(lastShotTaken.EncodeToPNG().Length!=0)
        {
            ResultByteScreenshot= lastShotTaken.EncodeToPNG();
            //Debug.Log("Tadeo  jjjj"+ResultByteScreenshot.Length);

        }

        //activamos la rutina y enviamos los bytes para que se envie al servidor
        //StartCoroutine(sendFile(lastShotTaken.EncodeToPNG()));
        
    }

    private void NativeGalleryError(string result_error)
    {
        Debug.Log(result_error);
    }

    public void SaveTexture(Texture2D file)
    {
        if (file == null) return;

#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
            // Save the screenshot to Gallery/Photos
            var result = NativeGallery.SaveImageToGallery(file, AlbumName, FileNameOnGallery+".png", NativeGalleryError);
            Debug.Log("Permission result: " + result);
            Debug.Log("Tadeo es la mera ostia"+file);
            //urlFile=FileNameOnGallery+".png";

            Destroy(file);  
#endif
    }

    IEnumerator sendFile(byte[] bytes)
    {
       WWWForm form = new WWWForm();
        //Debug.Log("tamaño de la imagen en IEnumerator"+lastShotTaken.height);
        //form.AddBinaryData("myimage", bytes, "myImage.png", "image/png");
        //esta metodo ayuda a enviar la imagen a un servicio php donde lo guarda con /año/mes/dia/hora/minutos/segundos
        form.AddBinaryData("myimage", bytes, DateTime.Now.ToString($"yyyy_MM_dd_HH_mm_ss")+".png", "image/png");

    
    WWW w = new WWW ("http://192.168.8.39/pruebaOnirix/pruebaImagen/", form);

		yield return w;

		if (w.error != null) {
			//error : 
			
				Debug.Log("error :"+w.error); //or OnErrorAction.Invoke (w.error);
		} else {
			//success
			
				Debug.Log(w.text); //or OnCompleteAction.Invoke (w.error);
		}
		w.Dispose ();
    }

}
