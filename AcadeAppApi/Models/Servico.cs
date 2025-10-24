namespace AcadeAppApi.Models;
using System.ComponentModel.DataAnnotations;

public class Servico
{
    [Key]
    public int Id { get; set; } // EF Core needs a primary key

    public float QtdLixo { get; set; }
    public int Tempo { get; set; }
    public DateTime DataColeta { get; set; } // changed from `date` to `DateTime`
    public float QtdMaxima { get; set; }
    public string TipoLixo { get; set; }
    public string Disponibilidade { get; set; }
}
