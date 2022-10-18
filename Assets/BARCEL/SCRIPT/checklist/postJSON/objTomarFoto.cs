using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class objTomarFoto
{
    //estas variables deben de coincidir con el JSON que se envian desde el servidor(php)
    public byte[] foto;
    public string HoraActualNombre;

    ///ESTE LO USAREMOS DE FORMA LOCAL PARA GUARDAR LAS FOTOS Y DESPUES ALMACENARLAS EN UN JSON
    public objTomarFoto(byte[] Foto, string horaActualNombre)
    {
        foto = Foto;
        HoraActualNombre = horaActualNombre;

    }
}

///ESTE LO USAREMOS PARA ENVIAR EL JSON AL SERVIDOR Y FUNCIONA COMO FORMATO
[System.Serializable]
public class sendServerObjFoto
{
    public objTomarFoto[] Results;
}
