using System;

public class TesteFormatacao
{
    public static void TestarInterface()
    {
        Tela tela = new Tela();
        
        // Demonstrar tela de cadastro como no problema original
        tela.LimparTela();
        tela.DesenharCabecalho("CADASTRAR MOTORISTA", "Novo Motorista");
        
        // Simular entrada de dados como seria no sistema real
        Console.SetCursorPosition(2, 8);
        Console.WriteLine("=== TESTE DE FORMATACAO ===");
        Console.WriteLine();
        Console.WriteLine("1. Nome completo: João Silva");
        Console.WriteLine("2. CPF (somente numeros): 12345678901");  
        Console.WriteLine("3. Cargo na empresa: Desenvolvedor");
        Console.WriteLine("4. Numero da CNH: ABC123456");
        Console.WriteLine();
        Console.WriteLine("Formatacao corrigida!");
        
        tela.AguardarTecla("Pressione qualquer tecla para testar entrada de dados...");
        
        // Testar sistema de input organizado
        tela.LimparTela();
        tela.DesenharCabecalho("TESTE DE INPUT", "Sistema Organizado");
        
        string nome = tela.LerTexto("Nome completo");
        string cpf = tela.LerTexto("CPF (somente numeros)");
        string cargo = tela.LerTexto("Cargo na empresa");
        string cnh = tela.LerTexto("Numero da CNH");
        
        tela.ExibirSucesso("Dados coletados com sucesso!");
        tela.AguardarTecla();
    }
}