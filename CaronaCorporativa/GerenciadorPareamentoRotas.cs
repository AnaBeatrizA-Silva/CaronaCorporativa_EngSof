using System;
using System.Collections.Generic;
using System.Linq;

public class GerenciadorPareamentoRotas
{
    private Dictionary<string, EixoRota> eixosRota;
    private List<Rota> rotas;
    private List<SolicitacaoCarona> solicitacoes;
    private List<Veiculo> veiculos;
    private Dictionary<string, int> assentosDisponiveis; // CPF do motorista -> assentos disponíveis
    private Dictionary<string, List<string>> caronasAceitas; // CPF motorista -> lista CPF passageiros

    public GerenciadorPareamentoRotas(List<Rota> rotas, List<SolicitacaoCarona> solicitacoes, List<Veiculo> veiculos)
    {
        this.rotas = rotas ?? new List<Rota>();
        this.solicitacoes = solicitacoes ?? new List<SolicitacaoCarona>();
        this.veiculos = veiculos ?? new List<Veiculo>();
        this.eixosRota = new Dictionary<string, EixoRota>();
        this.assentosDisponiveis = new Dictionary<string, int>();
        this.caronasAceitas = new Dictionary<string, List<string>>();
        
        CarregarEixosRota();
        InicializarAssentos();
    }

    private void CarregarEixosRota()
    {
        // Carregar dados diretamente no código - SEM dependência de arquivos CSV
        CarregarDadosPadrao();
    }

    private void CarregarDadosPadrao()
    {
        // ===== REGRAS DE PAREAMENTO DE ROTAS - JOINVILLE =====
        // Baseado no arquivo pareamentorotas.csv
        // Ordenação por distância crescente de Perini (mais próximo para mais distante)
        
        // EIXO SUL - Via Av. Santos Dumont / R. Dona Francisca
        // Sequência geográfica por distância: 13km → 22km
        eixosRota["Eixo Sul"] = new EixoRota(
            "Eixo Sul",
            new List<string> { 
                "Jarivatuba",           // 13,0 km
                "João Costa",           // 13,8 km
                "Adhemar Garcia",       // 14,5 km
                "Parque Guarani",       // 18,0 km
                "Petrópolis",           // 20,0 km
                "Itinga",               // 20,5 km
                "Paranaguamirim",       // 21,0 km
                "Ulysses Guimarães"     // 22,0 km
            },
            new List<string> { "Fátima", "Guanabara", "Floresta" },
            "Av. Santos Dumont / R. Dona Francisca"
        );

        // EIXO LESTE - Via Av. Santos Dumont / R. Dona Francisca
        // Sequência geográfica por distância: 9,8km → 19km
        eixosRota["Eixo Leste"] = new EixoRota(
            "Eixo Leste",
            new List<string> { 
                "Jardim Iririú",        // 9,8 km
                "Aventureiro",          // 10,6 km
                "Comasa",               // 12,5 km
                "Espinheiros",          // 14,0 km
                "Jardim Paraíso"        // 19,0 km
            },
            new List<string> { "Iririú", "Boa Vista", "Saguaçu" },
            "Av. Santos Dumont / R. Dona Francisca"
        );

        // EIXO CENTRAL - Via R. Dona Francisca
        // Sequência geográfica por distância: 8,1km → 11,2km
        eixosRota["Eixo Central"] = new EixoRota(
            "Eixo Central",
            new List<string> { 
                "Centro",               // 8,1 km
                "Anita Garibaldi",      // 11,0 km
                "Atiradores",           // 11,2 km
                "Bucarein",             // 11,5 km
                "Saguaçu"               // 9,0 km (pode estar fora de ordem por questões viárias)
            },
            new List<string> { "América" },
            "R. Dona Francisca"
        );

        // EIXO OESTE - Via R. Dona Francisca
        // Sequência geográfica por distância: 15km → 17km
        eixosRota["Eixo Oeste"] = new EixoRota(
            "Eixo Oeste",
            new List<string> { 
                "Vila Nova",            // 15,2 km
                "Morro do Meio",        // 16,3 km
                "Nova Brasília",        // 16,5 km
                "São Marcos"            // 17,0 km
            },
            new List<string> { "Glória", "Costa e Silva" },
            "R. Dona Francisca"
        );

        // EIXO NORTE - Via R. Dona Francisca
        // Sequência geográfica por distância: 4,2km → 9,2km
        eixosRota["Eixo Norte"] = new EixoRota(
            "Eixo Norte",
            new List<string> { 
                "Pirabeiraba",          // 4,2 km (pode ser ponto inicial distante)
                "Santo Antônio",        // 5,5 km
                "Bom Retiro",           // 6,5 km
                "Costa e Silva",        // 7,1 km
                "Glória",               // 7,8 km
                "América"               // 9,2 km
            },
            new List<string>(), // Pontos intermediários já estão na rota principal
            "R. Dona Francisca"
        );
        
        // NOTA: As distâncias são aproximadas e baseadas no arquivo bairros.csv
        // A sequência pode variar por questões de trânsito e vias de acesso reais
    }

