using System;

public class Reembolso
{
    private int idReembolso;
    private DateTime dataEmissao;
    private decimal valor;
    private Reserva? reserva;

    // Construtor
    public Reembolso(int idReembolso, DateTime dataEmissao, decimal valor)
    {
        this.idReembolso = idReembolso;
        this.dataEmissao = dataEmissao;
        this.valor = valor;
    }

    // Metodos
    public decimal CalcularValor()
    {
        // Implementacao do calculo do valor do reembolso
        Console.WriteLine($"Calculando valor do reembolso {idReembolso}");
        // Logica para calcular o valor baseado na reserva
        return valor;
    }

    public void EmitirReembolso()
    {
        Console.WriteLine($"Emitindo reembolso {idReembolso}");
        Console.WriteLine($"Valor: R$ {valor:F2}");
        Console.WriteLine($"Data de emissao: {dataEmissao}");
        // Implementacao da emissao do reembolso
    }
}