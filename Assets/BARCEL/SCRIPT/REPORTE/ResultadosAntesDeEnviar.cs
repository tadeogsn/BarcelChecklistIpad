using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ResultadosAntesDeEnviar : MonoBehaviour
{

    public int NumeroDeItems=6;
    public Text PuntoDeInpseccion;
    public Text TextParamRealesVALOR;
    public Text TextComentariosRESULTADO;
    public Image imagen;
    public List<InputEntry> entries = new List<InputEntry>();
    public List<objPuntosDeinspeccion> mchecklist = new List<objPuntosDeinspeccion>();
    public List<objTomarFoto> fotoTomada = new List<objTomarFoto>();
    // Start is called before the first frame update
   
    void Start()
    {

        //RECIBE EL VALOR  DEL JSON QUE SE ESCRIBIO CUANDO USUARIO INFRESA SUS INPUTS EN CADA PUNTO DE INSPECCION 
        // que se encuentra en winwdows = C:\Users\tadeo\AppData\LocalLow\DefaultCompany\Barcel\resultadoBD
        entries = FileHandler.ReadListFromJSON<InputEntry>("resultadoBD");
        mchecklist = FileHandlerChecklist.ReadListFromJSON<objPuntosDeinspeccion>("InfoChecklist");
        fotoTomada = FileHandler.ReadListFromJSON<objTomarFoto>("resultadoSERVER");
        
        for (int i = 0; i<entries.Count; i++)//SOLO UN FOR POR QUE LAS tres LISTAS MIDEN LO MISMO
        {
            Debug.Log("Foto tomadassssssssss"+fotoTomada[i].foto.Length);
            if (gameObject.name == entries[i].OrdenChecklist)
            {
                //var valorOrdenchecklit = int.Parse(gameObject.name);
                //Debug.Log("prueba vamos tu puedes!! " + valorOrdenchecklit);
                PuntoDeInpseccion.text = mchecklist[i].Titulo;
                TextParamRealesVALOR.text = entries[i].Results;
                TextComentariosRESULTADO.text = entries[i].Notas;


                //CODIGO DE LA IMAGEN

                //creamos una nueva textura para la vista previa
                Texture2D texture = new Texture2D(0, 0);

                //usamos la nueva textura para cargar los bytes 
                texture.LoadImage(fotoTomada[i].foto);

                //es la parte donde le damos tama�o a la textura que recbimos 
                Rect rect = new Rect(0, 0, texture.width, texture.height);

                //aqui creamos el sprite para que pueda ser compatible con la textura
                imagen.sprite = Sprite.Create(texture, rect, new Vector2(1f, 1f));

                //fin codgio imagen
                

            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
