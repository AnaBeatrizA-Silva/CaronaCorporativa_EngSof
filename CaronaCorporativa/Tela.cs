using System;
using System.Collections.Generic;

public class Tela
{
    // Propriedades para configuracao da tela
    private int largura, altura;
    private ConsoleColor corTexto, corFundo;
    
    // Construtor padrao
    public Tela()
    {
        this.largura = Math.Min(80, Console.WindowWidth - 1);
        this.altura = Math.Min(25, Console.WindowHeight - 1);
        this.corTexto = ConsoleColor.Green;
        this.corFundo = ConsoleColor.Black;
    }



    // ===============================
    // MÉTODOS DE UI BASE (Ex-TelaAvancada)
    // ===============================



    // Limpa uma area especifica da tela
    public void LimparArea(int colIni, int linIni, int colFin, int linFin)
    {
        for (int x = colIni; x <= colFin; x++)
        {
            for (int y = linIni; y <= linFin; y++)
            {
                Console.SetCursorPosition(x, y);
                Console.Write(" ");
            }
        }
    }

    // Monta moldura com caracteres Unicode elegantes
    public void MontarMoldura(int colIni, int linIni, int colFin, int linFin)
    {
        this.LimparArea(colIni, linIni, colFin, linFin);
        
        // Desenha as linhas horizontais (superior e inferior)
        for (int coluna = colIni; coluna <= colFin; coluna++)
        {
            // Linha superior
            Console.SetCursorPosition(coluna, linIni);
            Console.Write("═");

            // Linha inferior
            Console.SetCursorPosition(coluna, linFin);
            Console.Write("═");
        }

        // Desenha as linhas verticais (esquerda e direita)
        for (int linha = linIni; linha <= linFin; linha++)
        {
            // Linha esquerda
            Console.SetCursorPosition(colIni, linha);
            Console.Write("║");

            // Linha direita
            Console.SetCursorPosition(colFin, linha);
            Console.Write("║");
        }

        // Desenha os cantos da moldura
        Console.SetCursorPosition(colIni, linIni);  // Canto superior esquerdo
        Console.Write("╔");
        
        Console.SetCursorPosition(colIni, linFin);  // Canto inferior esquerdo
        Console.Write("╚");
        
        Console.SetCursorPosition(colFin, linIni);  // Canto superior direito
        Console.Write("╗");
        
        Console.SetCursorPosition(colFin, linFin);  // Canto inferior direito
        Console.Write("╝");
    }

    // Monta uma tela completa com titulo e conteudo


    // Exibe menu com opcoes e retorna a opcao escolhida
    public string MostrarMenu(List<string> opcoes, int col, int lin, string titulo = "")
    {
        string opcaoEscolhida = "";
        
        // Calcula dimensoes automaticamente
        int maiorLinha = titulo.Length;
        foreach (string opcao in opcoes)
        {
            if (opcao.Length > maiorLinha)
                maiorLinha = opcao.Length;
        }
        
        int colFin = col + maiorLinha + 4;
        int linFin = lin + opcoes.Count + 4;
        
        // Se tem titulo, adiciona mais espaco
        if (!string.IsNullOrEmpty(titulo))
            linFin += 2;

        this.MontarMoldura(col, lin, colFin, linFin);

        col++;
        lin++;

        // Exibe titulo se fornecido
        if (!string.IsNullOrEmpty(titulo))
        {
            Console.SetCursorPosition(col + 1, lin);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(titulo);
            Console.ForegroundColor = this.corTexto;
            lin += 2;
        }

        // Exibe opcoes do menu
        for (int i = 0; i < opcoes.Count; i++)
        {
            Console.SetCursorPosition(col + 1, lin);
            Console.Write(opcoes[i]);
            lin++;
        }

        // Campo para entrada da opcao
        Console.SetCursorPosition(col + 1, lin);
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("Opcao: ");
        Console.ForegroundColor = this.corTexto;
        opcaoEscolhida = Console.ReadLine() ?? "";

        return opcaoEscolhida;
    }



    // Faz uma pergunta e retorna a resposta
    // Exibe mensagem de erro
    public void ExibirErro(string erro, int col = 2, int lin = -1)
    {
        if (lin == -1)
        {
            // Usa uma linha após a área de input, mas garantindo espaço
            lin = Math.Max(proximaLinhaInput + 2, Console.WindowHeight - 6);
        }
            
        if (lin >= Console.WindowHeight)
            lin = Console.WindowHeight - 1;
            
        // Limpa a linha inteira antes de escrever
        Console.SetCursorPosition(0, lin);
        Console.Write(new string(' ', Console.WindowWidth - 1));
        
        Console.SetCursorPosition(col, lin);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write($"ERRO: {erro}");
        Console.ForegroundColor = this.corTexto;
    }

