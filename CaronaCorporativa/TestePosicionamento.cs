using System;

public class TestePosicionamento
{
    public static void TestarFormatacao()
    {
        Tela tela = new Tela();
        
        Console.WriteLine("=== TESTE DE POSICIONAMENTO CORRIGIDO ===");
        Console.WriteLine();
        
        // Teste 1: Tela de alteração de cadastro simulada
        tela.LimparTela();
        tela.DesenharCabecalho("ALTERAR CADASTRO", "Atualizacao de Dados");
        
        // Simular dados atuais corretamente posicionados
        Console.SetCursorPosition(2, 8);
        Console.WriteLine("Dados atuais do motorista:");
        Console.WriteLine("Nome: Pedro Silva");
        Console.WriteLine("CPF: 123456789");
        Console.WriteLine("Cargo: Desenvolvedor");
        Console.WriteLine("CNH: 123456789");
        Console.WriteLine(); // Linha em branco
        Console.WriteLine(); // Segunda linha em branco
        
        // Define posição correta para inputs
        tela.DefinirProximaLinhaInput(16);
        
        // Simular entradas organizadas
        Console.SetCursorPosition(2, 16);
        Console.WriteLine("Novo nome (Enter para manter: Pedro Silva): [INPUT AQUI]");
        Console.SetCursorPosition(2, 17);
        Console.WriteLine("Novo cargo (Enter para manter: Desenvolvedor): [INPUT AQUI]");
        Console.SetCursorPosition(2 , 18);
        Console.WriteLine("Nova CNH (Enter para manter: 123456789): [INPUT AQUI]");
        
        Console.SetCursorPosition(2, 22);
        Console.WriteLine("Alteração realizada com sucesso!");
        Console.WriteLine();
        Console.SetCursorPosition(2, Console.WindowHeight - 2);
        Console.WriteLine("Pressione qualquer tecla para continuar...");
        Console.ReadKey(true);
        
        // Teste 2: Uso do novo método ExibirInformacoes
        tela.LimparTela();
        tela.DesenharCabecalho("TESTE NOVO METODO", "ExibirInformacoes");
        
        tela.ExibirInformacoes("=== INFORMACOES DE TESTE ===",
            "Linha 1: Teste de posicionamento",
            "Linha 2: Formatacao automatica",
            "Linha 3: Espacamento controlado");
            
        Console.WriteLine("✅ Método ExibirInformacoes funcionando!");
        
        Console.SetCursorPosition(2, Console.WindowHeight - 2);
        Console.WriteLine("Teste concluido! Pressione qualquer tecla...");
        Console.ReadKey(true);
    }
}