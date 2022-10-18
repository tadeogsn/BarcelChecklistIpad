using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[System.Serializable]
public class objCheckList
{
    //estas variables deben de coincidir con el JSON que se envian desde el servidor(php)
    public string IdDeProcedimiento;
    public string ordenChecklist;
    public string tituloChecklist;
    public string description;
    public string NombreProcedimiento;
    public string MaxParemetros;
    public objCheckListRA[] objCheckListRA;
}

/// CHECKLIST PARA LOS OBJETOS DE REALIDAD AUMENTADA////////////////////////////////////////////////////////////////////////////////
[System.Serializable]
public class objCheckListRA
{
    //estas variables deben de coincidir con el JSON que se envian desde el servidor(php)
    public string NombreAsset;
    public string UrlServer;
    public string Tipo;
    public string OrdenChecklist;
}

[System.Serializable]
public class ChecklistList
{   
    public objCheckList[] Checklist;
}




//[System.Serializable]
//public class checklistAssetRA
//{
//    public objCheckListRA[] objCheckListRA;
//}


