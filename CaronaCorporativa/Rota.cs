using System;

public class Rota
{
    private int idRota;
    private string enderecoPartida;
    private string enderecoFinal;
    private DateTime horarioPartida;
    private double distanciaTotal;
    private Motorista? motorista;
    private PareamentoRota? pareamentoRota;

    // Construtor
    public Rota(int idRota, string enderecoPartida, string enderecoFinal, DateTime horarioPartida, double distanciaTotal)
    {
        this.idRota = idRota;
        this.enderecoPartida = enderecoPartida;
        this.enderecoFinal = enderecoFinal;
        this.horarioPartida = horarioPartida;
        this.distanciaTotal = distanciaTotal;
    }

    // Metodos
    public double CalcularDistancia()
    {
        // Implementacao do calculo de distancia
        Console.WriteLine($"Calculando distancia da rota {idRota}");
        // Logica para calcular a distancia real
        return distanciaTotal;
    }

    public bool VerificarDestinoFinal()
    {
        if (string.IsNullOrWhiteSpace(enderecoFinal))
        {
            Console.WriteLine("Endereco final nao definido");
            return false;
        }
        Console.WriteLine($"Destino final verificado: {enderecoFinal}");
        return true;
    }
}