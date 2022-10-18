using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generateUI : MonoBehaviour
{
    public GameObject canvas;
    public GameObject Panel;
    public GameObject image;
    //int size = 3;
    public List<InputEntry> entries = new List<InputEntry>();
    private float scaler = 0.0125f;
    public void ValoresItems(int size)
    {
        
    }
    private void OnEnable()
    {
        //RECIBE EL VALOR  DEL JSON QUE SE ESCRIBIO CUANDO USUARIO INFRESA SUS INPUTS EN CADA PUNTO DE INSPECCION 
        // que se encuentra en winwdows = C:\Users\tadeo\AppData\LocalLow\DefaultCompany\Barcel\resultadoBD
        entries = FileHandler.ReadListFromJSON<InputEntry>("resultadoBD");


        //AGREGAMOS LOS HIJOS EN EL PADRE DEL SCROLL RESULTADOS 
        Panel.transform.SetParent(canvas.transform, false);
        GameObject[] tiles = new GameObject[entries.Count];
        Vector3 change = new Vector3(20 * scaler, 0, 0);
        for (int i = 0; i < entries.Count; i++)
        {
            tiles[i] = GameObject.Instantiate(image, transform.position, transform.rotation);
            tiles[i].name = (i+1).ToString();//sumamos uno por que los objetos deben emepezar en uno
            tiles[i].transform.position += change;
            tiles[i].transform.SetParent(Panel.transform, false);
        }
    }
    public void destruirItems()//en el caso de que regrese a los puntos de inspeccion
    {
        //AGREGAMOS LOS HIJOS EN EL PADRE DEL SCROLL RESULTADOS 
        //Panel.transform.SetParent(canvas.transform, false);
        foreach (Transform child in Panel.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}