    // Exibe mensagem de sucesso
    public void ExibirSucesso(string mensagem, int col = 2, int lin = -1)
    {
        if (lin == -1)
        {
            // Usa uma linha após a área de input, mas garantindo espaço
            lin = Math.Max(proximaLinhaInput + 2, Console.WindowHeight - 6);
        }
            
        if (lin >= Console.WindowHeight)
            lin = Console.WindowHeight - 1;

        // Limpa a linha antes de escrever
        Console.SetCursorPosition(0, lin);
        Console.Write(new string(' ', Console.WindowWidth - 1));
        
        Console.SetCursorPosition(col, lin);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write($"SUCESSO: {mensagem}");
        Console.ForegroundColor = this.corTexto;
    }

    // Aguarda confirmacao do usuario (S/N)
    public bool ConfirmarAcao(string mensagem, int col = 2, int lin = -1)
    {
        if (lin == -1)
        {
            // Usa a próxima linha disponível para input
            lin = proximaLinhaInput + 1;
            if (lin >= Console.WindowHeight - 2)
                lin = Console.WindowHeight - 3;
        }
            
        if (lin >= Console.WindowHeight)
            lin = Console.WindowHeight - 1;
            
        // Limpa a linha
        Console.SetCursorPosition(0, lin);
        Console.Write(new string(' ', Console.WindowWidth - 1));
        
        Console.SetCursorPosition(col, lin);
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write($"{mensagem} (S/N): ");
        Console.ForegroundColor = this.corTexto;
        
        string resposta = Console.ReadLine()?.ToUpper() ?? "";
        return resposta == "S" || resposta == "SIM";
    }

    // Aguarda uma tecla para continuar
    public void AguardarTecla(string mensagem = "Pressione qualquer tecla para continuar...", int col = 2, int lin = -1)
    {
        if (lin == -1)
        {
            // Sempre usa a última linha da tela para aguardar tecla
            lin = Console.WindowHeight - 2;
        }

        if (lin >= Console.WindowHeight)
            lin = Console.WindowHeight - 1;
            
        // Limpa a tela
        Console.SetCursorPosition(0, lin);
        Console.Write(new string(' ', Console.WindowWidth - 1));
        
        Console.SetCursorPosition(col, lin);
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.Write(mensagem);
        Console.ForegroundColor = this.corTexto;
        Console.ReadKey(true);
    }

    // Monta cabeçalho do sistema
    public void DesenharCabecalho(string titulo, string subtitulo = "")
    {
        int larguraCabecalho = 80;
        int col = (Console.WindowWidth - larguraCabecalho) / 2;
        if (col < 0) col = 0;
        
        this.MontarMoldura(col, 1, col + larguraCabecalho - 1, 5);

        // Posição do cabeçalho
        int posicaoTitulo = col + (larguraCabecalho - titulo.Length) / 2;
        Console.SetCursorPosition(posicaoTitulo, 2);
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write(titulo);

        if (!string.IsNullOrEmpty(subtitulo))
        {
            int posicaoSubtitulo = col + (larguraCabecalho - subtitulo.Length) / 2;
            Console.SetCursorPosition(posicaoSubtitulo, 3);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(subtitulo);
        }
        
        Console.ForegroundColor = this.corTexto;
        ResetarPosicaoInput(); // Reseta posição após desenhar cabeçalho
    }

    // Limpa tela
    public void LimparTela()
    {
        Console.Clear();
        Console.BackgroundColor = this.corFundo;
        Console.ForegroundColor = this.corTexto;
        ResetarPosicaoInput(); // Reseta posição de input para nova tela
    }

   // Pega posição conforme tamanho do CMD
    public int ObterLinhaSegura(int linhaDesejada)
    {
        int alturaMaxima = Console.WindowHeight - 1;
        return Math.Min(linhaDesejada, alturaMaxima);
    }

    public int ObterColunaSegura(int colunaDesejada)
    {
        int larguraMaxima = Console.WindowWidth - 1;
        return Math.Min(colunaDesejada, larguraMaxima);
    }

    public void ExibirMenuPrincipal()
    {
        LimparTela();
        DesenharCabecalho("CARONA CORPORATIVA", "Sistema de Gestao de Caronas");

        List<string> opcoes = new List<string>
        {
            "1 - GESTOR",
            "2 - MOTORISTA", 
            "3 - PASSAGEIRO",
            "0 - Sair do Sistema"
        };

        MostrarMenu(opcoes, 10, 8, "Selecione o tipo de usuario:");
    }

