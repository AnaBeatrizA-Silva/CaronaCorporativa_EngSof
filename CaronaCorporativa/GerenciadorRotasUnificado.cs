using System;
using System.Collections.Generic;
using System.Linq;

public class GerenciadorRotasUnificado
{
    private Dictionary<string, BairroInfo> bairros = new Dictionary<string, BairroInfo>();
    private Dictionary<string, EixoRotaUnificado> eixos = new Dictionary<string, EixoRotaUnificado>();
    private const string ENDERECO_PERINI = "Perini";
    private const double DISTANCIA_MINIMA_REEMBOLSO = 10.0;
    
    // Para compatibilidade com sistema de pareamento
    private List<Rota> rotas;
    private List<SolicitacaoCarona> solicitacoes;
    private List<Veiculo> veiculos;
    private Dictionary<string, int> assentosDisponiveis;
    private Dictionary<string, List<string>> caronasAceitas;

    public GerenciadorRotasUnificado(List<Rota>? rotas = null, List<SolicitacaoCarona>? solicitacoes = null, List<Veiculo>? veiculos = null)
    {
        this.rotas = rotas ?? new List<Rota>();
        this.solicitacoes = solicitacoes ?? new List<SolicitacaoCarona>();
        this.veiculos = veiculos ?? new List<Veiculo>();
        this.assentosDisponiveis = new Dictionary<string, int>();
        this.caronasAceitas = new Dictionary<string, List<string>>();
        
        CarregarDadosUnificados();
        InicializarAssentos();
    }

