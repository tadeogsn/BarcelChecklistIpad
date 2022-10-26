using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class genrateUIRoles : MonoBehaviour
{
    public TextAsset pruebaPostJson;
    public RolesList rolesList = new RolesList();
    public GameObject canvas;
    public GameObject Panel;
    public GameObject image;
    //int size = 3;
    private float scaler = 0.0125f;
    public void ValoresItems(int size)
    {

    }
    private void Start()
    {
        //RECIBE EL VALOR  DEL JSON QUE SE ESCRIBIO CUANDO USUARIO INFRESA SUS INPUTS EN CADA PUNTO DE INSPECCION 
        // que se encuentra en winwdows = C:\Users\tadeo\AppData\LocalLow\DefaultCompany\Barcel\resultadoBD
        rolesList = JsonUtility.FromJson<RolesList>(pruebaPostJson.text);


        //AGREGAMOS LOS HIJOS EN EL PADRE DEL SCROLL RESULTADOS 
        Panel.transform.SetParent(canvas.transform, false);
        GameObject[] tiles = new GameObject[rolesList.Roles.Length];
        Vector3 change = new Vector3(20 * scaler, 0, 0);
        for (int i = 0; i < rolesList.Roles.Length; i++)
        {
            tiles[i] = GameObject.Instantiate(image, transform.position, transform.rotation);
            tiles[i].name = (i).ToString();//sumamos uno por que los objetos deben emepezar en uno
            tiles[i].transform.position += change;
            tiles[i].transform.SetParent(Panel.transform, false);
        }
    }
    //public void destruirItems()//en el caso de que regrese a los puntos de inspeccion
    //{
    //    //AGREGAMOS LOS HIJOS EN EL PADRE DEL SCROLL RESULTADOS 
    //    //Panel.transform.SetParent(canvas.transform, false);
    //    foreach (Transform child in Panel.transform)
    //    {
    //        GameObject.Destroy(child.gameObject);
    //    }
    //}
}