    private void InicializarAssentos()
    {
        foreach (var rota in rotas)
        {
            if (!string.IsNullOrWhiteSpace(rota.CpfMotorista))
            {
                int capacidade = ObterCapacidadeVeiculo(rota.CpfMotorista);
                assentosDisponiveis[rota.CpfMotorista] = capacidade;
                caronasAceitas[rota.CpfMotorista] = new List<string>();
            }
        }
    }

    private int ObterCapacidadeVeiculo(string cpfMotorista)
    {
        // Buscar capacidade do veículo do motorista
        var veiculo = veiculos.FirstOrDefault(); // Simplificado: usar primeiro veículo
        return veiculo?.ObterCapacidade() ?? 4; // Padrão 4 assentos
    }

    public List<SolicitacaoCarona> BuscarSolicitacoesCompativeis(string cpfMotorista)
    {
        var rotaMotorista = rotas.FirstOrDefault(r => r.CpfMotorista == cpfMotorista);
        if (rotaMotorista == null)
        {
            return new List<SolicitacaoCarona>();
        }

        var eixoMotorista = IdentificarEixoRota(rotaMotorista);
        if (eixoMotorista == null)
        {
            return new List<SolicitacaoCarona>();
        }

        var solicitacoesCompativeis = new List<SolicitacaoCarona>();

        foreach (var solicitacao in solicitacoes.Where(s => s.Status == "Pendente"))
        {
            if (VerificarCompatibilidadePareamento(eixoMotorista, rotaMotorista, solicitacao))
            {
                // Recalcular distância baseada no ponto de embarque do passageiro
                GerenciadorBairros gerenciadorBairros = new GerenciadorBairros();
                if (gerenciadorBairros.ValidarRota(solicitacao.EnderecoOrigem, solicitacao.EnderecoDestino, out double distanciaCorreta, out _))
                {
                    solicitacao.DistanciaKm = distanciaCorreta;
                }
                
                solicitacoesCompativeis.Add(solicitacao);
            }
        }

        return solicitacoesCompativeis;
    }

    private EixoRota? IdentificarEixoRota(Rota rota)
    {
        // Identifica qual eixo a rota do motorista pertence
        foreach (var eixo in eixosRota.Values)
        {
            // Verifica se a origem do motorista está nos bairros de origem do eixo
            if (eixo.BairrosOrigem.Any(b => b.Equals(rota.EnderecoPartida, StringComparison.OrdinalIgnoreCase)))
            {
                return eixo;
            }
            
            // Verifica se está nos bairros intermediários
            if (eixo.BairrosIntermediarios.Any(b => b.Equals(rota.EnderecoPartida, StringComparison.OrdinalIgnoreCase)))
            {
                return eixo;
            }
        }
        return null;
    }