    private void CarregarDadosUnificados()
    {
        // ===== DADOS UNIFICADOS DE BAIRROS E EIXOS =====
        // Organização por eixo geográfico com distâncias corretas da Perini
        
        // EIXO NORTE - Bairros ao NORTE da Perini
        // Via principal: R. Dona Francisca (sentido norte)
        var eixoNorte = new EixoRotaUnificado("Eixo Norte", "R. Dona Francisca (Norte)", ConsoleColor.Blue);
        
        // Ordem: do mais próximo ao mais distante da Perini
        AdicionarBairroNoEixo(eixoNorte, "Pirabeiraba", 4.2, TipoBairro.Origem);
        AdicionarBairroNoEixo(eixoNorte, "Santo Antônio", 5.5, TipoBairro.Origem);
        AdicionarBairroNoEixo(eixoNorte, "Bom Retiro", 6.5, TipoBairro.Origem);
        AdicionarBairroNoEixo(eixoNorte, "Costa e Silva", 7.1, TipoBairro.Intermediario);
        AdicionarBairroNoEixo(eixoNorte, "Glória", 7.8, TipoBairro.Intermediario);
        AdicionarBairroNoEixo(eixoNorte, "América", 9.2, TipoBairro.Origem);
        
        eixos["Eixo Norte"] = eixoNorte;

        // EIXO CENTRAL - Região CENTRAL próxima à Perini
        // Via principal: R. Dona Francisca (centro)
        var eixoCentral = new EixoRotaUnificado("Eixo Central", "R. Dona Francisca (Centro)", ConsoleColor.Green);
        
        AdicionarBairroNoEixo(eixoCentral, "Centro", 8.1, TipoBairro.Origem);
        AdicionarBairroNoEixo(eixoCentral, "Saguaçu", 9.0, TipoBairro.Intermediario);
        AdicionarBairroNoEixo(eixoCentral, "Iririú", 9.5, TipoBairro.Intermediario);
        AdicionarBairroNoEixo(eixoCentral, "Anita Garibaldi", 11.0, TipoBairro.Origem);
        AdicionarBairroNoEixo(eixoCentral, "Atiradores", 11.2, TipoBairro.Origem);
        AdicionarBairroNoEixo(eixoCentral, "Bucarein", 11.5, TipoBairro.Origem);
        
        eixos["Eixo Central"] = eixoCentral;

        // EIXO LESTE - Bairros a LESTE da Perini  
        // Via principal: Av. Santos Dumont
        var eixoLeste = new EixoRotaUnificado("Eixo Leste", "Av. Santos Dumont (Leste)", ConsoleColor.Yellow);
        
        AdicionarBairroNoEixo(eixoLeste, "Jardim Iririú", 9.8, TipoBairro.Origem);
        AdicionarBairroNoEixo(eixoLeste, "Aventureiro", 10.6, TipoBairro.Origem);
        AdicionarBairroNoEixo(eixoLeste, "Boa Vista", 10.8, TipoBairro.Intermediario);
        AdicionarBairroNoEixo(eixoLeste, "Comasa", 12.5, TipoBairro.Origem);
        AdicionarBairroNoEixo(eixoLeste, "Espinheiros", 14.0, TipoBairro.Origem);
        AdicionarBairroNoEixo(eixoLeste, "Jardim Paraíso", 19.0, TipoBairro.Origem);
        
        eixos["Eixo Leste"] = eixoLeste;

        // EIXO SUL - Bairros ao SUL da Perini
        // Via principal: Av. Santos Dumont / R. Dona Francisca (sul)
        var eixoSul = new EixoRotaUnificado("Eixo Sul", "Av. Santos Dumont (Sul)", ConsoleColor.Red);
        
        AdicionarBairroNoEixo(eixoSul, "Jarivatuba", 13.0, TipoBairro.Origem);
        AdicionarBairroNoEixo(eixoSul, "Fátima", 13.5, TipoBairro.Intermediario);
        AdicionarBairroNoEixo(eixoSul, "João Costa", 13.8, TipoBairro.Origem);
        AdicionarBairroNoEixo(eixoSul, "Adhemar Garcia", 14.5, TipoBairro.Origem);
        AdicionarBairroNoEixo(eixoSul, "Guanabara", 16.0, TipoBairro.Intermediario);
        AdicionarBairroNoEixo(eixoSul, "Parque Guarani", 18.0, TipoBairro.Origem);
        AdicionarBairroNoEixo(eixoSul, "Petrópolis", 20.0, TipoBairro.Origem);
        AdicionarBairroNoEixo(eixoSul, "Itinga", 20.5, TipoBairro.Origem);
        AdicionarBairroNoEixo(eixoSul, "Paranaguamirim", 21.0, TipoBairro.Origem);
        AdicionarBairroNoEixo(eixoSul, "Ulysses Guimarães", 22.0, TipoBairro.Origem);
        
        eixos["Eixo Sul"] = eixoSul;

        // EIXO OESTE - Bairros a OESTE da Perini
        // Via principal: R. Dona Francisca (oeste)
        var eixoOeste = new EixoRotaUnificado("Eixo Oeste", "R. Dona Francisca (Oeste)", ConsoleColor.Magenta);
        
        AdicionarBairroNoEixo(eixoOeste, "Vila Nova", 15.2, TipoBairro.Origem);
        AdicionarBairroNoEixo(eixoOeste, "Floresta", 15.4, TipoBairro.Intermediario);
        AdicionarBairroNoEixo(eixoOeste, "Morro do Meio", 16.3, TipoBairro.Origem);
        AdicionarBairroNoEixo(eixoOeste, "Nova Brasília", 16.5, TipoBairro.Origem);
        AdicionarBairroNoEixo(eixoOeste, "São Marcos", 17.0, TipoBairro.Origem);
        
        eixos["Eixo Oeste"] = eixoOeste;

        // BAIRROS ADICIONAIS (industriais ou especiais)
        AdicionarBairroAvulso("Zona Industrial Tupy", 3.0, "Industrial");
        AdicionarBairroAvulso("Santa Catarina", 11.8, "Residencial");
        AdicionarBairroAvulso("Dona Francisca", 12.0, "Residencial");
        AdicionarBairroAvulso("Boehmerwald", 17.5, "Rural");
        AdicionarBairroAvulso("Professora", 18.5, "Residencial");
        AdicionarBairroAvulso("Profipo", 19.5, "Industrial");
        AdicionarBairroAvulso("Lourdes", 19.8, "Residencial");
        AdicionarBairroAvulso("Vila Cubatão", 23.0, "Rural");
        AdicionarBairroAvulso("Ribeirão do Cubatão", 24.0, "Rural");
    }

