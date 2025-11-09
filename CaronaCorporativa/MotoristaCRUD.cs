using System;
using System.Collections.Generic;
using System.Linq;

public class MotoristaCRUD
{
    private List<Motorista> motoristas;
    private List<Veiculo> veiculos;
    private List<Rota> rotas;
    private List<Reembolso> reembolsos;
    private List<SolicitacaoCarona> solicitacoes; // Nova lista para gerenciar solicitações
    private GerenciadorPareamentoRotas gerenciadorPareamento; // Novo gerenciador
    private Tela tela;

    public MotoristaCRUD()
    {
        this.motoristas = new List<Motorista>();
        this.veiculos = new List<Veiculo>();
        this.rotas = new List<Rota>();
        this.reembolsos = new List<Reembolso>();
        this.solicitacoes = new List<SolicitacaoCarona>();
        this.tela = new Tela();
        this.gerenciadorPareamento = new GerenciadorPareamentoRotas(rotas, solicitacoes, veiculos);
    }

    public void ExecutarCRUD()
    {
        while (true)
        {
            tela.LimparTela();
            tela.DesenharCabecalho("MÓDULO MOTORISTA", "Sistema de Caronas Corporativas");

            List<string> opcoes = new List<string>
            {
                "1 - Acessar cadastro existente",
                "2 - Criar novo cadastro",
                "0 - Voltar ao menu principal"
            };

            string opcao = tela.MostrarMenu(opcoes, 10, 8, "Escolha uma opcao:");

            switch (opcao)
            {
                case "1":
                    AcessarCadastroExistente();
                    break;
                case "2":
                    CriarNovoCadastro();
                    break;
                case "0":
                    return;
                default:
                    tela.ExibirErro("Opcao invalida!");
                    tela.AguardarTecla();
                    break;
            }
        }
    }

    private void CriarNovoCadastro()
    {
        tela.LimparTela();
        tela.DesenharCabecalho("CADASTRAR MOTORISTA", "Novo Motorista");

        Motorista motorista = new Motorista();
        
        motorista.Nome = tela.LerTexto("Nome completo");
        motorista.Cpf = tela.LerTexto("CPF (somente numeros)");
        motorista.Cargo = tela.LerTexto("Cargo na empresa");
        motorista.Cnh = tela.LerTexto("Numero da CNH");

        // Verificar se CPF já existe
        if (motoristas.Any(m => m.Cpf == motorista.Cpf))
        {
            tela.ExibirErro("Ja existe um motorista cadastrado com este CPF!");
            tela.AguardarTecla();
            return;
        }

        if (tela.ConfirmarAcao("Confirma o cadastro?"))
        {
            motoristas.Add(motorista);
            tela.ExibirSucesso("Motorista cadastrado com sucesso!");
            tela.AguardarTecla();
        }
    }

    private void AcessarCadastroExistente()
    {
        tela.LimparTela();
        tela.DesenharCabecalho("ACESSAR CADASTRO", "Login do Motorista");

        string cpf = tela.LerTexto("Digite seu CPF");
        
        Motorista? motorista = motoristas.FirstOrDefault(m => m.Cpf == cpf);
        
        if (motorista == null)
        {
            tela.ExibirErro("CPF nao encontrado! Verifique o numero ou crie um novo cadastro.");
            tela.AguardarTecla();
            return;
        }

        // Menu do motorista logado
        MenuMotoristaLogado(motorista);
    }

    private void MenuMotoristaLogado(Motorista motorista)
    {
        while (true)
        {
            tela.LimparTela();
            tela.DesenharCabecalho($"BEM-VINDO, {motorista.Nome.ToUpper()}", "Painel do Motorista");

            List<string> opcoes = new List<string>
            {
                "1 - Alterar cadastro",
                "2 - Gerenciar veiculo",
                "3 - Gerenciar rota",
                "4 - Verificar solicitações de carona",
                "5 - Visualizar caronas aceitas",
                "6 - Verificar reembolsos",
                "0 - Sair"              
            };

            string opcao = tela.MostrarMenu(opcoes, 10, 8, "Escolha uma opcao:");

            switch (opcao)
            {
                case "1":
                    AlterarCadastroMotorista(motorista);
                    break;
                case "2":
                    GerenciarVeiculo(motorista);
                    break;
                case "3":
                    GerenciarRota(motorista);
                    break;
                case "4":
                    VerificarSolicitacoesCarona(motorista);
                    break;
                case "5":
                    VisualizarCaronasAceitas(motorista);
                    break;
                case "6":
                    VerificarReembolsos(motorista);
                    break;
                case "0":
                    return;
                default:
                    tela.ExibirErro("Opcao invalida!");
                    tela.AguardarTecla();
                    break;
            }
        }
    }

