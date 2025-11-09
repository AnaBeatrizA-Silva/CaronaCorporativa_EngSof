using System;

public class Alerta
{
    public int IdAlerta { get; set; }
    public DateTime HorarioEnvio { get; set; }
    public string Destinatario { get; set; }
    public string Mensagem { get; set; }
    public string TipoAlerta { get; set; } // "CANCELAMENTO", "REEMBOLSO", "SISTEMA", etc.
    public string Status { get; set; } // "PENDENTE", "VISUALIZADO", "RESOLVIDO"
    public string CpfOriginador { get; set; } // CPF de quem gerou o alerta
    public string DetalhesAdicionais { get; set; }

    // Construtor padrão
    public Alerta()
    {
        HorarioEnvio = DateTime.Now;
        Status = "PENDENTE";
        Destinatario = "";
        Mensagem = "";
        TipoAlerta = "";
        CpfOriginador = "";
        DetalhesAdicionais = "";
    }

    // Construtor completo
    public Alerta(int idAlerta, string tipoAlerta, string mensagem, string cpfOriginador, string detalhes = "")
    {
        IdAlerta = idAlerta;
        TipoAlerta = tipoAlerta;
        Mensagem = mensagem;
        CpfOriginador = cpfOriginador;
        DetalhesAdicionais = detalhes;
        HorarioEnvio = DateTime.Now;
        Status = "PENDENTE";
        Destinatario = "GESTOR";
    }

    // Métodos
    public void EnviarNotificacao()
    {
        Console.WriteLine($"Alerta #{IdAlerta} enviado para {Destinatario}");
        Console.WriteLine($"Tipo: {TipoAlerta}");
        Console.WriteLine($"Mensagem: {Mensagem}");
        Console.WriteLine($"Horário: {HorarioEnvio:dd/MM/yyyy HH:mm}");
    }

    public string ObterDetalhesFormatados()
    {
        string statusIcon = Status switch
        {
            "PENDENTE" => "🔴",
            "VISUALIZADO" => "🟡", 
            "RESOLVIDO" => "🟢",
            _ => "⚪"
        };

        return $"=== ALERTA #{IdAlerta} ===\n" +
               $"Status: {Status}\n" +
               $"Data/Hora: {HorarioEnvio:dd/MM/yyyy HH:mm}\n" +
               $"Tipo: {TipoAlerta}\n" +
               $"Originado por: {CpfOriginador}\n" +
               $"Mensagem: {Mensagem}\n" +
               (string.IsNullOrWhiteSpace(DetalhesAdicionais) ? "" : $"Detalhes: {DetalhesAdicionais}\n") +
               $"========================";
    }

    public void MarcarComoVisualizado()
    {
        if (Status == "PENDENTE")
        {
            Status = "VISUALIZADO";
        }
    }

    public void MarcarComoResolvido()
    {
        Status = "RESOLVIDO";
    }
}