    private void AdicionarBairroNoEixo(EixoRotaUnificado eixo, string nome, double distancia, TipoBairro tipo)
    {
        var bairro = new BairroInfo
        {
            Nome = nome,
            DistanciaPerini = distancia,
            Eixo = eixo.Nome,
            TipoPonto = tipo,
            ElegivelReembolso = distancia >= DISTANCIA_MINIMA_REEMBOLSO,
            ValorReembolso = distancia >= DISTANCIA_MINIMA_REEMBOLSO ? distancia * 2.50 : 0
        };
        
        bairros[nome] = bairro;
        eixo.AdicionarBairro(bairro);
    }

    private void AdicionarBairroAvulso(string nome, double distancia, string categoria)
    {
        var bairro = new BairroInfo
        {
            Nome = nome,
            DistanciaPerini = distancia,
            Eixo = $"Avulso ({categoria})",
            TipoPonto = TipoBairro.Avulso,
            ElegivelReembolso = distancia >= DISTANCIA_MINIMA_REEMBOLSO,
            ValorReembolso = distancia >= DISTANCIA_MINIMA_REEMBOLSO ? distancia * 2.50 : 0
        };
        
        bairros[nome] = bairro;
    }

    // ===== MÉTODOS DE VALIDAÇÃO (compatíveis com GerenciadorBairros) =====
    
    public bool ValidarRota(string origem, string destino, out double distancia, out string mensagem)
    {
        distancia = 0;
        mensagem = "";

        // Validação básica
        if (string.IsNullOrWhiteSpace(origem) || string.IsNullOrWhiteSpace(destino))
        {
            mensagem = "Origem e destino são obrigatórios!";
            return false;
        }

        // Pelo menos uma das localidades deve ser Perini (origem OU destino)
        bool origemEhPerini = origem.Equals(ENDERECO_PERINI, StringComparison.OrdinalIgnoreCase);
        bool destinoEhPerini = destino.Equals(ENDERECO_PERINI, StringComparison.OrdinalIgnoreCase);
        
        if (!origemEhPerini && !destinoEhPerini)
        {
            mensagem = "Pelo menos uma das localidades (origem ou destino) deve ser a sede da empresa (Perini)!";
            return false;
        }

        // Determinar qual é o bairro não-Perini para calcular a distância
        string bairroRota = origemEhPerini ? destino : origem;

        // Buscar bairro não-Perini
        if (!bairros.ContainsKey(bairroRota))
        {
            mensagem = $"Bairro '{bairroRota}' não encontrado no sistema!";
            return false;
        }

        var bairroOrigem = bairros[bairroRota];
        distancia = bairroOrigem.DistanciaPerini;
        
        string direcao = origemEhPerini ? $"{origem} → {destino}" : $"{origem} → {destino}";
        mensagem = $"Rota válida: {direcao} ({distancia:F1} km)";
        return true;
    }

    public bool EhElegivelParaReembolso(double distancia)
    {
        return distancia >= DISTANCIA_MINIMA_REEMBOLSO;
    }

    public double CalcularReembolso(double distancia)
    {
        return EhElegivelParaReembolso(distancia) ? distancia * 2.50 : 0;
    }

    // ===== MÉTODOS DE PAREAMENTO =====

    public List<SolicitacaoCarona> BuscarSolicitacoesCompativeis(string cpfMotorista)
    {
        var rotaMotorista = rotas.FirstOrDefault(r => r.CpfMotorista == cpfMotorista);
        if (rotaMotorista == null)
        {
            return new List<SolicitacaoCarona>();
        }

        var solicitacoesCompativeis = new List<SolicitacaoCarona>();

        foreach (var solicitacao in solicitacoes.Where(s => s.Status == "Pendente"))
        {
            if (VerificarCompatibilidadeInteligente(rotaMotorista, solicitacao))
            {
                // Atualizar distância correta
                if (ValidarRota(solicitacao.EnderecoOrigem, solicitacao.EnderecoDestino, out double distanciaCorreta, out _))
                {
                    solicitacao.DistanciaKm = distanciaCorreta;
                }
                
                solicitacoesCompativeis.Add(solicitacao);
            }
        }

        // Ordenar por proximidade geográfica (passageiros no caminho primeiro)
        return solicitacoesCompativeis.OrderBy(s => ObterDistanciaParaPerini(s.EnderecoOrigem)).ToList();
    }