    public string ExibirMenuPrincipalComRetorno()
    {
        LimparTela();
        DesenharCabecalho("CARONA CORPORATIVA", "Sistema de Gestao de Caronas");

        List<string> opcoes = new List<string>
        {
            "1 - GESTOR",
            "2 - MOTORISTA",
            "3 - PASSAGEIRO",
            "0 - Sair do Sistema"
        };

        return MostrarMenu(opcoes, 10, 8, "Selecione o tipo de usuario:");
    }

    public void ExibirMenuGestor()
    {
        LimparTela();
        DesenharCabecalho("PAINEL DO GESTOR", "Gerenciamento de Usuarios");

        List<string> opcoes = new List<string>
        {
            "1 - Listar Motoristas",
            "2 - Listar Passageiros", 
            "3 - Gerenciar Alertas",
            "4 - Gerenciar Reembolsos",
            "5 - Relatorios Gerenciais",
            "0 - Voltar ao Menu Principal"
        };

        MostrarMenu(opcoes, 10, 8, "Selecione uma opcao:");
    }

    public string ExibirMenuGestorComRetorno()
    {
        LimparTela();
        DesenharCabecalho("PAINEL DO GESTOR", "Gerenciamento de Usuarios");

        List<string> opcoes = new List<string>
        {
            "1 - Listar Motoristas",
            "2 - Listar Passageiros", 
            "3 - Gerenciar Alertas",
            "4 - Gerenciar Reembolsos",
            "5 - Relatorios Gerenciais",
            "0 - Voltar ao Menu Principal"
        };

        return MostrarMenu(opcoes, 10, 8, "Selecione uma opcao:");
    }





    // Métodos de entrada de dados
    private int proximaLinhaInput = 8; // Começa após o cabeçalho
    
    public string LerTexto(string prompt)
    {
        // Garante que estamos em uma posição segura da tela
        if (proximaLinhaInput >= Console.WindowHeight - 3)
            proximaLinhaInput = 8; // Reinicia se chegou no final da tela
            
        Console.SetCursorPosition(2, proximaLinhaInput);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write($"{prompt}: ");
        Console.ForegroundColor = this.corTexto;
        
        string resposta = Console.ReadLine() ?? "";
        proximaLinhaInput++; // Próxima entrada será na linha seguinte
        
        return resposta;
    }
    
    // Define a posição da próxima linha de input
    public void DefinirProximaLinhaInput(int linha)
    {
        proximaLinhaInput = linha;
    }
    
    // Exibe lista de bairros disponíveis
    public void ExibirBairrosDisponiveis()
    {
        GerenciadorBairros gerenciador = new GerenciadorBairros();
        var bairros = gerenciador.ObterBairrosDisponiveis();
        
        Console.WriteLine("BAIRROS DISPONÍVEIS:");
        for (int i = 0; i < bairros.Count && i < 10; i++) // Mostrar apenas os primeiros 10
        {
            Console.WriteLine($"• {bairros[i]}");
        }
        if (bairros.Count > 10)
        {
            Console.WriteLine($"... e mais {bairros.Count - 10} bairros");
        }
        Console.WriteLine();
    }
    
    // Método para resetar a posição de input (usar no início de novas telas)
    public void ResetarPosicaoInput()
    {
        proximaLinhaInput = 8;
    }





    public void ExibirMensagem(string mensagem, bool aguardar = true)
    {
        int linhaSeg = ObterLinhaSegura(Console.WindowHeight - 6);
        Console.SetCursorPosition(2, linhaSeg);
        Console.Write(mensagem);
        
        if (aguardar)
        {
            AguardarTecla();
        }
    }
    
    // Posiciona cursor para área de conteúdo (após cabeçalho)
    public void PosicionarParaConteudo()
    {
        Console.SetCursorPosition(2, 8);
        proximaLinhaInput = 8; // Reset da posição de input também
    }
    
    // Exibe informações formatadas com posicionamento controlado
    public void ExibirInformacoes(string titulo, params string[] linhas)
    {
        Console.SetCursorPosition(2, 8);
        Console.WriteLine(titulo);
        
        foreach (string linha in linhas)
        {
            Console.WriteLine(linha);
        }
        
        Console.WriteLine(); // Linha em branco
        Console.WriteLine(); // Segunda linha em branco para espaçamento
        
        // Calcula próxima posição de input
        int proximaPosicao = 8 + linhas.Length + 4; // título + linhas + 2 espaços + margem
        DefinirProximaLinhaInput(proximaPosicao);
    }
}