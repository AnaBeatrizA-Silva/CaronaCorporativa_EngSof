using System;
using System.Collections.Generic;
using System.Linq;

public class MotoristaCRUD
{
    private List<Motorista> motoristas;
    private List<Veiculo> veiculos;
    private List<Rota> rotas;
    private List<Reembolso> reembolsos;
    private Tela tela;

    public MotoristaCRUD()
    {
        this.motoristas = new List<Motorista>();
        this.veiculos = new List<Veiculo>();
        this.rotas = new List<Rota>();
        this.reembolsos = new List<Reembolso>();
        this.tela = new Tela();
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
                "4 - Verificar reembolsos",
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
            // Posiciona corretamente as informações do motorista encontrado
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

        // Define a próxima linha de input após as informações e espaçamento
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
            Console.WriteLine("=== SUA ROTA CADASTRADA ===");
            rotaMotorista.ExibirDetalhes();
            Console.WriteLine();

            List<string> opcoes = new List<string>
            {
                "1 - Alterar dados da rota",
                "2 - Excluir rota",
                "0 - Voltar"
            };

            string opcao = tela.MostrarMenu(opcoes, 10, 8, "Escolha uma opcao:");

            switch (opcao)
            {
                case "1":
                    AlterarDadosRota(rotaMotorista);
                    break;
                case "2":
                    if (tela.ConfirmarAcao("Confirma a exclusao da rota?"))
                    {
                        rotas.Remove(rotaMotorista);
                        tela.ExibirSucesso("Rota excluida com sucesso!");
                        tela.AguardarTecla();
                    }
                    break;
                case "0":
                    return;
                default:
                    tela.ExibirErro("Opcao invalida!");
                    tela.AguardarTecla();
                    break;
            }
        }
        
        tela.AguardarTecla();
    }

    private void CadastrarRotaMotorista(Motorista motorista)
    {
        tela.LimparTela();
        tela.DesenharCabecalho("CADASTRAR ROTA", "Nova Rota");

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

        try
        {
            Console.WriteLine("Informe os dados da sua rota:");
            Console.WriteLine();

            string origem = tela.LerTexto("Endereco de origem");
            string destino = tela.LerTexto("Endereco de destino");
            string horario = tela.LerTexto("Horario de partida (HH:MM)");
            
            string distanciaTexto = tela.LerTexto("Distancia em km (ex: 15.5)");
            if (!double.TryParse(distanciaTexto, out double distancia) || distancia <= 0)
            {
                tela.ExibirErro("Distancia invalida! Digite um numero positivo (ex: 15.5)");
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
            
            // Exibir resumo antes de confirmar com posição controlada
            tela.DefinirProximaLinhaInput(tela.ObterLinhaSegura(18)); // Define posição após os inputs
            Console.SetCursorPosition(2, 15);
            Console.WriteLine("\n=== RESUMO DA ROTA ===");
            Console.WriteLine($"Origem: {origem}");
            Console.WriteLine($"Destino: {destino}");
            Console.WriteLine($"Horario: {horarioPartida:HH:mm}");
            Console.WriteLine($"Distancia: {distancia:F1} km");
            if (distancia >= 10.0)
            {
                Console.WriteLine($"Elegivel para reembolso: R$ {(distancia * 0.50):F2}");
            }
            Console.WriteLine("======================");
            
            if (tela.ConfirmarAcao("Confirma o cadastro da rota?"))
            {
                rotas.Add(rota);
                tela.ExibirSucesso("Rota cadastrada com sucesso!");
            }
        }
        catch (Exception ex)
        {
            tela.ExibirErro($"Erro ao cadastrar rota: {ex.Message}");
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
        Console.WriteLine(); // Segunda linha em branco para espaçamento

        // Define a próxima linha de input após as informações
        tela.DefinirProximaLinhaInput(18);

        string novaOrigem = tela.LerTexto($"Nova origem (Enter para manter: {rota.EnderecoPartida})");
        if (!string.IsNullOrWhiteSpace(novaOrigem)) rota.EnderecoPartida = novaOrigem;

        string novoDestino = tela.LerTexto($"Novo destino (Enter para manter: {rota.EnderecoFinal})");
        if (!string.IsNullOrWhiteSpace(novoDestino)) rota.EnderecoFinal = novoDestino;

        string novoHorario = tela.LerTexto($"Novo horario (Enter para manter: {rota.HorarioPartida:HH:mm})");
        if (!string.IsNullOrWhiteSpace(novoHorario))
        {
            if (TimeSpan.TryParse(novoHorario, out TimeSpan horarioParsed))
            {
                rota.HorarioPartida = DateTime.Today.Add(horarioParsed);
            }
            else
            {
                tela.ExibirErro("Horario invalido! Mantendo o anterior.");
            }
        }

        string novaDistancia = tela.LerTexto($"Nova distancia (Enter para manter: {rota.DistanciaTotal:F1})");
        if (!string.IsNullOrWhiteSpace(novaDistancia))
        {
            if (double.TryParse(novaDistancia, out double distanciaParsed) && distanciaParsed > 0)
            {
                rota.DistanciaTotal = distanciaParsed;
            }
            else
            {
                tela.ExibirErro("Distancia invalida! Mantendo a anterior.");
            }
        }

        tela.ExibirSucesso("Rota atualizada com sucesso!");
        tela.AguardarTecla();
    }



    private void VerificarReembolsos(Motorista motorista)
    {
        tela.LimparTela();
        tela.DesenharCabecalho("VERIFICAR REEMBOLSOS", "Reembolso Disponivel");

        // Buscar a rota do motorista
        var rotaMotorista = rotas.FirstOrDefault(r => r.CpfMotorista == motorista.Cpf);

        if (rotaMotorista == null)
        {
            tela.ExibirMensagem("Voce nao possui rota cadastrada.");
            tela.ExibirMensagem("Cadastre uma rota primeiro para verificar reembolsos.");
        }
        else
        {
            // Usar método organizado para exibir informações da rota
            tela.ExibirInformacoes("=== SUA ROTA ===",
                $"Origem: {rotaMotorista.EnderecoPartida}",
                $"Destino: {rotaMotorista.EnderecoFinal}",
                $"Distancia: {rotaMotorista.DistanciaTotal:F1} km",
                $"Horario: {rotaMotorista.HorarioPartida:dd/MM/yyyy HH:mm}");

            if (rotaMotorista.DistanciaTotal >= 10.0)
            {
                double valorReembolso = rotaMotorista.DistanciaTotal * 0.50; // R$ 0,50 por km
                
                Console.WriteLine("✅ ELEGIVEL PARA REEMBOLSO!");
                Console.WriteLine($"Criterio atendido: distancia >= 10km");
                Console.WriteLine($"Valor do reembolso: R$ {valorReembolso:F2}");
                Console.WriteLine($"Calculo: {rotaMotorista.DistanciaTotal:F1} km × R$ 0,50 = R$ {valorReembolso:F2}");
            }
            else
            {
                double faltamKm = 10.0 - rotaMotorista.DistanciaTotal;
                
                Console.WriteLine("❌ NÃO ELEGIVEL PARA REEMBOLSO");
                Console.WriteLine($"Criterio: distancia >= 10km");
                Console.WriteLine($"Faltam {faltamKm:F1} km para ser elegivel");
                Console.WriteLine("Atualize sua rota se a distancia estiver incorreta.");
            }
        }
        
        tela.AguardarTecla();
    }

    public List<Motorista> ObterMotoristas()
    {
        return motoristas;
    }
}