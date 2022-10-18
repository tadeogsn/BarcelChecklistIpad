using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;
using System.Net;
using System.Collections.Specialized;
using System.Text;
using UnityEngine.UI;


public class reporteChecklist : MonoBehaviour
{

    public Text MensajeServer;
    public GameObject PanelEnvioServer;
    public GameObject BotonEnvioReporte;

    public InputHandler inputHandler_;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void EnviarReporte()
    {
        //enviamos el reporte junto con el id guardado del Insert(procedimientos almacenados) que hicimos en el login
        ReporteFinalChecklist(PlayerPrefs.GetString("Id_sesionEmpleados"));

        //Enviar imagenes al servidor directamente los nombres coninciden con los de la base de datos
       inputHandler_.probar();
        

    }
    public void  ReporteFinalChecklist(string Id_sesionEmpleados)//uso para envira JSON->me ayudaron en el grupo de facebook de unity
    {
        PanelEnvioServer.SetActive(true);//lo puse aqui por que en otro lado  fuera de la funcion o directo del buton no funciona como debe de ser
        BotonEnvioReporte.SetActive(false);

        /////////////////////////////////////////////
        string url= "http://192.168.8.39/Barcel/pruebas/reporte.php";
        string json = File.ReadAllText(Application.persistentDataPath + "/resultadoBD");
        var wclient = new WebClient();
        var campos = new NameValueCollection();
        campos.Add("jsonReporte", json);
        campos.Add("Id_sesionEmpleados", Id_sesionEmpleados);
        var response = wclient.UploadValues(url, "POST", campos);
        var respuesta = Encoding.Default.GetString(response);
        var respuesta_sinEspacios=respuesta.Trim();//quita los espacios vacios del final
        Debug.Log("RESPUESTA DE reporte.php " +respuesta_sinEspacios);
        if (respuesta_sinEspacios == "La consulta es un exito")
        {
            //actvar mensaje de "los datos se enviaron correctamente"
            //desactivar el panel de "enviando datos al servidor"
            MensajeServer.text = respuesta_sinEspacios;
            PanelEnvioServer.SetActive(false);

        }
        else
        {
            //activar de nuevo el boton de enviar
            //desactivar el panel de "enviando datos al servidor"
            //mensaje de "Hubo un problema al enviar los datos al servidor, checar su conexion de internet"}
            MensajeServer.text = respuesta_sinEspacios;
            PanelEnvioServer.SetActive(false);
            BotonEnvioReporte.SetActive(true);
        }
    }


    
}
