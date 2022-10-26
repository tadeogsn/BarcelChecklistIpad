using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class objRoles
{
    //estas variables deben de coincidir con el JSON que se envian desde el servidor(php)
    public string FehcaInspeccion;
    public string Area;
    public string Linea;
    public string Celula;
    public string Asset;
    public string NombreChecklist;
    public string id_CheckList;
    public string Id_Usuario;
    
}

[System.Serializable]
public class RolesList
{
    public objRoles[] Roles;
}
