namespace AcadeAppApi.Models;

using System.ComponentModel.DataAnnotations;

public class Usuario
{
    [Key]
    public int Id { get; set; }

    public string Nome { get; set; }
    public string Senha { get; set; }
    public string Email { get; set; }
    public string Telefone { get; set; }
    public string HistoricoAtv { get; set; }
    public string RelatorioImpacto { get; set; }
    public string InterfacePref { get; set; }
    public string IdiomaPref { get; set; }
    public string Localizacao { get; set; }
    public string PontosColeta { get; set; }
}

