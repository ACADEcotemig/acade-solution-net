namespace AcadeAppWeb.Models;

using System.ComponentModel.DataAnnotations;

public class Trabalhador
{
    [Key]
    public int Id { get; set; }

    public string Nome { get; set; }
    public string Senha { get; set; }
    public string Email { get; set; }
    public string TipoCarro { get; set; }
    public string TipoLixoRecolhido { get; set; }
    public float QtdMaxima { get; set; }
    public string Disponibilidade { get; set; }
    public string HistoricoAtv { get; set; }
    public float Nota { get; set; }
}
