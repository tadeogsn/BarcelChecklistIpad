[System.Serializable]
public class objPuntosDeinspeccion
{
    //estas variables deben de coincidir con el JSON que se envian desde el servidor(php)
    public string Orden;
    public string Titulo;
    public string Descripcion;
    public string MaxParemetros;
    public string Nombre;//este valor viene de la tabla de checklist 
    public ChecklistList[] CheckList;
}

/// CHECKLIST PARA LOS OBJETOS DE REALIDAD AUMENTADA////////////////////////////////////////////////////////////////////////////////
[System.Serializable]
public class ChecklistList
{
    //estas variables deben de coincidir con el JSON que se envian desde el servidor(php)
    public string Nombre;
    public string Descripcion;
}

[System.Serializable]
public class PuntosDeInspeccionList{
    public objPuntosDeinspeccion[] PuntosDeInspeccion;
}