    private void CadastrarMotorista()
    {
        tela.LimparTela();
        tela.DesenharCabecalho("CADASTRAR MOTORISTA", "Novo Motorista");

        Motorista motorista = new Motorista();
        
        motorista.Nome = tela.LerTexto("Nome");
        motorista.Cpf = tela.LerTexto("CPF");
        motorista.Cargo = tela.LerTexto("Cargo");
        motorista.Cnh = tela.LerTexto("CNH");

        if (tela.ConfirmarAcao("Confirma o cadastro?"))
        {
            motoristas.Add(motorista);
            tela.ExibirSucesso("Motorista cadastrado com sucesso!");
        }
        
        tela.AguardarTecla();
    }

    public void ListarMotoristas()
    {
        tela.LimparTela();
        tela.DesenharCabecalho("LISTA DE MOTORISTAS", "Motoristas Cadastrados");

        if (motoristas.Count == 0)
        {
            tela.ExibirMensagem("Nenhum motorista cadastrado.");
            tela.AguardarTecla();
            return;
        }

        // Posiciona corretamente a lista de motoristas
        Console.SetCursorPosition(2, 8);
        Console.WriteLine();
        for (int i = 0; i < motoristas.Count; i++)
        {
            Console.WriteLine($"{i + 1}. Nome: {motoristas[i].Nome} | CPF: {motoristas[i].Cpf} | CNH: {motoristas[i].Cnh}");
        }

        tela.AguardarTecla();
    }

    private void BuscarMotorista()
    {
        tela.LimparTela();
        tela.DesenharCabecalho("BUSCAR MOTORISTA", "Consulta por CPF");

        string cpf = tela.LerTexto("Digite o CPF para buscar");
        
        Motorista? encontrado = motoristas.Find(m => m.Cpf == cpf);
        
        if (encontrado != null)
        {
            Console.SetCursorPosition(2, 10);
            Console.WriteLine("\n=== MOTORISTA ENCONTRADO ===");
            Console.WriteLine($"Nome: {encontrado.Nome}");
            Console.WriteLine($"CPF: {encontrado.Cpf}");
            Console.WriteLine($"Cargo: {encontrado.Cargo}");
            Console.WriteLine($"CNH: {encontrado.Cnh}");
        }
        else
        {
            tela.ExibirErro("Motorista nao encontrado!");
        }
        
        tela.AguardarTecla();
    }

    private void AtualizarMotorista()
    {
        tela.LimparTela();
        tela.DesenharCabecalho("ATUALIZAR MOTORISTA", "Atualizacao de Dados");

        string cpf = tela.LerTexto("Digite o CPF do motorista");
        
        Motorista? motorista = motoristas.Find(m => m.Cpf == cpf);
        
        if (motorista != null)
        {
            Console.WriteLine($"\nMotorista encontrado: {motorista.Nome}");
            
            string novoNome = tela.LerTexto($"Novo nome (atual: {motorista.Nome}, Enter=manter)");
            if (!string.IsNullOrWhiteSpace(novoNome)) motorista.Nome = novoNome;
            
            string novoCargo = tela.LerTexto($"Novo cargo (atual: {motorista.Cargo}, Enter=manter)");
            if (!string.IsNullOrWhiteSpace(novoCargo)) motorista.Cargo = novoCargo;
            
            string novaCnh = tela.LerTexto($"Nova CNH (atual: {motorista.Cnh}, Enter=manter)");
            if (!string.IsNullOrWhiteSpace(novaCnh)) motorista.Cnh = novaCnh;
            
            tela.ExibirSucesso("Motorista atualizado com sucesso!");
        }
        else
        {
            tela.ExibirErro("Motorista nao encontrado!");
        }
        
        tela.AguardarTecla();
    }

    private void ExcluirMotorista()
    {
        tela.LimparTela();
        tela.DesenharCabecalho("EXCLUIR MOTORISTA", "Remocao de Cadastro");

        string cpf = tela.LerTexto("Digite o CPF do motorista");
        
        Motorista? motorista = motoristas.Find(m => m.Cpf == cpf);
        
        if (motorista != null)
        {
            Console.WriteLine($"\nMotorista encontrado: {motorista.Nome}");
            
            if (tela.ConfirmarAcao("Confirma a exclusao?"))
            {
                motoristas.Remove(motorista);
                tela.ExibirSucesso("Motorista excluido com sucesso!");
            }
        }
        else
        {
            tela.ExibirErro("Motorista nao encontrado!");
        }
        
        tela.AguardarTecla();
    }

    private void AlterarCadastroMotorista(Motorista motorista)
    {
        tela.LimparTela();
        tela.DesenharCabecalho("ALTERAR CADASTRO", "Atualizacao de Dados");

        // Posiciona corretamente os dados atuais
        Console.SetCursorPosition(2, 8);
        Console.WriteLine($"---Dados atuais do motorista:---");
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine($"Nome: {motorista.Nome}");
        Console.WriteLine($"CPF: {motorista.Cpf}");
        Console.WriteLine($"Cargo: {motorista.Cargo}");
        Console.WriteLine($"CNH: {motorista.Cnh}");
        Console.WriteLine(); // Linha em branco
        Console.WriteLine(); // Segunda linha em branco para espaçamento

        tela.DefinirProximaLinhaInput(16);
        
        string novoNome = tela.LerTexto($"Novo nome (Enter para manter: {motorista.Nome})");
        if (!string.IsNullOrWhiteSpace(novoNome)) motorista.Nome = novoNome;
        
        string novoCargo = tela.LerTexto($"Novo cargo (Enter para manter: {motorista.Cargo})");
        if (!string.IsNullOrWhiteSpace(novoCargo)) motorista.Cargo = novoCargo;
        
        string novaCnh = tela.LerTexto($"Nova CNH (Enter para manter: {motorista.Cnh})");
        if (!string.IsNullOrWhiteSpace(novaCnh)) motorista.Cnh = novaCnh;
        
        tela.ExibirSucesso("Cadastro atualizado com sucesso!");
        tela.AguardarTecla();
    }

