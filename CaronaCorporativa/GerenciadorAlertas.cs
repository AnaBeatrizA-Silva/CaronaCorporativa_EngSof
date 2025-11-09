using System;
using System.Collections.Generic;
using System.Linq;

public class GerenciadorAlertas
{
    private static List<Alerta> alertas = new List<Alerta>();
    private static int proximoId = 1;

    public static void AdicionarAlerta(string tipoAlerta, string mensagem, string cpfOriginador, string detalhes = "")
    {
        var alerta = new Alerta(proximoId++, tipoAlerta, mensagem, cpfOriginador, detalhes);
        alertas.Add(alerta);
        
        // Log para debug (opcional)
        Console.WriteLine($"🔔 Alerta gerado: {tipoAlerta} - {mensagem}");
    }

    public static List<Alerta> ObterTodosAlertas()
    {
        return alertas.OrderByDescending(a => a.HorarioEnvio).ToList();
    }

    public static List<Alerta> ObterAlertasPendentes()
    {
        return alertas.Where(a => a.Status == "PENDENTE").OrderByDescending(a => a.HorarioEnvio).ToList();
    }

    public static List<Alerta> ObterAlertasPorTipo(string tipoAlerta)
    {
        return alertas.Where(a => a.TipoAlerta.Equals(tipoAlerta, StringComparison.OrdinalIgnoreCase))
                     .OrderByDescending(a => a.HorarioEnvio).ToList();
    }

    public static Alerta? ObterAlertaPorId(int id)
    {
        return alertas.FirstOrDefault(a => a.IdAlerta == id);
    }

    public static int ContarAlertasPendentes()
    {
        return alertas.Count(a => a.Status == "PENDENTE");
    }

    public static int ContarAlertasPorTipo(string tipoAlerta)
    {
        return alertas.Count(a => a.TipoAlerta.Equals(tipoAlerta, StringComparison.OrdinalIgnoreCase));
    }

    public static bool MarcarComoVisualizado(int idAlerta)
    {
        var alerta = ObterAlertaPorId(idAlerta);
        if (alerta != null)
        {
            alerta.MarcarComoVisualizado();
            return true;
        }
        return false;
    }

    public static bool MarcarComoResolvido(int idAlerta)
    {
        var alerta = ObterAlertaPorId(idAlerta);
        if (alerta != null)
        {
            alerta.MarcarComoResolvido();
            return true;
        }
        return false;
    }

    public static void LimparAlertas()
    {
        alertas.Clear();
        proximoId = 1;
    }

    public static void LimparAlertasResolvidos()
    {
        alertas.RemoveAll(a => a.Status == "RESOLVIDO");
    }

    // Métodos específicos para diferentes tipos de alertas
    public static void AlertarCancelamentoCarona(string cpfMotorista, int idSolicitacao, string origemPassageiro, string destinoPassageiro)
    {
        string mensagem = $"Motorista {cpfMotorista} CANCELOU carona da solicitação #{idSolicitacao}";
        string detalhes = $"Rota cancelada: {origemPassageiro} → {destinoPassageiro}. " +
                         "Passageiro pode ter ficado sem transporte.";
        
        AdicionarAlerta("CANCELAMENTO", mensagem, cpfMotorista, detalhes);
    }

    public static void AlertarReembolsoProcessado(string cpfPassageiro, double valorReembolso, string rota)
    {
        string mensagem = $"Reembolso de R$ {valorReembolso:F2} processado para passageiro {cpfPassageiro}";
        string detalhes = $"Rota: {rota}. Reembolso calculado automaticamente.";
        
        AdicionarAlerta("REEMBOLSO", mensagem, cpfPassageiro, detalhes);
    }

    public static void AlertarRotaIncompativel(string cpfPassageiro, string origem, string destino)
    {
        string mensagem = $"Solicitação de rota incompatível: {origem} → {destino}";
        string detalhes = $"Passageiro {cpfPassageiro} tentou solicitar rota que não atende critérios de pareamento.";
        
        AdicionarAlerta("SISTEMA", mensagem, cpfPassageiro, detalhes);
    }

    public static void AlertarVeiculoLotado(string cpfMotorista, string cpfPassageiro)
    {
        string mensagem = $"Tentativa de aceitar passageiro com veículo lotado";
        string detalhes = $"Motorista {cpfMotorista} tentou aceitar passageiro {cpfPassageiro} sem assentos disponíveis.";
        
        AdicionarAlerta("SISTEMA", mensagem, cpfMotorista, detalhes);
    }
}