    private bool VerificarCompatibilidadeInteligente(Rota rotaMotorista, SolicitacaoCarona solicitacao)
    {
        // Regra 1: PELO MENOS UMA das rotas deve incluir Perini (origem OU destino)
        bool motoristaEnvolvePerini = rotaMotorista.EnderecoPartida.Equals(ENDERECO_PERINI, StringComparison.OrdinalIgnoreCase) ||
                                     rotaMotorista.EnderecoFinal.Equals(ENDERECO_PERINI, StringComparison.OrdinalIgnoreCase);
        bool passageiroEnvolvePerini = solicitacao.EnderecoOrigem.Equals(ENDERECO_PERINI, StringComparison.OrdinalIgnoreCase) ||
                                      solicitacao.EnderecoDestino.Equals(ENDERECO_PERINI, StringComparison.OrdinalIgnoreCase);
        
        // Se nenhuma das rotas envolve Perini, não é compatível
        if (!motoristaEnvolvePerini && !passageiroEnvolvePerini)
        {
            return false;
        }

        // Regra 2: Verificar se o passageiro está no trajeto do motorista
        var eixoMotorista = IdentificarEixoBairro(rotaMotorista.EnderecoPartida);
        var eixoPassageiro = IdentificarEixoBairro(solicitacao.EnderecoOrigem);
        
        // Se não conseguir identificar eixos, permitir (ser flexível)
        if (eixoMotorista == null || eixoPassageiro == null)
        {
            return true;
        }

        // Se estão no mesmo eixo
        if (eixoMotorista.Nome == eixoPassageiro.Nome)
        {
            // Verificar se passageiro está no trajeto (mais próximo da Perini ou no caminho)
            double distMotorista = ObterDistanciaParaPerini(rotaMotorista.EnderecoPartida);
            double distPassageiro = ObterDistanciaParaPerini(solicitacao.EnderecoOrigem);
            
            // Passageiro deve estar no caminho (mais longe da Perini que o motorista, ou próximo)
            double diferenca = Math.Abs(distMotorista - distPassageiro);
            return diferenca <= 5.0; // Tolerância de 5km
        }

        // Se estão em eixos diferentes mas próximos geograficamente
        double distMotoristaTotal = ObterDistanciaParaPerini(rotaMotorista.EnderecoPartida);
        double distPassageiroTotal = ObterDistanciaParaPerini(solicitacao.EnderecoOrigem);
        double diferencaTotal = Math.Abs(distMotoristaTotal - distPassageiroTotal);
        
        // Permitir se diferença for pequena (eixos próximos)
        return diferencaTotal <= 3.0; // Tolerância de 3km entre eixos
    }

    private EixoRotaUnificado? IdentificarEixoBairro(string nomeBairro)
    {
        if (!bairros.ContainsKey(nomeBairro))
        {
            return null;
        }

        var bairro = bairros[nomeBairro];
        return eixos.Values.FirstOrDefault(e => e.Nome == bairro.Eixo);
    }

    private double ObterDistanciaParaPerini(string nomeBairro)
    {
        return bairros.ContainsKey(nomeBairro) ? bairros[nomeBairro].DistanciaPerini : 0;
    }

