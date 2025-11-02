using System;

// Ainda falta pensar como implementar o alerta; 
public class Alerta
{
    private int idAlerta;
    private DateTime horarioEnvio;
    private string destinatario;
    private string mensagem;
    private Passageiro? passageiro;
    private Reserva? reserva;

    // Construtor
    public Alerta(int idAlerta, DateTime horarioEnvio, string destinatario, string mensagem)
    {
        this.idAlerta = idAlerta;
        this.horarioEnvio = horarioEnvio;
        this.destinatario = destinatario;
        this.mensagem = mensagem;
    }

    // Metodos
    public void EnviarNotificacao()
    {
        Console.WriteLine($"Enviando alerta {idAlerta} para {destinatario}");
        Console.WriteLine($"Mensagem: {mensagem}");
        Console.WriteLine($"Horario: {horarioEnvio}");
        // Implementacao do envio da notificacao
    }
}