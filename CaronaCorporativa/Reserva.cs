using System;
using System.Collections.Generic;

public class Reserva
{
    private int idReserva;
    private string status;
    private DateTime dataReserva;
    private Passageiro? passageiro;
    private Veiculo? veiculo;
    private Reembolso? reembolso;
    private List<Alerta> alertas;

    // Construtor
    public Reserva(int idReserva, string status, DateTime dataReserva)
    {
        this.idReserva = idReserva;
        this.status = status;
        this.dataReserva = dataReserva;
        this.alertas = new List<Alerta>();
    }

    // Metodos
    public void ConfirmarReserva()
    {
        status = "Confirmada";
        Console.WriteLine($"Reserva {idReserva} confirmada em {DateTime.Now}");
        // Implementacao da confirmacao
    }

    public void CancelarReserva()
    {
        status = "Cancelada";
        Console.WriteLine($"Reserva {idReserva} cancelada em {DateTime.Now}");
        // Implementacao do cancelamento
    }
}