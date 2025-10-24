namespace AcadeAppWeb.Models;

using System;
using System.ComponentModel.DataAnnotations;

public class Servico
{
    [Key]
    public int Id { get; set; }

    public float QtdLixo { get; set; }
    public int Tempo { get; set; }
    public DateTime DataColeta { get; set; }
    public float QtdMaxima { get; set; }
    public string TipoLixo { get; set; }
    public string Disponibilidade { get; set; }
}