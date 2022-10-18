using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class login : MonoBehaviour
{
    public InputField NumeroDeEmpleado;
    public InputField Contrase�a;
    public Text textError;


    public void validarLogin()
    {
        if (NumeroDeEmpleado.text==""||Contrase�a.text=="")
        {
            textError.text="Ingresar ususario o contrase�a";
        }
        else
        {
            textError.text = "";
            //Debug.Log("entro");
            StartCoroutine(Login(NumeroDeEmpleado.text,Contrase�a.text));
        }
        
    }

    private IEnumerator Login(string numeroDeEmpleado, string contrase�a)
    {
        
        string url = "http://192.168.8.39/Barcel/pruebas/login.php";
        WWWForm form = new WWWForm();
        form.AddField("NumeroDeEmpleado", numeroDeEmpleado);
        form.AddField("Contrase�a", contrase�a);
        UnityWebRequest www = UnityWebRequest.Post(url, form);
        yield return www.SendWebRequest();
        Debug.Log("entro");
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
            textError.text = "No se puede conectar al servidor";//podria ser que el xampp no este conectado
        }
        else
        {
            if (www.isDone)
            {
                var result = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
                Debug.Log("este es el resultado de mi petici�n " + result);

                //ESTOS RESULTADOS VIENEN DE Login.php
                if (result == "los datos no se pudieron agregar a la base de datos" || result == "Usuario o contrase�a incorrectos")
                {
                    Debug.Log("este es el resultado de mi petici�n " + result);
                    textError.text = result;
                }
                else
                {
                    Debug.Log(result);
                    //guardaremos el ID(tabla=SesionEmpleados) que recibimos del INSERT(procedimientos alamacenados) 
                    //cargaremos la siguiente escena de CHECKLIST
                    PlayerPrefs.SetString("Id_sesionEmpleados", result);
                    SceneManager.LoadScene("Checklist");
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
