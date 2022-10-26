using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;



public class InputHandler : MonoBehaviour
{
    [SerializeField] string var_OrdenCheck;
    [SerializeField] InputField var_resultInput;
    [SerializeField] public string valorReal;
    [SerializeField] InputField var_notas;
    [SerializeField] GameObject botonCamra;//uso para cuando sea diferente de vacio de inputvalorreal 

    /// <guardar imagen en base de datos y en el server de forma se parada pero conincide el nombre>
    [SerializeField] string InfoImagen;//esto para el objeto para que se guarde en base de datos
    [SerializeField] string Infohora;
    /// </summary>

    [SerializeField] string filenameJsonBD;
    [SerializeField] string filenameJsonFotoServer;

    //script de TOMAR FOTO
    public PhoneCameraUnit ScriptCamara;
    public List<objTomarFoto> fotoTomada = new List<objTomarFoto>();
    public Image vistaPreviaImg;
    //fin//

    public List<InputEntry> entries = new List<InputEntry>();

    //objeto de la foto tomada
    public sendServerObjFoto sendfotoTomada = new sendServerObjFoto();

    public Text smsFtosServer;

    //lo pongo en esta parte ya que varias IEnumerator lo usaran
    UnityWebRequest www;

    //BARRA DE PROGRESO ACTUAL y CONTADOR DE IMAGENES SUBIDAS
    public Image barUI;
    public Text progresoText;
    public float valorNuevo;

    public float valorAnteriorBar;

    private void Start()
    {
        entries = FileHandler.ReadListFromJSON<InputEntry>(filenameJsonBD);
        fotoTomada = FileHandler.ReadListFromJSON<objTomarFoto>(filenameJsonFotoServer);

        //VISTA PREVIA DE LA PRIMERA PANTALLA
        for (int i = 0; i < fotoTomada.Count; i++)
        {
            Debug.Log("entro a foto");
            if (i == 0)
            {
                Debug.Log("entro a foto"+ " VALOR real");
                //NOTAS DEL PUNTO DE INSPECCION ACTUAL(i)
                var_notas.text = entries[i].Notas;
                //FIN DE NOTAS

                //Input de valor real del punto de inspeccion actual(i)
                var_resultInput.text = entries[i].Results;
                valorReal = var_resultInput.text;
                //FIN valor real


                //VISTA PREVIA
                //creamos una nueva textura para la vista previa
                Texture2D texture = new Texture2D(0, 0);
                Debug.Log("entro a foto");
                //usamos la nueva textura para cargar los bytes 
                texture.LoadImage(fotoTomada[(i)].foto);

                //es la parte donde le damos tama�o a la textura que recbimos 
                Rect rect = new Rect(0, 0, texture.width, texture.height);
                //activamos la vista previa
                vistaPreviaImg.enabled = true;

                ////aqui creamos el sprite para que pueda ser compatible con la textura(vista previa)
                vistaPreviaImg.sprite = Sprite.Create(texture, rect, new Vector2(1f, 1f));
                //FIN VISTA PREVIA
            }
        }

    }


    public void InputParametroReal()
    {
        valorReal=var_resultInput.text;
        Debug.Log("entro a foto"+ " VALOR real2");
        Debug.Log("Entro al input de valor real " + valorReal);
        if (var_resultInput.text != "") botonCamra.SetActive(true); else botonCamra.SetActive(false);
    }

    public void ultimoValorChecklist(){//se usa cuando ya estoy en los resultados, y quiero regresar al checklist pero necesito dibujar los ultimos inputs de checklist
            //NOTAS DEL PUNTO DE INSPECCION ACTUAL(i)
                        var_notas.text = entries[entries.Count-1].Notas;
                        //FIN DE NOTAS

                        //Input de valor real del punto de inspeccion actual(i)
                        var_resultInput.text = entries[entries.Count-1].Results;
                        Debug.Log("entro a foto"+ " VALOR real 5");
                        
                        valorReal = entries[entries.Count-1].Results;
                        //FIN valor real
    }


