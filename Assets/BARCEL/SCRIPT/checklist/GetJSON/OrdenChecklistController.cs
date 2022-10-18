using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class getObjectNumber
{
    //TEXTOS
    public Text ordenChecklist;
    public Text tituloChecklist;
    public Text descripcion;
    public Text nombreProcedimiento;

    //BOTONES SIGUIENTE, ANTERIOR Y TOMAR FOTO

    public GameObject botonAnterior;
    public GameObject botonSiguiente;
    public GameObject botonTomarFoto;
}




public class OrdenChecklistController : MonoBehaviour
{
    public getObjectNumber objCanvas;

    //ESTA FUNCION SE EJECUTA EN EL SCRIPT DE "checklist.cs"
    public void DisplayDataPanel(ChecklistList Checjelist, int Contador, int listSizeEntries)
    {

        Debug.Log("ChecklistList size " +Checjelist.Checklist.Length+" y tamaño de entries "+listSizeEntries);
        //obtenemos el ultimo valos del checklist
        var _contador = Contador - 1;//ya que el objeto "Checjelist.Checklist" empieza de cero
        Debug.Log("valor contador "+ _contador);
        var lasnumero = Checjelist.Checklist.Length;
        //Debug.Log("contador de ordenChecklist "+ valueObj + "last numero "+lasnumero);
        //var lastElement = Checjelist.Checklist[Checjelist.Checklist.Length-1];

        //SOLO TEXTOS del checklist 
        //if(checamos si recibimos un valor)
        //else(dibujamos el valor en los "text" UI 
            if (Contador > lasnumero)
            {
                 objCanvas.ordenChecklist.text      = "";
                 objCanvas.tituloChecklist.text     = "TERMINADO";
                 objCanvas.descripcion.text         = "Presionar a enviar para que los datos sean almacenados en el servidor y puedan ser vistos por los administradores";
                 objCanvas.nombreProcedimiento.text = "................";
            }
            else if (Checjelist.Checklist[_contador].ordenChecklist == "" && Checjelist.Checklist[_contador].tituloChecklist == "" && Checjelist.Checklist[_contador].description == "" && Checjelist.Checklist[_contador].NombreProcedimiento == "")
            {
                 objCanvas.ordenChecklist.text      = "No hay numero";
                 objCanvas.tituloChecklist.text     = "No hay titulo";
                 objCanvas.descripcion.text         = "Descripción no disponible";
                 objCanvas.nombreProcedimiento.text = "................";

            }
            else
            {
                ///la logica no tiene lo de ordenar el checklist segun el "ordenChecklist" hacerlo si es que se necesita
                 objCanvas.ordenChecklist.text      = Checjelist.Checklist[_contador].ordenChecklist;
                 objCanvas.tituloChecklist.text     = Checjelist.Checklist[_contador].tituloChecklist;
                 objCanvas.descripcion.text         = Checjelist.Checklist[_contador].description;
                 objCanvas.nombreProcedimiento.text = Checjelist.Checklist[_contador].NombreProcedimiento;
            }
        // BOTONES SIGUEINTE(UI) O ANTERIOR(UI)
        //ACTIVA Y DESACTIVA AL PRINCIPIO Y AL FINAL
            
            //if (_contador == 0 &&  listSizeEntries!=0)// si contador = 0 y boton de camara esta desactivado
            //{
            Debug.Log("si el ultimo numero es igual al contador");
                //for (int i = 0; i < listSizeEntries; i++)
                //{
                    if (listSizeEntries>=(_contador+1))//solo en el caso de que el valor de "i" exista en el array 
                    {
                        Debug.Log("si el ultimo numero es igual al contador " + "valor de entries "+ listSizeEntries+ " valor de contador "+(_contador+1));
                        for (int i = 0; i < listSizeEntries; i++)
                        {
                            if (i==0&&_contador==0)//solo en el caso de que del primer puento de inspeccion
                            {
                                Debug.Log("si el ultimo numero es igual al contador"+ " valor de i= "+i+ " valor de contador =" + _contador);
                                objCanvas.botonAnterior.SetActive(false);
                                objCanvas.botonSiguiente.SetActive(true);
                                objCanvas.botonTomarFoto.SetActive(true);
                            }   
                            else if(i>0 && _contador > 0)//en el caso de que sea cualquier otro punto de inspeccion
                            {
                                Debug.Log("si el ultimo numero es igual al contador"+ " valor de i= "+i+ " valor de contador =" + _contador);
                                objCanvas.botonAnterior.SetActive(true);
                                objCanvas.botonSiguiente.SetActive(true);
                                objCanvas.botonTomarFoto.SetActive(true);
                            }
                        }

                    }else
                    { 
                        Debug.Log("si el ultimo numero es igual al contador");
                        if (Checjelist.Checklist.Length <= 1)
                        {
                            Debug.Log("si el ultimo numero es igual al contador");
                            objCanvas.botonAnterior.SetActive(false);
                            objCanvas.botonSiguiente.SetActive(false);
                        }

                        else if (_contador == 0)// si contador = 0 y boton de camara esta desactivado
                        {

                            Debug.Log("si el ultimo numero es igual al contador");
                            objCanvas.botonAnterior.SetActive(false);
                            objCanvas.botonSiguiente.SetActive(false);


                        }
                        else if (_contador >= 1 && _contador < lasnumero && objCanvas.botonTomarFoto.activeSelf == false)//contador es mayor a 1 y contador es menor al ultimo numero de la lista
                        {
                            Debug.Log("si el ultimo numero es igual al contador");
                            objCanvas.botonAnterior.SetActive(true);
                            objCanvas.botonSiguiente.SetActive(false);
                        }
                        else if (lasnumero == _contador && objCanvas.botonTomarFoto.activeSelf == false)//si el ultimo numero es igual al contador
                        {
                            Debug.Log("si el ultimo numero es igual al contador");
                            objCanvas.botonAnterior.SetActive(true);
                             objCanvas.botonSiguiente.SetActive(false);
                        }
                        else
                        {
                            Debug.Log("si el ultimo numero es igual al contador");
                            objCanvas.botonSiguiente.SetActive(false);
                            //objCanvas.botonAnterior.SetActive(false);
                        }
                    }
                //}
            
            //}else 
            //{ 
            //    Debug.Log("si el ultimo numero es igual al contador");
            //}

        }




        //ESTA FUNCION SE EJECUTA EN EL SCRIPT DE "checklist.cs"
        //Checjelist=EL OBJETO PADRE DEL JSON
        //contador  = es el contador del numero de objetos que genera por instruccion de checlist
        //contadorvideo = de cada instruccion que genera se genera un multimedia(en este caso video) aqui tomamos todos lo multimedia(imagen, pdf y etc), pero lo clasificamos en tipo video
        
        

}

    
