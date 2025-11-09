using System;

public class Rota
{
    public int IdRota = 0;
    public string EnderecoPartida = "";
    public string EnderecoFinal = "";
    public DateTime HorarioPartida = DateTime.Now;
    public double DistanciaTotal = 0.0;
    public string CpfMotorista = ""; // Para identificar o dono da rota

    // Construtor simples
    public Rota()
    {
    }

    // Construtor com parâmetros
    public Rota(int idRota, string enderecoPartida, string enderecoFinal, DateTime horarioPartida, double distanciaTotal, string cpfMotorista = "")
    {
        this.IdRota = idRota;
        this.EnderecoPartida = enderecoPartida;
        this.EnderecoFinal = enderecoFinal;
        this.HorarioPartida = horarioPartida;
        this.DistanciaTotal = distanciaTotal;
        this.CpfMotorista = cpfMotorista;
    }

    // Metodos simples
    public double CalcularDistancia()
    {
        return DistanciaTotal;
    }

    public bool VerificarDestinoFinal()
    {
        if (string.IsNullOrWhiteSpace(EnderecoFinal))
        {
            // Endereço final não definido - retorna false sem imprimir
            return false;
        }
        // Destino válido - retorna true sem imprimir
        return true;
    }

    // Retorna string formatada em vez de imprimir diretamente
    public string ObterDetalhesFormatados()
    {
        return $"=== ROTA #{IdRota} ===\n" +
               $"Origem: {EnderecoPartida}\n" +
               $"Destino: {EnderecoFinal}\n" +
               $"Horario: {HorarioPartida:dd/MM/yyyy HH:mm}\n" +
               $"Distancia: {DistanciaTotal:F1} km\n" +
               $"Motorista: {CpfMotorista}\n" +
               $"=====================";
    }
    
    // Método legado mantido para compatibilidade
    public void ExibirDetalhes()
    {
        Console.WriteLine(ObterDetalhesFormatados());
    }
}