    public void ButonSiguientePost(PuntosDeInspeccionList Checjelist, int Contador, string horaActualNombre)
    {
        //escribimos el formato como ira insertado a la base de datos
        Infohora = horaActualNombre;
        InfoImagen = "image/" + horaActualNombre + ".png";
        Debug.Log("aquitoy0 " + Contador);



        //dataFoto = ScriptCamara.ResultByteScreenshot.Length;

       // Debug.Log("entries.Count(LISTA EMPIEZA 1)= " + entries.Count + "valor de contador= " + (Contador-2) + "  ORDEN CHEKLIST= "+ Checjelist.Checklist[Contador - 2].ordenChecklist);
        if (entries.Count==Contador-2)///CUANDO SE EMPIEZA EL PROCEDIEMIENTO
        {
            Debug.Log("entro a foto"+ " VALOR real 3" +Contador);
            Debug.Log("aquitoy0 " + var_notas.text);
            if (var_notas.text == "") var_notas.text = "Sin comentarioss";
            //if (var_resultInput.text != "") botonCamra.SetActive(true); else botonCamra.SetActive(false);
            entries.Add(new InputEntry(Checjelist.PuntosDeInspeccion[Contador-2].Orden, valorReal, InfoImagen, var_notas.text));
            

            FileHandler.SaveToJSON<InputEntry>(entries, filenameJsonBD);
            var_notas.text = "";
            var_resultInput.text = "";
            valorReal = "";
        }
        else//CAUNDO REGRESAN CORREGIR VALORES INGRESADOS
        {
            for (int i = 0; i < entries.Count; i++)
            {
                Debug.Log("valor de entries "+i);
                if (i == Contador - 2)//se comp�ra con la lista "entires", "i" empieza de cero, contador de cero pero con los textos, cuando empieza la scena "Start()" ya conto 0 y cuando doy siguiente ya es 1 por eso lo resto "-1"
                {
                    Debug.Log("aquitoy1 " + valorReal+ i);
                    Debug.Log("entro a foto"+ " VALOR real 4");
                    if (var_notas.text == "") var_notas.text = "Sin comentarios " + i;
                    if (var_resultInput.text != "") botonCamra.SetActive(true); else botonCamra.SetActive(false);
                    //al contador esta con -1 ya que utilzamos un valor "anterior" para el boton siguiente
                    entries[i] = new InputEntry(Checjelist.PuntosDeInspeccion[Contador - 2].Orden, valorReal, InfoImagen, var_notas.text);
                    FileHandler.SaveToJSON<InputEntry>(entries, filenameJsonBD);
                    var_notas.text = "";
                    var_resultInput.text = "";
                    valorReal = "";
                }
            }
        }
    }
    public void ButtonTomarFoto(int Contador, string horaActualNombre, bool sumarContador)
    {
        
        Debug.Log("entro a foto 1 "+ Contador);
        //creamos una lista donde se almacenaran los bytes para despues usarlos cuando se guarden en el servidor
        Debug.Log("Foto tomada " + fotoTomada.Count + " contador " + Contador);
        if (fotoTomada.Count == Contador - 2 )//PRIMERA VEZ QUE SE TOMAN TODAS LAS FOTOS
        {
            Debug.Log("entro a foto 2");
            fotoTomada.Add(new objTomarFoto(ScriptCamara.ResultByteScreenshot, horaActualNombre));

            Debug.Log("Foto tomada1" + ScriptCamara.ResultByteScreenshot.Length);

            FileHandler.SaveToJSON<objTomarFoto>(fotoTomada, filenameJsonFotoServer);

            ///limpiamos la captura tomada
            ScriptCamara.ResultByteScreenshot = new byte[0]; 
        }
        else// CUANDO REGRESAN A TOMAR DE NUEVO LAS FOTOS
        {
            Debug.Log("entro a foto 3");
            for ( int i =0; i<fotoTomada.Count; i++ )//fotos tomadas en la experiencia 
            {
                
                Debug.Log("entro a foto 4");
                if ( i == Contador -2 )
                {
                    
                    Debug.Log("entro a foto 5.1 " + "contador " + (Contador));
                    if ((i+1)<fotoTomada.Count||(i+1)<=fotoTomada.Count&& sumarContador==false)//evita que envie valor que no existe en el boton "siguiente" del ultimo "punto de inspeccion"
                    {
                        Debug.Log(" ciclo for "+i);
                        if(sumarContador==true){//SI SE DIO BOTON SIGUIENTE
                            Debug.Log("entro a foto 5" + "contador " + (Contador) + "valor de i " + i +"entrie count"+ entries[i].Results +" valor real "+valorReal);
                        //NOTAS DEL PUNTO DE INSPECCION ACTUAL(i)
                        var_notas.text = entries[(i+1)].Notas;
                        //FIN DE NOTAS

                        //Input de valor real del punto de inspeccion actual(i)
                        var_resultInput.text = entries[(i+1)].Results;
                        Debug.Log("entro a foto"+ " VALOR real 5");
                        
                        valorReal = entries[(i+1)].Results;
                        //FIN valor real


                        //VISTA PREVIA CADA QUE SE LE DA SIGUIENTE AL BOTON
                        //creamos una nueva textura para la vista previa
                        Texture2D texture = new Texture2D(0, 0);
                        //Debug.Log("entro a foto"+(i + 1));
                        //usamos la nueva textura para cargar los bytes 
                        texture.LoadImage(fotoTomada[(i + 1)].foto);

                        //es la parte donde le damos tamaño a la textura que recbimos 
                        Rect rect = new Rect(0, 0, texture.width, texture.height);
                        //activamos la vista previa
                        vistaPreviaImg.enabled = true;

                        ////aqui creamos el sprite para que pueda ser compatible con la textura(vista previa)
                        vistaPreviaImg.sprite = Sprite.Create(texture, rect, new Vector2(1f, 1f));
                        //FIN VISTA PREVIA
                        }else
                        {
                            // SI SE DIO BOTON ANTERIOR
                                Debug.Log("entro a foto 5.5 " + " contador " + (Contador) + "valor de i " + i +"entrie count"+ entries[i].Results+" valor real "+valorReal);
                            //NOTAS DEL PUNTO DE INSPECCION ACTUAL(i)
                            var_notas.text = entries[(i-1)].Notas;
                            //FIN DE NOTAS

                        //Input de valor real del punto de inspeccion actual(i)
                        var_resultInput.text = entries[(i-1)].Results;
                        Debug.Log("entro a foto"+ " VALOR real 5");
                        
                        valorReal = entries[(i-1)].Results;
                        //FIN valor real


                        //VISTA PREVIA CADA QUE SE LE DA SIGUIENTE AL BOTON
                        //creamos una nueva textura para la vista previa
                        Texture2D texture = new Texture2D(0, 0);
                        //Debug.Log("entro a foto"+(i + 1));
                        //usamos la nueva textura para cargar los bytes 
                        texture.LoadImage(fotoTomada[(i-1)].foto);

                        //es la parte donde le damos tamaño a la textura que recbimos 
                        Rect rect = new Rect(0, 0, texture.width, texture.height);
                        //activamos la vista previa
                        vistaPreviaImg.enabled = true;

                        ////aqui creamos el sprite para que pueda ser compatible con la textura(vista previa)
                        vistaPreviaImg.sprite = Sprite.Create(texture, rect, new Vector2(1f, 1f));
                        //FIN VISTA PREVIA
                        }
                        
                    }



                    ///GUARDAR FOTO EN LA UBUCACION ACTUAL DE DONDE FUE TOMADA
                    Debug.Log("tamaño de la foto tomada "+ScriptCamara.ResultByteScreenshot.Length);
                    if (ScriptCamara.ResultByteScreenshot.Length!=0)
                    {
                        Debug.Log("entro a foto 6");
                        fotoTomada[i] = new objTomarFoto(ScriptCamara.ResultByteScreenshot, horaActualNombre);

                        Debug.Log("Foto tomada2" + ScriptCamara.ResultByteScreenshot.Length);

                        FileHandler.SaveToJSON<objTomarFoto>(fotoTomada, filenameJsonFotoServer);

                        ///limpiamos la captura tomada
                        ScriptCamara.ResultByteScreenshot= new byte[0];
                    }
                    else
                    {
                        Debug.Log("entro a foto 7");
                    }

                }
                else if (Contador==1)//valor que se da en el primer "punto de inspeccion"
                {
                    if (i == 0)///primer imagen del array 
                    {
                        //NOTAS DEL PUNTO DE INSPECCION ACTUAL(i)
                        var_notas.text = entries[i].Notas;
                        //FIN DE NOTAS

                        //Input de valor real del punto de inspeccion actual(i)
                        var_resultInput.text = entries[i].Results;
                        valorReal = entries[i].Results;
                        Debug.Log("entro a foto"+ " VALOR real 6");

                        //FIN valor real


                        //VISTA PREVIA
                        //creamos una nueva textura para la vista previa
                        Texture2D texture = new Texture2D(0, 0);
                        Debug.Log("entro a foto");
                        //usamos la nueva textura para cargar los bytes 
                        texture.LoadImage(fotoTomada[(i)].foto);

                        //es la parte donde le damos tama�o a la textura que recbimos 
                        Rect rect = new Rect(0, 0, texture.width, texture.height);
                        //activamos la vista previa
                        vistaPreviaImg.enabled = true;

                        ////aqui creamos el sprite para que pueda ser compatible con la textura(vista previa)
                        vistaPreviaImg.sprite = Sprite.Create(texture, rect, new Vector2(1f, 1f));
                        //FIN VISTA PREVIA
                    }
                }
            }
        }
        
       

    }


