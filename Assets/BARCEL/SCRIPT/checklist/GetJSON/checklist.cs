using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class checklist : MonoBehaviour
{
    //public TextAsset pruebaGETJsson;
    //public TextAsset pruebaPostJson;
    string url = "http://192.168.8.39/Barcel/pruebas/";
    public OrdenChecklistController OrdenChecklistController_;
    public InputHandler inputHandler_;

    //INICIO->cada vez que se cree un paso de checklist se debe de anclar el numero(gameobject) para que funcione con la RA
    public OrdenAssetsRAController[] OrdenAssetARController_;
    //FIN
    
    public ChecklistList mchecklist = new ChecklistList();


    public int contadorJSON = 1;// ESTE CONTADOR SIRVE PARA RECORRER EL JSON CON LOS BOTONES "SIGUIENTE" Y ANTERIOR
    public int contadorVideos = 1;// ESTE CONTADOR SIRVE PARA RECORRER EL LOS VIDEOS QUE VIENEN DEL JSON Y SE USA PARA CAMBIAR DESDE LOS BOTONES DEL VIDEO

    // Start is called before the first frame update

    ///FINALIZAR CHECKLIST
    public GameObject DisableChecklist;
    public GameObject EnbaleEnviarChecklist;


    /// ESTE MENSAJE ES PARA ALERTAR QUE NO HAY INTERNET>
    public GameObject SMSNoInternet;
    /// </summary>
    /// 


    //PADRE DE LOS PUNTOS DE INSPECCION
    public GameObject padrePuntosInspeccion;

    string horaActualNombreProcedimiento;

    // CUANDO PRESIONE BOTON "ANTERIOR" SE EJECUTE LA FUNCION DE INSERTAR DATOS
    bool sumarContador=true;

    void Start()
    {

        //RELLENAMOS EL TAMAÑO DINAMICAMENTE DE LOS "PUNTO DE REFERENCIA" QUE HAY EN LA ESCENA, RECORDAR QUE LOS PUNTOS DE REFERECIA SE AGREGAN MANUALMENTE
        //ESTE SCRIPT SOLO LLENA LA LISTA DINAMINCAMENTE A TRAVES DE LOS HIJOS QUE HAY EN LA SECENA
        //EL PADRE ES "Contador(punto de referencia)"


        GameObject go = padrePuntosInspeccion;//buscamos al padre

        Debug.Log(go.name + " has " + go.transform.childCount + " children");

        OrdenAssetARController_ = new OrdenAssetsRAController[go.transform.childCount];//creamos el tamño de los hijos 

        for (int i = 0; i < go.transform.childCount; i++)
        {

            OrdenAssetARController_[i] = go.transform.GetChild(i).GetComponent<OrdenAssetsRAController>();//insertamos los hijos en el array

        }


        ///fin//


        ///empezamos consultando las instrucciones del checklist
        StartCoroutine(Post(url));
    }

    // Update is called once per frame

    /// <summary>
   // ESTAS FUNCIONES SON PARA QUE PASE AL SIGUEINTE TEXTO DEL CHECKLIST
    /// </summary>
    public void Sumacontador()
    {
        contadorJSON++;
        StartCoroutine(Post(url));
        sumarContador = true;
    }
    public void restarContador()
    {
        sumarContador = false;
        contadorJSON--;
        StartCoroutine(Post(url));

        ///me quede aqui en la  libreta tengo el codigo para continuar con la condicion desde la misma funciion "DisplayDataPanel" para que dependiedon
        ///el contador este lo compara con la variable de ordenChecklist
    }
    public IEnumerator Post(string url)
    {
        
        WWWForm form = new WWWForm();
        ////enviamos el json
        UnityWebRequest www = UnityWebRequest.Post(url, form);
        // Debug.Log(www.SendWebRequest());
        yield return www.SendWebRequest();
        Debug.Log("entro al metdo espero diosito que si entre por que ya me canse ajaja");
        if (www.result != UnityWebRequest.Result.Success)
        {
            // Debug.Log("este es el resultado de mi petición ");
            Debug.Log(www.error);
            SMSNoInternet.SetActive(true);
        }
        else
        {
            SMSNoInternet.SetActive(false);
            Debug.Log("este es el resultado de mi petición ");
            if (www.isDone)
            {
                

                Debug.Log("este es el resultado de mi petición ");
                //handle the result
                //recibimos los datos en JSON del server(php)
                var result = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
                Debug.Log("este es el resultado de mi petición " + result);

                //el resultado del JSON hace match con el la clase que creamos "almacenGsn" ya que tiene la misma estructura

                

                //obtnemos JSON y el resultado lo anclamos con nuestro que es similar al JSON que recibimos
                mchecklist = JsonUtility.FromJson<ChecklistList>(result);

                ///GUARDAR EL RESULTADO EN JSON PARA USARLO O GUARDALOR EN UN ARCHIVO
                FileHandler.SaveToJSON<ChecklistList>(mchecklist, "InfoChecklist");


                Debug.Log("checklist "+mchecklist.Checklist.Length+1+ " contador "+ contadorJSON);
                if (contadorJSON== (mchecklist.Checklist.Length+1))//contador comienza de 1 y el tamaño del objeto comienza de 0 y se suma 1 para que empate con el contador
                {
                    Debug.Log("Entro aqui");
                    //HORA ACUTAL Y NOMBRE DE PROCEDIMIENTO
                    horaActualNombreProcedimiento = DateTime.Now.ToString($"yyyy_MM_dd_HH_mm_ss") + mchecklist.Checklist[contadorJSON - 2].NombreProcedimiento;
                    inputHandler_.ButonSiguientePost(mchecklist, contadorJSON, horaActualNombreProcedimiento);
                    inputHandler_.ButtonTomarFoto(contadorJSON, horaActualNombreProcedimiento);
                    DisableChecklist.SetActive(false);//desactivar checklist
                    EnbaleEnviarChecklist.SetActive(true);//activar panel de envio de datos

                }
                else
                {

                    Debug.Log("Entro aqui");//entro aqui en anterior
                    OrdenChecklistController_.DisplayDataPanel(mchecklist, contadorJSON, inputHandler_.entries.Count);//funcion que controla el panel 2D solo recibe datos


                    if (contadorJSON > 1&&sumarContador==true)//cuando se suma
                    {
                        Debug.Log("Entro aqui " + "contadorJson "+ (contadorJSON));
                        //HORA ACUTAL Y NOMBRE DE PROCEDIMIENTO
                        horaActualNombreProcedimiento = DateTime.Now.ToString($"yyyy_MM_dd_HH_mm_ss") + mchecklist.Checklist[contadorJSON - 2].NombreProcedimiento;
                        inputHandler_.ButonSiguientePost(mchecklist, contadorJSON, horaActualNombreProcedimiento);//entra para hacer la la interactividad para el envio de datos 
                        inputHandler_.ButtonTomarFoto(contadorJSON, horaActualNombreProcedimiento);
                    }else if (contadorJSON > 1 && sumarContador == false)
                    {
                        Debug.Log("Entro aqui " + "contadorJson " + (contadorJSON -2));
                        //HORA ACUTAL Y NOMBRE DE PROCEDIMIENTO
                        horaActualNombreProcedimiento = DateTime.Now.ToString($"yyyy_MM_dd_HH_mm_ss") + mchecklist.Checklist[contadorJSON-1].NombreProcedimiento;
                        inputHandler_.ButonSiguientePost(mchecklist, contadorJSON+1, horaActualNombreProcedimiento);//entra para hacer la la interactividad para el envio de datos 
                        inputHandler_.ButtonTomarFoto(contadorJSON+1, horaActualNombreProcedimiento);
                    }
                    else if(contadorJSON==1 && sumarContador == false)
                    {
                        Debug.Log("Entro aqui " + (contadorJSON + 2));//entro aqui en anterior
                        //hay que ver como resivir los datos ya que truena el codigo si pongo lo de abajo
                        horaActualNombreProcedimiento = DateTime.Now.ToString($"yyyy_MM_dd_HH_mm_ss") + mchecklist.Checklist[contadorJSON + 2].NombreProcedimiento;
                        inputHandler_.ButonSiguientePost(mchecklist, (contadorJSON+1), horaActualNombreProcedimiento);//entra para hacer la la interactividad para el envio de datos 
                       
                        inputHandler_.ButtonTomarFoto(contadorJSON+1, horaActualNombreProcedimiento);
                    }



                }
                
                for (int i =0; i<OrdenAssetARController_.Length; i++)
                {
                    //Debug.Log("Entro aqui");
                    OrdenAssetARController_[i].ShowAssetsRA(contadorJSON);//gameobjects que van de lado de area target en la parte 3d
                }
            }
            else
            {
                //handle the result 
                Debug.Log("Error! data couldnt get.");
            }
        }
    }


    
}