    private void GerenciarVeiculo(Motorista motorista)
    {
        tela.LimparTela();
        tela.DesenharCabecalho("GERENCIAR VEICULO", "Dados do Veiculo");

        // Criar uma lista para armazenar veiculos do motorista
        // Por simplicidade, vamos assumir que cada motorista pode ter apenas 1 veiculo
        // e usar uma propriedade customizada para identificar
        var veiculosMotorista = veiculos.Where(v => v != null).ToList();
        
        if (veiculosMotorista.Count == 0)
        {
            tela.ExibirMensagem("Nenhum veiculo vinculado ao seu cadastro.");
            
            if (tela.ConfirmarAcao("Deseja cadastrar um veiculo?"))
            {
                CadastrarVeiculoMotorista(motorista);
            }
        }
        else
        {
            // Exibir dados do primeiro veiculo (por simplicidade)
            Veiculo veiculo = veiculosMotorista.First();
            
            Console.WriteLine("=== DADOS DO SEU VEICULO ===");
            veiculo.Consultar();
            
            if (tela.ConfirmarAcao("Deseja alterar os dados do veiculo?"))
            {
                AlterarVeiculoMotorista(veiculo);
            }
        }
        
        tela.AguardarTecla();
    }

    private void CadastrarVeiculoMotorista(Motorista motorista)
    {
        tela.LimparTela();
        tela.DesenharCabecalho("CADASTRAR VEICULO", "Novo Veiculo");

        try
        {
            string placa = tela.LerTexto("Placa do veiculo");
            string marca = tela.LerTexto("Marca");
            string modelo = tela.LerTexto("Modelo");
            
            string anoTexto = tela.LerTexto("Ano");
            if (!int.TryParse(anoTexto, out int ano) || ano < 1900 || ano > DateTime.Now.Year + 1)
            {
                tela.ExibirErro("Ano invalido! Digite um ano valido (ex: 2020)");
                tela.AguardarTecla();
                return;
            }
            
            string seguro = tela.LerTexto("Numero do seguro");
            
            string capacidadeTexto = tela.LerTexto("Capacidade de passageiros");
            if (!int.TryParse(capacidadeTexto, out int capacidade) || capacidade < 1 || capacidade > 50)
            {
                tela.ExibirErro("Capacidade invalida! Digite um numero entre 1 e 50");
                tela.AguardarTecla();
                return;
            }

            // Usar um ID sequencial simples ao invés do CPF
            int idVeiculo = veiculos.Count + 1;
            
            Veiculo veiculo = new Veiculo(idVeiculo, placa, marca, modelo, ano, seguro, capacidade);
            
            if (tela.ConfirmarAcao("Confirma o cadastro do veiculo?"))
            {
                veiculos.Add(veiculo);
                tela.ExibirSucesso("Veiculo cadastrado com sucesso!");
            }
        }
        catch (Exception ex)
        {
            tela.ExibirErro($"Erro ao cadastrar veiculo: {ex.Message}");
        }
        
        tela.AguardarTecla();
    }

    private void AlterarVeiculoMotorista(Veiculo veiculo)
    {
        tela.LimparTela();
        tela.DesenharCabecalho("ALTERAR VEICULO", "Atualizacao de Dados");
        
        tela.ExibirMensagem("Funcionalidade de alteracao de veiculo sera implementada.");
        // TODO: Implementar alteracao de dados do veiculo
    }

    private void GerenciarRota(Motorista motorista)
    {
        tela.LimparTela();
        tela.DesenharCabecalho("GERENCIAR ROTA", "Rota do Motorista");

        // Buscar a rota do motorista pelo CPF (máximo 1 rota por motorista)
        var rotaMotorista = rotas.FirstOrDefault(r => r.CpfMotorista == motorista.Cpf);

        if (rotaMotorista == null)
        {
            tela.ExibirMensagem("Nenhuma rota cadastrada.");
            
            if (tela.ConfirmarAcao("Deseja cadastrar uma rota?"))
            {
                CadastrarRotaMotorista(motorista);
            }
        }
        else
        {
            // Exibir dados da rota existente
            Console.SetCursorPosition(2, 8);
            Console.WriteLine("=== SUA ROTA CADASTRADA ===");
            rotaMotorista.ExibirDetalhes();
            Console.WriteLine();
            
            // Posicionar a solicitação na parte inferior da tela
            Console.SetCursorPosition(2, Console.WindowHeight - 3);
            Console.Write("Pressione: [A]lterar | [E]xcluir | [Enter] Voltar: ");
            
            ConsoleKeyInfo tecla = Console.ReadKey(true);
            
            switch (tecla.Key)
            {
                case ConsoleKey.A:
                    AlterarDadosRota(rotaMotorista);
                    break;
                case ConsoleKey.E:
                    if (tela.ConfirmarAcao("Confirma a exclusao da rota?"))
                    {
                        rotas.Remove(rotaMotorista);
                        tela.ExibirSucesso("Rota excluida com sucesso!");
                        tela.AguardarTecla();
                    }
                    break;
                case ConsoleKey.Enter:
                    return;
                default:
                    tela.ExibirErro("Tecla invalida!");
                    tela.AguardarTecla();
                    break;
            }
        }
        
        tela.AguardarTecla();
    }

