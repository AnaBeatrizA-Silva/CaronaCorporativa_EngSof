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

    // Construtor com dimensoes personalizadas
    public Tela(int largura, int altura)
    {
        this.largura = largura;
        this.altura = altura;
        this.corTexto = ConsoleColor.Green;
        this.corFundo = ConsoleColor.Black;
    }

    // Construtor completo com cores personalizadas
    public Tela(int largura, int altura, ConsoleColor corTexto, ConsoleColor corFundo)
    {
        this.largura = largura;
        this.altura = altura;
        this.corTexto = corTexto;
        this.corFundo = corFundo;
    }

    // ===============================
    // MÉTODOS DE UI BASE (Ex-TelaAvancada)
    // ===============================

    // Prepara a tela com cores e limpa o console
    public void PrepararTela()
    {
        Console.BackgroundColor = this.corFundo;
        Console.ForegroundColor = this.corTexto;
        Console.Clear();
    }

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
    public void MontarTela(int col, int lin, List<string> dados, string titulo)
    {
        int larguraTela = Math.Max(titulo.Length + 4, this.largura);
        int alturaTela = dados.Count + 4;
        
        this.MontarMoldura(col, lin, col + larguraTela, lin + alturaTela);

        // Posiciona e escreve o titulo
        col++;
        lin++;
        Console.SetCursorPosition(col + 1, lin);
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write(titulo);
        Console.ForegroundColor = this.corTexto;
        
        lin += 2; // Pula linha após titulo
        
        // Escreve cada linha de dados
        foreach (string linha in dados)
        {
            Console.SetCursorPosition(col + 1, lin);
            Console.Write(linha);
            lin++;
        }
    }

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

    // Mostra uma mensagem em posicao especifica
    public void MostrarMensagem(int col, int lin, string mensagem, bool limparLinha = false)
    {
        if (limparLinha)
        {
            // Limpa a linha antes de escrever
            Console.SetCursorPosition(0, lin);
            Console.Write(new string(' ', Console.WindowWidth - 1));
        }
        
        Console.SetCursorPosition(col, lin);
        Console.Write(mensagem);
    }

    // Faz uma pergunta e retorna a resposta
    public string Perguntar(int col, int lin, string pergunta)
    {
        Console.SetCursorPosition(col, lin);
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write($"{pergunta}: ");
        Console.ForegroundColor = this.corTexto;
        string resposta = Console.ReadLine() ?? "";
        return resposta;
    }

    // Exibe mensagem de erro
    public void ExibirErro(string erro, int col = 2, int lin = -1)
    {
        if (lin == -1)
            lin = Console.WindowHeight - 4;
            
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
            lin = Console.WindowHeight - 4;
            
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
            lin = Console.WindowHeight - 3;
            
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
            lin = Console.WindowHeight - 2;

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
    }

    // Limpa tela
    public void LimparTela()
    {
        Console.Clear();
        Console.BackgroundColor = this.corFundo;
        Console.ForegroundColor = this.corTexto;
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
        DesenharCabecalho("PAINEL DO GESTOR", "Gerenciamento Completo do Sistema");

        List<string> opcoes = new List<string>
        {
            "1 - Gerenciar Motoristas",
            "2 - Gerenciar Passageiros", 
            "3 - Gerenciar Veiculos",
            "4 - Gerenciar Rotas",
            "5 - Gerenciar Reservas",
            "6 - Gerenciar Alertas",
            "7 - Gerenciar Reembolsos",
            "8 - Relatorios Gerenciais",
            "0 - Voltar ao Menu Principal"
        };

        MostrarMenu(opcoes, 10, 8, "Selecione uma opcao:");
    }

    public string ExibirMenuGestorComRetorno()
    {
        LimparTela();
        DesenharCabecalho("PAINEL DO GESTOR", "Gerenciamento Completo do Sistema");

        List<string> opcoes = new List<string>
        {
            "1 - Gerenciar Motoristas",
            "2 - Gerenciar Passageiros", 
            "3 - Gerenciar Veiculos",
            "4 - Gerenciar Rotas",
            "5 - Gerenciar Reservas",
            "6 - Gerenciar Alertas",
            "7 - Gerenciar Reembolsos",
            "8 - Relatorios Gerenciais",
            "0 - Voltar ao Menu Principal"
        };

        return MostrarMenu(opcoes, 10, 8, "Selecione uma opcao:");
    }

    public void ExibirMenuMotoristaLogado()
    {
        LimparTela();
        DesenharCabecalho("PAINEL DO MOTORISTA", "Suas opcoes disponiveis");

        List<string> opcoes = new List<string>
        {
            "1 - Meus Dados Pessoais",
            "2 - Meu Veiculo",
            "3 - Cadastrar Nova Rota",
            "4 - Minhas Rotas",
            "5 - Reservas do Meu Veiculo",
            "6 - Emitir Reembolso",
            "7 - Validar minha CNH",
            "8 - Historico de Viagens",
            "0 - Voltar ao Menu Principal"
        };

        MostrarMenu(opcoes, 10, 8, "Selecione uma opcao:");
    }

    public string ExibirMenuMotoristaLogadoComRetorno()
    {
        LimparTela();
        DesenharCabecalho("PAINEL DO MOTORISTA", "Suas opcoes disponiveis");

        List<string> opcoes = new List<string>
        {
            "1 - Meus Dados Pessoais",
            "2 - Meu Veiculo",
            "3 - Cadastrar Nova Rota",
            "4 - Minhas Rotas",
            "5 - Reservas do Meu Veiculo",
            "6 - Emitir Reembolso",
            "7 - Validar minha CNH",
            "8 - Historico de Viagens",
            "0 - Voltar ao Menu Principal"
        };

        return MostrarMenu(opcoes, 10, 8, "Selecione uma opcao:");
    }

    public void ExibirMenuPassageiroLogado()
    {
        LimparTela();
        DesenharCabecalho("PAINEL DO PASSAGEIRO", "Suas opcoes disponiveis");

        List<string> opcoes = new List<string>
        {
            "1 - Meus Dados Pessoais",
            "2 - Solicitar Nova Reserva",
            "3 - Minhas Reservas",
            "4 - Realizar Check-in",
            "5 - Realizar Check-out",
            "6 - Meus Alertas",
            "7 - Historico de Viagens",
            "8 - Avaliar Viagem",
            "0 - Voltar ao Menu Principal"
        };

        MostrarMenu(opcoes, 10, 8, "Selecione uma opcao:");
    }

    public string ExibirMenuPassageiroLogadoComRetorno()
    {
        LimparTela();
        DesenharCabecalho("PAINEL DO PASSAGEIRO", "Suas opcoes disponiveis");

        List<string> opcoes = new List<string>
        {
            "1 - Meus Dados Pessoais",
            "2 - Solicitar Nova Reserva",
            "3 - Minhas Reservas",
            "4 - Realizar Check-in",
            "5 - Realizar Check-out",
            "6 - Meus Alertas",
            "7 - Historico de Viagens",
            "8 - Avaliar Viagem",
            "0 - Voltar ao Menu Principal"
        };

        return MostrarMenu(opcoes, 10, 8, "Selecione uma opcao:");
    }

    // Menus CRUD
    public void ExibirMenuMotorista()
    {
        LimparTela();
        DesenharCabecalho("GERENCIAR MOTORISTAS", "Operacoes CRUD");

        List<string> opcoes = new List<string>
        {
            "1 - Cadastrar Novo Motorista",
            "2 - Listar Motoristas",
            "3 - Atualizar Dados do Motorista",
            "4 - Consultar Motorista",
            "5 - Validar CNH",
            "6 - Cadastrar Rota",
            "0 - Voltar ao Menu Principal"
        };

        MostrarMenu(opcoes, 10, 8, "Selecione uma opcao:");
    }

    public void ExibirMenuPassageiro()
    {
        LimparTela();
        DesenharCabecalho("GERENCIAR PASSAGEIROS", "Operacoes CRUD");

        List<string> opcoes = new List<string>
        {
            "1 - Cadastrar Novo Passageiro",
            "2 - Listar Passageiros",
            "3 - Atualizar Dados do Passageiro",
            "4 - Consultar Passageiro",
            "5 - Solicitar Reserva",
            "6 - Realizar Check-in",
            "7 - Realizar Check-out",
            "0 - Voltar ao Menu Principal"
        };

        MostrarMenu(opcoes, 10, 8, "Selecione uma opcao:");
    }

    public void ExibirMenuVeiculo()
    {
        LimparTela();
        DesenharCabecalho("GERENCIAR VEICULOS", "Operacoes CRUD");

        List<string> opcoes = new List<string>
        {
            "1 - Cadastrar Novo Veiculo",
            "2 - Listar Veiculos",
            "3 - Atualizar Dados do Veiculo",
            "4 - Consultar Veiculo",
            "5 - Validar Capacidade",
            "0 - Voltar ao Menu Principal"
        };

        MostrarMenu(opcoes, 10, 8, "Selecione uma opcao:");
    }

    // Métodos de entrada de dados
    public string LerTexto(string prompt)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write($"{prompt}: ");
        Console.ResetColor();
        return Console.ReadLine() ?? "";
    }

    public int LerNumero(string prompt)
    {
        while (true)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"{prompt}: ");
            Console.ResetColor();
            if (int.TryParse(Console.ReadLine(), out int numero))
            {
                return numero;
            }
            Console.WriteLine("Por favor, digite um numero valido!");
        }
    }

    public decimal LerDecimal(string prompt)
    {
        while (true)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"{prompt}: ");
            Console.ResetColor();
            if (decimal.TryParse(Console.ReadLine(), out decimal numero))
            {
                return numero;
            }
            Console.WriteLine("Por favor, digite um valor decimal valido!");
        }
    }

    public DateTime LerData(string prompt)
    {
        while (true)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"{prompt} (dd/MM/yyyy HH:mm): ");
            Console.ResetColor();
            if (DateTime.TryParse(Console.ReadLine(), out DateTime data))
            {
                return data;
            }
            Console.WriteLine("Por favor, digite uma data valida no formato dd/MM/yyyy HH:mm!");
        }
    }

    // Métodos da tela
    public void ExibirCabecalho(string titulo)
    {
        LimparTela();
        DesenharCabecalho(titulo);
    }

    public void ExibirMensagem(string mensagem, bool aguardar = true)
    {
        int linhaSeg = ObterLinhaSegura(Console.WindowHeight - 6);
        MostrarMensagem(2, linhaSeg, mensagem);
        
        if (aguardar)
        {
            AguardarTecla();
        }
    }
}