using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Video;



public class OrdenAssetsRAController : MonoBehaviour
{



    public void ShowAssetsRA(int Contador)
    {
        if (Contador.ToString() == gameObject.name)//esto ayuda oara verificar en que paso(contador) me encuentro y activar "gameobject.name"  = 1, 2,3 o etc.
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);//desactivar si no es el paso actual
        }

    }
}
