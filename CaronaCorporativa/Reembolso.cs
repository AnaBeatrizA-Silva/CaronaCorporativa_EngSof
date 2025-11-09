using System;

public class Reembolso
{
    private int idReembolso;
    private DateTime dataEmissao;
    private decimal valor;

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
        // Cálculo sem output direto - interface controlaria logging
        // Logica para calcular o valor baseado na reserva
        return valor;
    }

    public void EmitirReembolso()
    {
        // Emissão do reembolso sem output direto
        // Interface controlaria a exibição das informações
        // Implementacao da emissao do reembolso
    }
    
    // Método para obter informações formatadas se necessário
    public string ObterDetalhesFormatados()
    {
        return $"Reembolso #{idReembolso}\n" +
               $"Valor: R$ {valor:F2}\n" +
               $"Data de emissao: {dataEmissao:dd/MM/yyyy}";
    }
}