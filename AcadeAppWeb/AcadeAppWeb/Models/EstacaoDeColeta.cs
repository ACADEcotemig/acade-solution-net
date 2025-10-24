namespace AcadeAppWeb.Models;

using System.ComponentModel.DataAnnotations;

public class EstacaoDeColeta
{
    [Key]
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Localizacao { get; set; }
    public string LinkMaps { get; set; }
}