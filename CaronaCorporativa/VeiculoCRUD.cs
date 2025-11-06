using System;
using System.Collections.Generic;

public class VeiculoCRUD
{
    // Propriedades privadas
    private List<Veiculo> veiculos;
    private Veiculo veiculo;
    private int indice;
    private int coluna, linha, largura, altura;
    private List<string> dados;
    private Tela tela;

    // Construtor
    public VeiculoCRUD()
    {
        this.veiculos = new List<Veiculo>();
        this.veiculo = new Veiculo(0, "", "", "", 0, "", 0);
        this.indice = -1;

        // Configuracao dos campos para entrada de dados
        this.dados = new List<string>();
        dados.Add("ID Veiculo  : ");
        dados.Add("Placa       : ");
        dados.Add("Marca       : ");
        dados.Add("Modelo      : ");
        dados.Add("Ano         : ");
        dados.Add("Seguro      : ");
        dados.Add("Capacidade  : ");

        // Configuracao da interface
        this.coluna = 15;
        this.linha = 7;
        this.largura = 50;
        this.altura = 12;
        
        tela = new Tela();
    }

    // Metodo principal para executar operacoes CRUD
    public void ExecutarCRUD()
    {
        string resposta;

        tela.LimparTela();
        tela.DesenharCabecalho("GERENCIAR VEICULOS", "Cadastro e Manutencao");
        
        // Pergunta se deseja cadastrar novo ou buscar existente
        if (tela.ConfirmarAcao("Deseja cadastrar NOVO veiculo? (Nao = Buscar existente)"))
        {
            // Cadastro novo - começa pelo ID
            tela.LimparTela();
            tela.DesenharCabecalho("DADOS DO VEICULO", "Cadastro de Novo Veiculo");
            this.EntrarDados(3); // Todos os dados na ordem correta
            
            if (tela.ConfirmarAcao("Confirma o cadastro?"))
            {
                this.veiculos.Add(new Veiculo(
                    this.veiculo.ObterIdVeiculo(),
                    this.veiculo.ObterPlaca(),
                    this.veiculo.ObterMarca(),
                    this.veiculo.ObterModelo(),
                    this.veiculo.ObterAno(),
                    this.veiculo.ObterSeguro(),
                    this.veiculo.ObterCapacidade()
                ));
                
                tela.ExibirSucesso("Veiculo cadastrado com sucesso!");
            }
        }
        else
        {
            // Buscar existente - pede ID primeiro
            Console.Clear();
            tela.DesenharCabecalho("Buscar Veiculo");
            this.EntrarDados(1); // Apenas ID
            bool encontrou = this.ProcurarId();

            if (!encontrou)
            {
                tela.ExibirErro("ID de veiculo nao encontrado no sistema");
                
                if (tela.ConfirmarAcao("Deseja cadastrar novo veiculo com este ID?"))
                {
                    this.EntrarDados(2); // Todos os dados exceto ID
                    
                    if (tela.ConfirmarAcao("Confirma o cadastro?"))
                    {
                        this.veiculos.Add(new Veiculo(
                            this.veiculo.ObterIdVeiculo(),
                            this.veiculo.ObterPlaca(),
                            this.veiculo.ObterMarca(),
                            this.veiculo.ObterModelo(),
                            this.veiculo.ObterAno(),
                            this.veiculo.ObterSeguro(),
                            this.veiculo.ObterCapacidade()
                        ));
                        
                        tela.ExibirSucesso("Veiculo cadastrado com sucesso!");
                    }
                }
            }
            
            if (encontrou)
            {
                this.MostrarDados();
                tela.ExibirSucesso("Veiculo encontrado no sistema!");
                
                resposta = tela.LerTexto("Deseja Alterar, Excluir ou Voltar? (A/E/V)");
                
                if (resposta.ToUpper() == "A")
                {
                    // Limpa area dos dados para nova entrada
                    tela.LimparArea(coluna + 13, linha + 3, coluna + largura - 2, linha + 9);
                    this.EntrarDados(2);
                    
                    if (tela.ConfirmarAcao("Confirma as alteracoes?"))
                    {
                        // Atualiza o veiculo existente
                        this.veiculos[this.indice] = new Veiculo(
                            this.veiculo.ObterIdVeiculo(), // ID permanece o mesmo
                            this.veiculo.ObterPlaca(),
                            this.veiculo.ObterMarca(),
                            this.veiculo.ObterModelo(),
                            this.veiculo.ObterAno(),
                            this.veiculo.ObterSeguro(),
                            this.veiculo.ObterCapacidade()
                        );
                        
                        tela.ExibirSucesso("Dados alterados com sucesso!");
                    }
                }
                else if (resposta.ToUpper() == "E")
                {
                    if (tela.ConfirmarAcao("Confirma a exclusao do veiculo?"))
                    {
                        this.veiculos.RemoveAt(this.indice);
                        tela.ExibirSucesso("Veiculo excluido com sucesso!");
                    }
                }
            }
        }
        
        tela.AguardarTecla();
    }

