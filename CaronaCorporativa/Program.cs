using System;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Configurando o console
            Console.Title = "Sistema de Carona Corporativa";
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            
            // Verifica tamanho minimo do console
            if (Console.WindowHeight < 15 || Console.WindowWidth < 50)
            {
                Console.WriteLine("AVISO: Console muito pequeno. Recomenda-se redimensionar para melhor visualizacao.");
                Console.WriteLine("Pressione qualquer tecla para continuar...");
                Console.ReadKey();
            }
            
            // Iniciando o sistema com gerenciamento por perfis
            GerenciadorSistema gerenciador = new GerenciadorSistema();
            gerenciador.IniciarSistema();
        }
        catch (Exception ex)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("ERRO CRITICO NO SISTEMA:");
            Console.WriteLine(ex.Message);
            Console.ResetColor();
            Console.WriteLine("\nPressione qualquer tecla para sair...");
            Console.ReadKey();
        }
    }
}