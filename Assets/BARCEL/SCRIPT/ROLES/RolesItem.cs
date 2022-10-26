using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RolesItem : MonoBehaviour
{
    public TextAsset pruebaPostJson;
    public RolesList rolesList = new RolesList();
    public Text AreaText;
    public Text LineaText;
    public Text CelulaText;
    public Text AssetText;
    public Text NombreChecklist;

    string valorActualIdChecklist;
    // Start is called before the first frame update
    void Start()
    {
        rolesList = JsonUtility.FromJson<RolesList>(pruebaPostJson.text);
        Debug.Log(rolesList.Roles.Length);
        for (int i=0; i<rolesList.Roles.Length; i++) {
            if (gameObject.name == i.ToString())
            {

                //var valorOrdenchecklit = int.Parse(gameObject.name);
                //Debug.Log("prueba vamos tu puedes!! " + valorOrdenchecklit);
                AreaText.text   = rolesList.Roles[i].Area;
                LineaText.text  = rolesList.Roles[i].Linea;
                CelulaText.text = rolesList.Roles[i].Celula;
                AssetText.text  = rolesList.Roles[i].Asset;
                NombreChecklist.text = rolesList.Roles[i].NombreChecklist;

                valorActualIdChecklist = rolesList.Roles[i].id_CheckList;
            }
        }
    }

    // Update is called once per frame
    public void getIdChecklist()
    {
       
            PlayerPrefs.SetString("Id_checklist", valorActualIdChecklist);

           string json = File.ReadAllText(Application.persistentDataPath + "/resultadoBD");
        //entries = FileHandler.ReadListFromJSON<InputEntry>(filenameJsonBD);
        //string json = File.ReadAllText(Application.persistentDataPath + "/resultadoBD");

        SceneManager.LoadScene("Checklist");
    }
}