    // Entrada de dados em etapas
    private void EntrarDados(int etapa)
    {
        int colEntrada = this.coluna + this.dados[0].Length + 1;
        int linEntrada = this.linha + 3;

        if (etapa == 1) // Apenas ID para busca
        {
            Console.SetCursorPosition(colEntrada, linEntrada);
            int id = int.Parse(Console.ReadLine() ?? "0");
            this.veiculo = new Veiculo(id, "", "", "", 0, "", 0);
        }
        else if (etapa == 2) // Todos os outros dados (ID ja preenchido)
        {
            // ID ja foi preenchido na etapa 1, mantemos o valor
            int idAtual = this.veiculo.ObterIdVeiculo();

            // Placa
            Console.SetCursorPosition(colEntrada, linEntrada + 1);
            string placa = Console.ReadLine() ?? "";

            // Marca  
            Console.SetCursorPosition(colEntrada, linEntrada + 2);
            string marca = Console.ReadLine() ?? "";

            // Modelo
            Console.SetCursorPosition(colEntrada, linEntrada + 3);
            string modelo = Console.ReadLine() ?? "";

            // Ano
            Console.SetCursorPosition(colEntrada, linEntrada + 4);
            int ano = int.Parse(Console.ReadLine() ?? "0");

            // Seguro
            Console.SetCursorPosition(colEntrada, linEntrada + 5);
            string seguro = Console.ReadLine() ?? "";

            // Capacidade
            Console.SetCursorPosition(colEntrada, linEntrada + 6);
            int capacidade = int.Parse(Console.ReadLine() ?? "0");

            // Cria novo objeto com todos os dados
            this.veiculo = new Veiculo(idAtual, placa, marca, modelo, ano, seguro, capacidade);
        }
        else if (etapa == 3) // Cadastro completo na ordem correta (ID, Placa, Marca, Modelo, Ano, Seguro, Capacidade)
        {
            // ID
            Console.SetCursorPosition(colEntrada, linEntrada);
            int id = int.Parse(Console.ReadLine() ?? "0");

            // Placa
            Console.SetCursorPosition(colEntrada, linEntrada + 1);
            string placa = Console.ReadLine() ?? "";

            // Marca  
            Console.SetCursorPosition(colEntrada, linEntrada + 2);
            string marca = Console.ReadLine() ?? "";

            // Modelo
            Console.SetCursorPosition(colEntrada, linEntrada + 3);
            string modelo = Console.ReadLine() ?? "";

            // Ano
            Console.SetCursorPosition(colEntrada, linEntrada + 4);
            int ano = int.Parse(Console.ReadLine() ?? "0");

            // Seguro
            Console.SetCursorPosition(colEntrada, linEntrada + 5);
            string seguro = Console.ReadLine() ?? "";

            // Capacidade
            Console.SetCursorPosition(colEntrada, linEntrada + 6);
            int capacidade = int.Parse(Console.ReadLine() ?? "0");

            // Cria novo objeto com todos os dados
            this.veiculo = new Veiculo(id, placa, marca, modelo, ano, seguro, capacidade);
        }
    }