    private void CadastrarRotaMotorista(Motorista motorista)
    {
        tela.LimparTela();
        tela.DesenharCabecalho("CADASTRAR ROTA", "Nova Rota: ");
        tela.ExibirMensagem("Informe os dados da sua rota:");

        // Verificar se já existe uma rota para este motorista
        var rotaExistente = rotas.FirstOrDefault(r => r.CpfMotorista == motorista.Cpf);
        if (rotaExistente != null)
        {
            tela.ExibirErro("Voce ja possui uma rota cadastrada!");
            tela.ExibirMensagem("Cada motorista pode ter apenas uma rota.");
            tela.ExibirMensagem("Use a opcao 'Alterar dados da rota' para modificar sua rota existente.");
            tela.AguardarTecla();
            return;
        }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("• Um dos endereços deve ser a sede da empresa Perini");
            Console.WriteLine("• Se origem não for Perini, destino deve ser Perini");
            Console.WriteLine("• Caronas serão pagas pela corporação somente para distâncias maiores que 10km");
            Console.WriteLine("• Critério de reembolso não sendo atendido, passageiro irá pagar o valor diretamente ao motorista.");
            Console.WriteLine();

            string origem = tela.LerTexto("Endereco de origem");
            string destino = tela.LerTexto("Endereco de destino");
            string horario = tela.LerTexto("Horario de partida (HH:MM)");

            // Valida rota usando o GerenciadorBairros
            GerenciadorBairros gerenciadorBairros = new GerenciadorBairros();
            if (!gerenciadorBairros.ValidarRota(origem, destino, out double distancia, out string mensagem))
            {
                tela.ExibirErro(mensagem);
                tela.AguardarTecla();
                return;
            }

            if (!TimeSpan.TryParse(horario, out TimeSpan horarioParsed))
            {
                tela.ExibirErro("Horario invalido! Use formato HH:MM (ex: 08:30)");
                tela.AguardarTecla();
                return;
            }

            DateTime horarioPartida = DateTime.Today.Add(horarioParsed);
            Rota rota = new Rota(rotas.Count + 1, origem, destino, horarioPartida, distancia, motorista.Cpf);

            // Verifica elegibilidade para reembolso corporativo
            bool elegivelReembolso = gerenciadorBairros.EhElegivelParaReembolso(distancia);
            double valorReembolso = elegivelReembolso ? distancia * 2.50 : 0;

            // Limpar tela antes de exibir o resumo
            tela.LimparTela();
            tela.DesenharCabecalho("CADASTRAR ROTA", "Confirmar Dados");

            // Exibir resumo da rota com posição controlada
            Console.SetCursorPosition(2, 8);
            Console.WriteLine("=== RESUMO DA ROTA ===");
            Console.WriteLine($"Origem: {origem}");
            Console.WriteLine($"Destino: {destino}");
            Console.WriteLine($"Horario: {horarioPartida:HH:mm}");
            Console.WriteLine($"Distancia (calculada): {distancia:F1} km");
            Console.WriteLine();
            
            if (elegivelReembolso)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"ROTA ELEGÍVEL PARA REEMBOLSO CORPORATIVO");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Rota não elegível para reembolso corporativo");
                double faltam = 10.0 - distancia;
                Console.WriteLine($"Faltam: {faltam:F1}km para elegibilidade");
            }
            Console.ResetColor();
            Console.WriteLine("======================");
            
            tela.DefinirProximaLinhaInput(Console.CursorTop + 2);
            
            if (tela.ConfirmarAcao("Confirma o cadastro da rota?"))
            {
                rotas.Add(rota);
                tela.ExibirSucesso("Rota cadastrada com sucesso!");
                tela.AguardarTecla();
            }
    }

    private void AlterarDadosRota(Rota rota)
    {
        tela.LimparTela();
        tela.DesenharCabecalho("ALTERAR DADOS DA ROTA", "Atualizacao");

        // Posiciona corretamente os dados atuais da rota
        Console.SetCursorPosition(2, 8);
        Console.WriteLine("Dados atuais da rota:");
        Console.WriteLine(rota.ObterDetalhesFormatados());
        Console.WriteLine(); // Linha em branco
        Console.WriteLine("IMPORTANTE: Um dos enderecos deve ser 'Perini'");
        Console.WriteLine("A distancia sera recalculada automaticamente");
        Console.WriteLine(); // Segunda linha em branco para espaçamento

        // Define a próxima linha de input após as informações
        tela.DefinirProximaLinhaInput(18);

        string novaOrigem = tela.LerTexto($"Nova origem (Enter para manter: {rota.EnderecoPartida})");
        if (string.IsNullOrWhiteSpace(novaOrigem)) novaOrigem = rota.EnderecoPartida;

        string novoDestino = tela.LerTexto($"Novo destino (Enter para manter: {rota.EnderecoFinal})");
        if (string.IsNullOrWhiteSpace(novoDestino)) novoDestino = rota.EnderecoFinal;

        // Validar nova rota usando GerenciadorBairros
        GerenciadorBairros gerenciadorBairros = new GerenciadorBairros();
        if (!gerenciadorBairros.ValidarRota(novaOrigem, novoDestino, out double novaDistancia, out string mensagem))
        {
            tela.ExibirErro(mensagem);
            tela.AguardarTecla();
            return;
        }

        string novoHorario = tela.LerTexto($"Novo horario (Enter para manter: {rota.HorarioPartida:HH:mm})");
        DateTime novoHorarioPartida = rota.HorarioPartida;
        
        if (!string.IsNullOrWhiteSpace(novoHorario))
        {
            if (TimeSpan.TryParse(novoHorario, out TimeSpan horarioParsed))
            {
                novoHorarioPartida = DateTime.Today.Add(horarioParsed);
            }
            else
            {
                tela.ExibirErro("Horario invalido! Mantendo o anterior.");
                tela.AguardarTecla();
                return;
            }
        }

        // Verificar elegibilidade para reembolso da nova rota
        bool elegivelReembolso = gerenciadorBairros.EhElegivelParaReembolso(novaDistancia);
        double valorReembolso = elegivelReembolso ? novaDistancia * 2.50 : 0;

        // Exibir resumo das mudanças
        Console.WriteLine();
        Console.WriteLine("=== RESUMO DAS ALTERAÇÕES ===");
        Console.WriteLine($"Origem: {novaOrigem}");
        Console.WriteLine($"Destino: {novoDestino}");
        Console.WriteLine($"Nova distancia (calculada): {novaDistancia:F1} km");
        Console.WriteLine($"Horário: {novoHorarioPartida:HH:mm}");
        Console.WriteLine();

        if (elegivelReembolso)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"ROTA ELEGIVEL PARA REEMBOLSO");
            Console.WriteLine($"Valor por passageiro: R$ {valorReembolso:F2}");
            Console.ResetColor();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Rota nao elegivel para reembolso");
            double faltam = 10.0 - novaDistancia;
            Console.WriteLine($"Faltam: {faltam:F1}km para elegibilidade");
            Console.ResetColor();
        }
        Console.WriteLine("=============================");

        if (tela.ConfirmarAcao("Confirma as alterações?"))
        {
            rota.EnderecoPartida = novaOrigem;
            rota.EnderecoFinal = novoDestino;
            rota.DistanciaTotal = novaDistancia;
            rota.HorarioPartida = novoHorarioPartida;
            
            tela.ExibirSucesso("Rota atualizada com sucesso!");
            tela.AguardarTecla();
        }
        
        tela.AguardarTecla();
    }



    private void VerificarReembolsos(Motorista motorista)
    {
        tela.LimparTela();
        tela.DesenharCabecalho("VERIFICAR REEMBOLSOS", "Análise Detalhada de Reembolsos");

        // Buscar a rota do motorista
        var rotaMotorista = rotas.FirstOrDefault(r => r.CpfMotorista == motorista.Cpf);

        if (rotaMotorista == null)
        {
            tela.ExibirMensagem("Voce nao possui rota cadastrada.");
            tela.ExibirMensagem("Cadastre uma rota primeiro para verificar elegibilidade para reembolsos.");
        }
        else
        {
            // Usar GerenciadorBairros para verificar elegibilidade da rota do motorista
            GerenciadorBairros gerenciador = new GerenciadorBairros();
            bool rotaElegivel = gerenciador.EhElegivelParaReembolso(rotaMotorista.DistanciaTotal);

            // Usar método organizado para exibir informações da rota
            tela.ExibirInformacoes("=== SUA ROTA ===",
                $"Origem: {rotaMotorista.EnderecoPartida}",
                $"Destino: {rotaMotorista.EnderecoFinal}",
                $"Distancia: {rotaMotorista.DistanciaTotal:F1} km",
                $"Horario: {rotaMotorista.HorarioPartida:dd/MM/yyyy HH:mm}");

            Console.WriteLine();
            Console.WriteLine("=== STATUS GERAL DA ROTA ===");
            
            if (rotaElegivel)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("SUA ROTA E ELEGIVEL PARA SISTEMA DE REEMBOLSO!");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Sua rota tem distancia < 10km do ponto base");
                Console.ResetColor();
            }

            // Buscar caronas aceitas e calcular reembolsos baseado na distância do passageiro
            var caronasAceitas = gerenciadorPareamento.ObterCaronasAceitas(motorista.Cpf);
            
            Console.WriteLine();
            Console.WriteLine("=== REEMBOLSOS POR PASSAGEIRO ===");
            Console.WriteLine("Reembolso calculado pela distancia do ponto de embarque do passageiro ate Perini");
            Console.WriteLine();

            if (caronasAceitas.Count == 0)
            {
                Console.WriteLine("Nenhuma carona aceita ainda.");
                Console.WriteLine("Use 'Verificar solicitações de carona' para aceitar solicitações.");
            }
            else
            {
                double totalReembolsos = 0;
                int totalElegiveis = 0;

                foreach (var carona in caronasAceitas)
                {
                    // Recalcular distância baseada no ponto de embarque do passageiro
                    bool validaRota = gerenciador.ValidarRota(carona.EnderecoOrigem, carona.EnderecoDestino, out double distanciaPassageiro, out _);
                    
                    if (validaRota)
                    {
                        bool elegivelReembolso = gerenciador.EhElegivelParaReembolso(distanciaPassageiro);
                        double valorReembolso = elegivelReembolso ? distanciaPassageiro * 2.50 : 0;

                        Console.WriteLine($"Passageiro: {carona.CpfPassageiro}");
                        Console.WriteLine($"Embarque em: {carona.EnderecoOrigem}");
                        Console.WriteLine($"Destino: {carona.EnderecoDestino}");
                        Console.WriteLine($"Distancia do passageiro: {distanciaPassageiro:F1}km");
                        
                        if (elegivelReembolso)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"REEMBOLSO APROVADO: R$ {valorReembolso:F2}");
                            Console.ResetColor();
                            totalReembolsos += valorReembolso;
                            totalElegiveis++;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            double faltam = 10.0 - distanciaPassageiro;
                            Console.WriteLine($"SEM REEMBOLSO - Faltam {faltam:F1}km para atingir 10km minimos");
                            Console.ResetColor();
                        }
                        Console.WriteLine($"� Data da carona: {carona.DataSolicitacao:dd/MM/yyyy}");
                        Console.WriteLine();
                    }
                }

                Console.WriteLine("==============================");
                Console.WriteLine($"RESUMO:");
                Console.WriteLine($"Total de passageiros: {caronasAceitas.Count}");
                Console.WriteLine($"Passageiros elegiveis: {totalElegiveis}");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"TOTAL EM REEMBOLSOS: R$ {totalReembolsos:F2}");
                Console.ResetColor();
                Console.WriteLine();
                
                Console.WriteLine("INFORMACOES IMPORTANTES:");
                Console.WriteLine("• Reembolsos são pagos diretamente aos passageiros");
                Console.WriteLine("• Cálculo baseado na distância do ponto de embarque até Perini");
                Console.WriteLine("• Valor: R$ 2,50 por quilômetro para distâncias >= 10km");
                Console.WriteLine("• Você como motorista oferece a carona, a empresa paga o reembolso");
            }
        }
        
        tela.AguardarTecla();
    }

    public List<Motorista> ObterMotoristas()
    {
        return motoristas;
    }

    // Método para integrar solicitações do sistema de passageiros
    public void AtualizarSolicitacoes(List<SolicitacaoCarona> novasSolicitacoes)
    {
        solicitacoes = novasSolicitacoes;
        gerenciadorPareamento = new GerenciadorPareamentoRotas(rotas, solicitacoes, veiculos);
    }

    private void VerificarSolicitacoesCarona(Motorista motorista)
    {
        tela.LimparTela();
        tela.DesenharCabecalho("SOLICITAÇÕES DE CARONA", "Pareamento de Rotas");

        // Verificar se tem rota cadastrada
        var rotaMotorista = rotas.FirstOrDefault(r => r.CpfMotorista == motorista.Cpf);
        if (rotaMotorista == null)
        {
            tela.ExibirErro("Você precisa cadastrar uma rota primeiro!");
            tela.ExibirMensagem("Acesse 'Gerenciar rota' para cadastrar sua rota.");
            tela.AguardarTecla();
            return;
        }

        // Buscar solicitações compatíveis
        var solicitacoesCompativeis = gerenciadorPareamento.BuscarSolicitacoesCompativeis(motorista.Cpf);
        int assentosDisponiveis = gerenciadorPareamento.ObterAssentosDisponiveis(motorista.Cpf);

        // Exibir informações do eixo
        Console.SetCursorPosition(2, 8);
        Console.WriteLine("=== INFORMAÇÕES DA SUA ROTA ===");
        Console.WriteLine(gerenciadorPareamento.ObterInformacaoEixo(motorista.Cpf));
        Console.WriteLine();
        Console.WriteLine($"Assentos disponiveis: {assentosDisponiveis}");
        Console.WriteLine();

        if (solicitacoesCompativeis.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("❌ Nenhuma solicitação compatível encontrada.");
            Console.WriteLine();
            Console.WriteLine("DICAS:");
            Console.WriteLine("• Solicitações aparecem quando passageiros do seu eixo pedem carona");
            Console.WriteLine("• Verifique se há passageiros nos bairros do seu trajeto");
            Console.WriteLine("• Solicitações devem ter destino 'Perini' para reembolso");
            Console.ResetColor();
            tela.AguardarTecla();
            return;
        }

        Console.WriteLine($"SOLICITACOES COMPATIVEIS ({solicitacoesCompativeis.Count}):");
        Console.WriteLine();

        for (int i = 0; i < solicitacoesCompativeis.Count; i++)
        {
            var sol = solicitacoesCompativeis[i];

            Console.WriteLine($"[{i + 1}] Solicitação #{sol.Id}");
            Console.WriteLine($"    Passageiro: {sol.CpfPassageiro}");
            Console.WriteLine($"    Rota: {sol.EnderecoOrigem} -> {sol.EnderecoDestino}");
            Console.WriteLine($"    Distancia: {sol.DistanciaKm:F1}km");
            Console.WriteLine($"    Data: {sol.DataSolicitacao:dd/MM/yyyy HH:mm}");
            Console.WriteLine();
        }

        if (assentosDisponiveis <= 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("VEICULO LOTADO - Nao e possivel aceitar mais passageiros!");
            Console.ResetColor();
            tela.AguardarTecla();
            return;
        }

        // Menu de ações
            List<string> opcoes = new List<string>
            {
                "1 - Aceitar solicitação",
                "2 - Rejeitar solicitação",
                "3 - Ver detalhes",
                "4 - Cancelar carona aceita",
                "0 - Voltar"
            };        string opcao = tela.MostrarMenu(opcoes, 10, Console.CursorTop + 2, "Escolha uma opção:");

        switch (opcao)
        {
            case "1":
                AceitarSolicitacaoCarona(motorista, solicitacoesCompativeis);
                break;
            case "2":
                RejeitarSolicitacaoCarona(solicitacoesCompativeis);
                break;
            case "3":
                VisualizarDetalhesSolicitacao(solicitacoesCompativeis);
                break;
            case "4":
                CancelarCaronaAceita(motorista);
                break;
            case "0":
                return;
            default:
                tela.ExibirErro("Opção inválida!");
                break;
        }

        tela.AguardarTecla();
    }

    private void AceitarSolicitacaoCarona(Motorista motorista, List<SolicitacaoCarona> solicitacoes)
    {
        string numeroStr = tela.LerTexto("Digite o número da solicitação para aceitar");
        if (!int.TryParse(numeroStr, out int numero) || numero < 1 || numero > solicitacoes.Count)
        {
            tela.ExibirErro("Número inválido!");
            return;
        }

        var solicitacao = solicitacoes[numero - 1];
        var resultado = gerenciadorPareamento.AceitarCarona(motorista.Cpf, solicitacao.Id);

        if (resultado.Sucesso)
        {
            tela.ExibirSucesso(resultado.Mensagem);
            Console.WriteLine();
            Console.WriteLine("Carona aceita com sucesso!");
            Console.WriteLine($"Passageiro: {solicitacao.CpfPassageiro}");
            Console.WriteLine($"Pegara em: {solicitacao.EnderecoOrigem}");
            Console.WriteLine($"Destino: {solicitacao.EnderecoDestino}");
        }
        else
        {
            tela.ExibirErro(resultado.Mensagem);
        }
    }

    private void RejeitarSolicitacaoCarona(List<SolicitacaoCarona> solicitacoes)
    {
        string numeroStr = tela.LerTexto("Digite o número da solicitação para rejeitar");
        if (!int.TryParse(numeroStr, out int numero) || numero < 1 || numero > solicitacoes.Count)
        {
            tela.ExibirErro("Número inválido!");
            return;
        }

        var solicitacao = solicitacoes[numero - 1];
        var resultado = gerenciadorPareamento.RejeitarCarona(solicitacao.Id);

        if (resultado.Sucesso)
        {
            tela.ExibirSucesso("Solicitação rejeitada.");
        }
        else
        {
            tela.ExibirErro(resultado.Mensagem);
        }
    }

    private void VisualizarDetalhesSolicitacao(List<SolicitacaoCarona> solicitacoes)
    {
        string numeroStr = tela.LerTexto("Digite o número da solicitação para ver detalhes");
        if (!int.TryParse(numeroStr, out int numero) || numero < 1 || numero > solicitacoes.Count)
        {
            tela.ExibirErro("Número inválido!");
            return;
        }

        var sol = solicitacoes[numero - 1];
        
        tela.LimparTela();
        tela.DesenharCabecalho("DETALHES DA SOLICITAÇÃO", $"Solicitação #{sol.Id}");

        Console.SetCursorPosition(2, 8);
        Console.WriteLine(sol.ObterDetalhesFormatados());
        
        Console.WriteLine();
        Console.WriteLine("Para informacoes sobre reembolsos, acesse 'Verificar reembolsos' no menu principal.");
    }

    private void VisualizarCaronasAceitas(Motorista motorista)
    {
        tela.LimparTela();
        tela.DesenharCabecalho("CARONAS ACEITAS", "Passageiros Confirmados");

        var caronasAceitas = gerenciadorPareamento.ObterCaronasAceitas(motorista.Cpf);
        int assentosDisponiveis = gerenciadorPareamento.ObterAssentosDisponiveis(motorista.Cpf);

        Console.SetCursorPosition(2, 8);
        Console.WriteLine($"Assentos disponiveis: {assentosDisponiveis}");
        Console.WriteLine($"Passageiros confirmados: {caronasAceitas.Count}");
        Console.WriteLine();

        if (caronasAceitas.Count == 0)
        {
            Console.WriteLine("Nenhuma carona aceita ainda.");
            Console.WriteLine("Use 'Verificar solicitações de carona' para aceitar solicitações.");
        }
        else
        {
            Console.WriteLine("=== PASSAGEIROS CONFIRMADOS ===");
            Console.WriteLine();

            foreach (var carona in caronasAceitas)
            {
                Console.WriteLine($"Passageiro: {carona.CpfPassageiro}");
                Console.WriteLine($"Pegara em: {carona.EnderecoOrigem}");
                Console.WriteLine($"Destino: {carona.EnderecoDestino}");
                Console.WriteLine($"Distancia: {carona.DistanciaKm:F1}km");
                Console.WriteLine($"� Data: {carona.DataSolicitacao:dd/MM/yyyy}");
                Console.WriteLine();
            }

            Console.WriteLine("================================");
            Console.WriteLine("Para informacoes detalhadas sobre reembolsos,");
            Console.WriteLine("   acesse 'Verificar reembolsos' no menu principal.");
        }

        tela.AguardarTecla();
    }

    private void CancelarCaronaAceita(Motorista motorista)
    {
        tela.LimparTela();
        tela.DesenharCabecalho("CANCELAR CARONA ACEITA", "Cancelamento de Carona");

        var caronasAceitas = gerenciadorPareamento.ObterCaronasAceitas(motorista.Cpf);
        
        if (caronasAceitas.Count == 0)
        {
            tela.ExibirMensagem("Você não possui caronas aceitas para cancelar.");
            tela.AguardarTecla();
            return;
        }

        Console.SetCursorPosition(2, 8);
        Console.WriteLine("=== CARONAS ACEITAS PARA CANCELAR ===");
        Console.WriteLine();

        for (int i = 0; i < caronasAceitas.Count; i++)
        {
            var carona = caronasAceitas[i];
            Console.WriteLine($"[{i + 1}] Solicitação #{carona.Id}");
            Console.WriteLine($"    Passageiro: {carona.CpfPassageiro}");
            Console.WriteLine($"    Rota: {carona.EnderecoOrigem} -> {carona.EnderecoDestino}");
            Console.WriteLine($"    Data aceita: {carona.DataSolicitacao:dd/MM/yyyy HH:mm}");
            Console.WriteLine();
        }

        string numeroStr = tela.LerTexto("Digite o número da carona para cancelar (0 para voltar)");
        if (!int.TryParse(numeroStr, out int numero) || numero < 0 || numero > caronasAceitas.Count)
        {
            if (numero != 0) tela.ExibirErro("Número inválido!");
            return;
        }

        if (numero == 0) return;

        var caronaSelecionada = caronasAceitas[numero - 1];

        // Exibir detalhes da carona a ser cancelada
        tela.ExibirInformacoes("=== CARONA A SER CANCELADA ===",
            $"Solicitação: #{caronaSelecionada.Id}",
            $"Passageiro: {caronaSelecionada.CpfPassageiro}",
            $"Rota: {caronaSelecionada.EnderecoOrigem} → {caronaSelecionada.EnderecoDestino}",
            $"Distância: {caronaSelecionada.DistanciaKm:F1}km");

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("ATENCAO: Este cancelamento sera reportado ao gestor!");
        Console.WriteLine("O passageiro pode ficar sem transporte!");
        Console.ResetColor();

        if (tela.ConfirmarAcao("Confirma o cancelamento desta carona?"))
        {
            // Cancelar a carona (alterar status para "Cancelada")
            caronaSelecionada.Status = "Cancelada";

            // Liberar assento no veículo
            gerenciadorPareamento.LiberarAssento(motorista.Cpf, caronaSelecionada.CpfPassageiro);

            // Gerar alerta para o gestor
            GerenciadorAlertas.AlertarCancelamentoCarona(
                motorista.Cpf, 
                caronaSelecionada.Id,
                caronaSelecionada.EnderecoOrigem,
                caronaSelecionada.EnderecoDestino
            );

            tela.ExibirSucesso("Carona cancelada com sucesso!");
            Console.WriteLine();
            Console.WriteLine("Alerta enviado ao gestor sobre o cancelamento.");
            Console.WriteLine("Assento liberado no veiculo.");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Recomendacao: Notifique o passageiro sobre o cancelamento");
            Console.WriteLine("   para que ele possa buscar transporte alternativo.");
            Console.ResetColor();
        }

        tela.AguardarTecla();
    }
}