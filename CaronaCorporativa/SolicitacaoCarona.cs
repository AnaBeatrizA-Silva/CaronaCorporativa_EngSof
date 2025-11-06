using System;

public class SolicitacaoCarona
{
    public int Id { get; set; }
    public string CpfPassageiro { get; set; }
    public string EnderecoOrigem { get; set; }
    public string EnderecoDestino { get; set; }
    public DateTime DataSolicitacao { get; set; }
    public double DistanciaKm { get; set; }
    public string Status { get; set; } // "Pendente", "Aceita", "Rejeitada"

    public SolicitacaoCarona()
    {
        DataSolicitacao = DateTime.Now;
        Status = "Pendente";
        CpfPassageiro = "";
        EnderecoOrigem = "";
        EnderecoDestino = "";
    }

    public SolicitacaoCarona(int id, string cpfPassageiro, string origem, string destino, double distancia)
    {
        Id = id;
        CpfPassageiro = cpfPassageiro;
        EnderecoOrigem = origem;
        EnderecoDestino = destino;
        DistanciaKm = distancia;
        DataSolicitacao = DateTime.Now;
        Status = "Pendente";
    }

    // Retorna string formatada em vez de imprimir diretamente
    public string ObterDetalhesFormatados()
    {
        return $"=== SOLICITACAO DE CARONA #{Id} ===\n" +
               $"Passageiro: {CpfPassageiro}\n" +
               $"Origem: {EnderecoOrigem}\n" +
               $"Destino: {EnderecoDestino}\n" +
               $"Distancia: {DistanciaKm:F1} km\n" +
               $"Data/Hora: {DataSolicitacao:dd/MM/yyyy HH:mm}\n" +
               $"Status: {Status}\n" +
               $"================================";
    }
    
    // Método legado mantido para compatibilidade, mas agora controlado
    public void ExibirDetalhes()
    {
        Console.WriteLine(ObterDetalhesFormatados());
    }
}