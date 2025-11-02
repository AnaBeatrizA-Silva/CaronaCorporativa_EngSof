using System;

public class PareamentoRota
{
    private int idPareamento;
    private string nivelCompatibilidade;
    private Rota? rota;

    // Construtor
    public PareamentoRota(int idPareamento, string nivelCompatibilidade)
    {
        this.idPareamento = idPareamento;
        this.nivelCompatibilidade = nivelCompatibilidade;
    }

    // Metodos
    public void CompararRotas()
    {
        Console.WriteLine($"Comparando rotas para pareamento {idPareamento}");
        // Implementacao da comparacao de rotas
    }

    public void SugerirPareamento()
    {
        Console.WriteLine($"Sugerindo pareamento com nivel de compatibilidade: {nivelCompatibilidade}");
        // Implementacao da sugestao de pareamento
    }
}