    // ===== MÉTODOS DE CONTROLE DE ASSENTOS =====

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
        var veiculo = veiculos.FirstOrDefault();
        return veiculo?.ObterCapacidade() ?? 4;
    }

    public ResultadoAcaoCarona AceitarCarona(string cpfMotorista, int idSolicitacao)
    {
        if (!assentosDisponiveis.ContainsKey(cpfMotorista) || assentosDisponiveis[cpfMotorista] <= 0)
        {
            return new ResultadoAcaoCarona(false, "Não há assentos disponíveis no veículo!");
        }

        var solicitacao = solicitacoes.FirstOrDefault(s => s.Id == idSolicitacao && s.Status == "Pendente");
        if (solicitacao == null)
        {
            return new ResultadoAcaoCarona(false, "Solicitação não encontrada ou já processada!");
        }

        if (caronasAceitas[cpfMotorista].Contains(solicitacao.CpfPassageiro))
        {
            return new ResultadoAcaoCarona(false, "Este passageiro já possui carona aceita com você!");
        }

        solicitacao.Status = "Aceita";
        solicitacao.CpfMotorista = cpfMotorista;
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
        if (assentosDisponiveis.ContainsKey(cpfMotorista))
        {
            assentosDisponiveis[cpfMotorista] = novaCapacidade;
        }
    }

    public void LiberarAssento(string cpfMotorista, string cpfPassageiro)
    {
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

    // ===== MÉTODOS DE CONSULTA E RELATÓRIOS =====

    public string ObterInformacaoBairro(string nomeBairro)
    {
        if (!bairros.ContainsKey(nomeBairro))
        {
            return $"Bairro '{nomeBairro}' não encontrado!";
        }

        var bairro = bairros[nomeBairro];
        return $"=== {bairro.Nome.ToUpper()} ===\n" +
               $"Distância da Perini: {bairro.DistanciaPerini:F1} km\n" +
               $"Eixo: {bairro.Eixo}\n" +
               $"Tipo: {bairro.TipoPonto}\n" +
               $"Reembolso: {(bairro.ElegivelReembolso ? $"R$ {bairro.ValorReembolso:F2}" : "Não elegível")}\n" +
               $"========================";
    }

    public string ObterInformacaoEixo(string nomeEixo)
    {
        if (!eixos.ContainsKey(nomeEixo))
        {
            return $"Eixo '{nomeEixo}' não encontrado!";
        }

        var eixo = eixos[nomeEixo];
        return eixo.ObterRelatorioCompleto();
    }

    public List<BairroInfo> ListarBairrosElegiveis()
    {
        return bairros.Values
                     .Where(b => b.ElegivelReembolso)
                     .OrderBy(b => b.DistanciaPerini)
                     .ToList();
    }
}

// ===== CLASSES DE APOIO =====

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

public class BairroInfo
{
    public string Nome { get; set; } = "";
    public double DistanciaPerini { get; set; }
    public string Eixo { get; set; } = "";
    public TipoBairro TipoPonto { get; set; }
    public bool ElegivelReembolso { get; set; }
    public double ValorReembolso { get; set; }
}

public class EixoRotaUnificado
{
    public string Nome { get; set; }
    public string ViaAcesso { get; set; }
    public ConsoleColor Cor { get; set; }
    public List<BairroInfo> Bairros { get; set; }

    public EixoRotaUnificado(string nome, string viaAcesso, ConsoleColor cor)
    {
        Nome = nome;
        ViaAcesso = viaAcesso;
        Cor = cor;
        Bairros = new List<BairroInfo>();
    }

    public void AdicionarBairro(BairroInfo bairro)
    {
        Bairros.Add(bairro);
    }

    public string ObterRelatorioCompleto()
    {
        var origens = Bairros.Where(b => b.TipoPonto == TipoBairro.Origem).OrderBy(b => b.DistanciaPerini);
        var intermediarios = Bairros.Where(b => b.TipoPonto == TipoBairro.Intermediario).OrderBy(b => b.DistanciaPerini);
        
        var relatorio = $"=== {Nome.ToUpper()} ===\n";
        relatorio += $"Via de Acesso: {ViaAcesso}\n";
        relatorio += $"Total de Bairros: {Bairros.Count}\n\n";
        
        relatorio += "Bairros de Origem:\n";
        foreach (var bairro in origens)
        {
            relatorio += $"• {bairro.Nome}: {bairro.DistanciaPerini:F1} km";
            relatorio += bairro.ElegivelReembolso ? $" (R$ {bairro.ValorReembolso:F2})\n" : " (sem reembolso)\n";
        }
        
        if (intermediarios.Any())
        {
            relatorio += "\nPontos Intermediários:\n";
            foreach (var bairro in intermediarios)
            {
                relatorio += $"• {bairro.Nome}: {bairro.DistanciaPerini:F1} km";
                relatorio += bairro.ElegivelReembolso ? $" (R$ {bairro.ValorReembolso:F2})\n" : " (sem reembolso)\n";
            }
        }
        
        return relatorio;
    }
}

public enum TipoBairro
{
    Origem,         // Ponto de origem de caronas
    Intermediario,  // Ponto de passagem/embarque
    Avulso         // Bairro sem eixo definido
}