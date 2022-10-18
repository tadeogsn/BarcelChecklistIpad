using System;

[Serializable]
public class InputEntry
{
    public string idDeProcedimiento;
    public string OrdenChecklist;
    public string Results;
    public string UrlServer;
    public string Notas;

    public InputEntry(string ordenChecklist, string result, string urlServer, string notas, string IdDeProcedimiento)
    {
        OrdenChecklist = ordenChecklist;
        Results = result;
        UrlServer = urlServer;
        Notas = notas;
        idDeProcedimiento = IdDeProcedimiento;
    }
}