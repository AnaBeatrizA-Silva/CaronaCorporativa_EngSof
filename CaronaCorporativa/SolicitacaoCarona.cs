using System;

public class SolicitacaoCarona
{
    public int Id { get; set; }
    public string CpfPassageiro { get; set; }
    public string EnderecoOrigem { get; set; }
    public string EnderecoDestino { get; set; }
    public DateTime DataSolicitacao { get; set; }
    public double DistanciaKm { get; set; }
    public string Status { get; set; } // "Pendente", "Aceita", "Check-in Feito", "Finalizada", "Rejeitada"
    public DateTime? DataCheckIn { get; set; } // Data/hora do check-in do passageiro
    public DateTime? DataCheckOut { get; set; } // Data/hora do check-out do motorista
    public string? CpfMotorista { get; set; } // CPF do motorista que aceitou a carona

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
        string detalhes = $"=== SOLICITACAO DE CARONA #{Id} ===\n" +
                         $"Passageiro: {CpfPassageiro}\n" +
                         $"Origem: {EnderecoOrigem}\n" +
                         $"Destino: {EnderecoDestino}\n" +
                         $"Distancia: {DistanciaKm:F1} km\n" +
                         $"Data/Hora: {DataSolicitacao:dd/MM/yyyy HH:mm}\n" +
                         $"Status: {Status}\n";

        if (!string.IsNullOrEmpty(CpfMotorista))
        {
            detalhes += $"Motorista: {CpfMotorista}\n";
        }

        if (DataCheckIn.HasValue)
        {
            detalhes += $"Check-in: {DataCheckIn.Value:dd/MM/yyyy HH:mm}\n";
        }

        if (DataCheckOut.HasValue)
        {
            detalhes += $"Check-out: {DataCheckOut.Value:dd/MM/yyyy HH:mm}\n";
        }

        detalhes += "================================";
        return detalhes;
    }
    
    // Método legado mantido para compatibilidade, mas agora controlado
    public void ExibirDetalhes()
    {
        Console.WriteLine(ObterDetalhesFormatados());
    }
}