    private bool VerificarCompatibilidadePareamento(EixoRota eixo, Rota rotaMotorista, SolicitacaoCarona solicitacao)
    {
        // Regra 1: Destino deve ser Perini (obrigatório para reembolso)
        if (!solicitacao.EnderecoDestino.Equals("Perini", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        // Regra 2: Origem do passageiro deve estar no mesmo eixo, mas DEPOIS da origem do motorista
        
        // Se passageiro está na mesma origem do motorista
        if (solicitacao.EnderecoOrigem.Equals(rotaMotorista.EnderecoPartida, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        // Se passageiro está nos pontos intermediários do eixo
        if (eixo.BairrosIntermediarios.Any(b => b.Equals(solicitacao.EnderecoOrigem, StringComparison.OrdinalIgnoreCase)))
        {
            return true;
        }

        // Se passageiro está em um bairro de origem do mesmo eixo mas geograficamente após o motorista
        if (eixo.BairrosOrigem.Any(b => b.Equals(solicitacao.EnderecoOrigem, StringComparison.OrdinalIgnoreCase)))
        {
            return VerificarSequenciaGeografica(eixo, rotaMotorista.EnderecoPartida, solicitacao.EnderecoOrigem);
        }

        return false;
    }

    private bool VerificarSequenciaGeografica(EixoRota eixo, string origemMotorista, string origemPassageiro)
    {
        // Verifica se o bairro do passageiro está geograficamente no caminho do motorista
        int indexMotorista = eixo.BairrosOrigem.FindIndex(b => b.Equals(origemMotorista, StringComparison.OrdinalIgnoreCase));
        int indexPassageiro = eixo.BairrosOrigem.FindIndex(b => b.Equals(origemPassageiro, StringComparison.OrdinalIgnoreCase));

        // Se ambos estão na lista e passageiro está ANTES do motorista na sequência (no trajeto)
        if (indexMotorista >= 0 && indexPassageiro >= 0)
        {
            return indexPassageiro < indexMotorista; // CORRIGIDO: passageiro deve estar antes do motorista
        }

        // Se não conseguir determinar, permite (para ser mais flexível)
        return true;
    }

    public ResultadoAcaoCarona AceitarCarona(string cpfMotorista, int idSolicitacao)
    {
        // Verificar se há assentos disponíveis
        if (!assentosDisponiveis.ContainsKey(cpfMotorista) || assentosDisponiveis[cpfMotorista] <= 0)
        {
            return new ResultadoAcaoCarona(false, "Não há assentos disponíveis no veículo!");
        }

        var solicitacao = solicitacoes.FirstOrDefault(s => s.Id == idSolicitacao && s.Status == "Pendente");
        if (solicitacao == null)
        {
            return new ResultadoAcaoCarona(false, "Solicitação não encontrada ou já processada!");
        }

        // Verificar se já aceita carona deste passageiro
        if (caronasAceitas[cpfMotorista].Contains(solicitacao.CpfPassageiro))
        {
            return new ResultadoAcaoCarona(false, "Este passageiro já possui carona aceita com você!");
        }

        // Aceitar a carona
        solicitacao.Status = "Aceita";
        assentosDisponiveis[cpfMotorista]--;
        caronasAceitas[cpfMotorista].Add(solicitacao.CpfPassageiro);

        return new ResultadoAcaoCarona(true, $"Carona aceita! Assentos restantes: {assentosDisponiveis[cpfMotorista]}");
    }

    public ResultadoAcaoCarona RejeitarCarona(int idSolicitacao)
    {
        var solicitacao = solicitacoes.FirstOrDefault(s => s.Id == idSolicitacao && s.Status == "Pendente");
        if (solicitacao == null)
        {
            return new ResultadoAcaoCarona(false, "Solicitação não encontrada ou já processada!");
        }

        solicitacao.Status = "Rejeitada";
        return new ResultadoAcaoCarona(true, "Carona rejeitada.");
    }

    public int ObterAssentosDisponiveis(string cpfMotorista)
    {
        return assentosDisponiveis.ContainsKey(cpfMotorista) ? assentosDisponiveis[cpfMotorista] : 0;
    }

    public List<SolicitacaoCarona> ObterCaronasAceitas(string cpfMotorista)
    {
        if (!caronasAceitas.ContainsKey(cpfMotorista))
        {
            return new List<SolicitacaoCarona>();
        }

        return solicitacoes.Where(s => s.Status == "Aceita" && 
                                      caronasAceitas[cpfMotorista].Contains(s.CpfPassageiro)).ToList();
    }

    public void AtualizarCapacidadeVeiculo(string cpfMotorista, int novaCapacidade)
    {
        // Define nova capacidade disponível
        if (assentosDisponiveis.ContainsKey(cpfMotorista))
        {
            assentosDisponiveis[cpfMotorista] = novaCapacidade;
        }
    }

    public void LiberarAssento(string cpfMotorista, string cpfPassageiro)
    {
        // Remove passageiro da lista e libera assento
        if (caronasAceitas.ContainsKey(cpfMotorista) && 
            caronasAceitas[cpfMotorista].Contains(cpfPassageiro))
        {
            caronasAceitas[cpfMotorista].Remove(cpfPassageiro);
            
            if (assentosDisponiveis.ContainsKey(cpfMotorista))
            {
                assentosDisponiveis[cpfMotorista]++;
            }
        }
    }

    public string ObterInformacaoEixo(string cpfMotorista)
    {
        var rota = rotas.FirstOrDefault(r => r.CpfMotorista == cpfMotorista);
        if (rota == null) return "Rota não encontrada";

        var eixo = IdentificarEixoRota(rota);
        if (eixo == null) return "Eixo não identificado";

        return eixo.ObterResumoFormatado();
    }
}

public class EixoRota
{
    public string Nome { get; set; }
    public List<string> BairrosOrigem { get; set; }
    public List<string> BairrosIntermediarios { get; set; }
    public string ViaAcessoFinal { get; set; }

    public EixoRota(string nome, List<string> bairrosOrigem, List<string> bairrosIntermediarios, string viaAcesso)
    {
        Nome = nome;
        BairrosOrigem = bairrosOrigem ?? new List<string>();
        BairrosIntermediarios = bairrosIntermediarios ?? new List<string>();
        ViaAcessoFinal = viaAcesso ?? "";
    }

    public string ObterResumoFormatado()
    {
        return $"=== {Nome.ToUpper()} ===\n" +
               $"Bairros de Origem: {string.Join(", ", BairrosOrigem)}\n" +
               $"Pontos Intermediários: {string.Join(", ", BairrosIntermediarios)}\n" +
               $"Via de Acesso: {ViaAcessoFinal}";
    }
}

public class ResultadoAcaoCarona
{
    public bool Sucesso { get; set; }
    public string Mensagem { get; set; }

    public ResultadoAcaoCarona(bool sucesso, string mensagem)
    {
        Sucesso = sucesso;
        Mensagem = mensagem;
    }
}