    public void probar()
    {
        StartCoroutine(sendFile());
        StartCoroutine(UploadProgressCoroutine()); 
        
    }
    public IEnumerator sendFile()
    {
        var url = "http://localhost/Barcel/pruebas/serverImage.php";
        string json = File.ReadAllText(Application.persistentDataPath + "/resultadoSERVER");
        sendfotoTomada = JsonUtility.FromJson<sendServerObjFoto>(json);
        Debug.Log("foto tomadaaaa " + Application.persistentDataPath);
        WWWForm form = new WWWForm();
        for (int i = 0; i < sendfotoTomada.Results.Length; i++)
        {
            form.AddBinaryData("myimage", sendfotoTomada.Results[i].foto, sendfotoTomada.Results[i].HoraActualNombre + ".png", "image/png");
            www = UnityWebRequest.Post(url, form);
            // Debug.Log(www.SendWebRequest());
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                // Debug.Log("este es el resultado de mi petici�n ");
                Debug.Log(www.error);
                smsFtosServer.text = "la informacion no se cargo  ";
            }
            else
            {
                //success
                if (www.isDone)
                {
                    
                    
                    progresoText.text = (i + 1)+ "/" +sendfotoTomada.Results.Length.ToString()  ;

                    //SUMA LOS VALORES PARA RELLENAR LA BARRA DE CARGA
                    HandleProgress((i+1));

                    //VALOR TOTAL SEA IGUAL A A LA CARGA DE TODAS LAS IMAGENES
                    if ((i+1)== sendfotoTomada.Results.Length)
                    {
                        smsFtosServer.text = "LISTO ";
                        Debug.Log("las inmagenes se enviaron correctamente al servidor ");
                    }
                    else
                    {

                        
                        
                    }
                }
            }
        }
      

    }

    //te registra con un valor cuanto lleva subido
    public IEnumerator UploadProgressCoroutine()//POR EL MOMENTO NO SE OCUPA
    {
        valorAnteriorBar++;
        while (!www.isDone)
        {
            //HandleProgress((www.uploadProgress));
            //valorAnteriorBar =+ 1f;
            smsFtosServer.text = ((valorAnteriorBar / 4)*100f).ToString();
            Debug.Log("Barra de progreso " + www.uploadProgress);
            yield return null;
        }
    }

    ///BARRA DE PROGRESO UI
    public void HandleProgress(float currentProgress)
    {
            valorNuevo=+ currentProgress;// se divide entre el valor total de las fotos para que ya que el valor de el barUI es 1f

            // currentProgress is value between 0 and 1
            barUI.fillAmount = valorNuevo / sendfotoTomada.Results.Length;//llena la barra con valor maximmo de 1f
    }
}