    // Busca veiculo por ID
    private bool ProcurarId()
    {
        bool encontrou = false;

        for (int i = 0; i < this.veiculos.Count; i++)
        {
            if (this.veiculo.ObterIdVeiculo() == this.veiculos[i].ObterIdVeiculo())
            {
                encontrou = true;
                this.indice = i;
                // Copia dados do veiculo encontrado
                this.veiculo = new Veiculo(
                    this.veiculos[i].ObterIdVeiculo(),
                    this.veiculos[i].ObterPlaca(),
                    this.veiculos[i].ObterMarca(),
                    this.veiculos[i].ObterModelo(),
                    this.veiculos[i].ObterAno(),
                    this.veiculos[i].ObterSeguro(),
                    this.veiculos[i].ObterCapacidade()
                );
                break;
            }
        }

        return encontrou;
    }

    // Exibe dados do veiculo encontrado
    private void MostrarDados()
    {
        int colDados = this.coluna + this.dados[0].Length + 1;
        int linDados = this.linha + 3;

        // ID
        Console.SetCursorPosition(colDados, linDados);
        Console.Write(this.veiculo.ObterIdVeiculo());

        // Placa
        Console.SetCursorPosition(colDados, linDados + 1);
        Console.Write(this.veiculo.ObterPlaca());

        // Marca
        Console.SetCursorPosition(colDados, linDados + 2);
        Console.Write(this.veiculo.ObterMarca());

        // Modelo
        Console.SetCursorPosition(colDados, linDados + 3);
        Console.Write(this.veiculo.ObterModelo());

        // Ano
        Console.SetCursorPosition(colDados, linDados + 4);
        Console.Write(this.veiculo.ObterAno());

        // Seguro
        Console.SetCursorPosition(colDados, linDados + 5);
        Console.Write(this.veiculo.ObterSeguro());

        // Capacidade
        Console.SetCursorPosition(colDados, linDados + 6);
        Console.Write(this.veiculo.ObterCapacidade());
    }

    // Listar todos os veiculos
    public void ListarVeiculos()
    {
        tela.LimparTela();
        tela.DesenharCabecalho("LISTA DE VEICULOS", "Veiculos Cadastrados");

        if (veiculos.Count == 0)
        {
            tela.ExibirMensagem("Nenhum veiculo cadastrado no sistema.");
        }
        else
        {
            for (int i = 0; i < veiculos.Count; i++)
            {
                Console.WriteLine($"{i + 1}. ID: {veiculos[i].ObterIdVeiculo()} | {veiculos[i].ObterMarca()} {veiculos[i].ObterModelo()} | Placa: {veiculos[i].ObterPlaca()}");
            }
        }

        tela.AguardarTecla();
    }

    // Obter lista de veiculos (para uso por outras classes)
    public List<Veiculo> ObterVeiculos()
    {
        return new List<Veiculo>(veiculos);
    }

    // Adicionar veiculo diretamente (para uso interno do sistema)
    public void AdicionarVeiculo(Veiculo novoVeiculo)
    {
        veiculos.Add(novoVeiculo);
    }

    // Buscar veiculo por ID (para uso externo)
    public Veiculo? BuscarPorId(int id)
    {
        foreach (var veiculo in veiculos)
        {
            if (veiculo.ObterIdVeiculo() == id)
                return veiculo;
        }
        return null;
    }

    // Buscar veiculo por placa (para uso externo)
    public Veiculo? BuscarPorPlaca(string placa)
    {
        foreach (var veiculo in veiculos)
        {
            if (veiculo.ObterPlaca().ToUpper() == placa.ToUpper())
                return veiculo;
        }
        return null;
    }
}