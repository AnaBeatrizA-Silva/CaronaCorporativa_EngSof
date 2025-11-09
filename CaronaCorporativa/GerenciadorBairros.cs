using System;
using System.Collections.Generic;

public class GerenciadorBairros
{
    private Dictionary<string, double> bairrosDistancias = new Dictionary<string, double>();
    private const string ENDERECO_PERINI = "Perini";
    private const double DISTANCIA_MINIMA_REEMBOLSO = 10.0;
    
    public GerenciadorBairros()
    {
        CarregarBairros();
    }
    
    private void CarregarBairros()
    {
        // Carregar dados diretamente no código - SEM dependência de arquivos externos
        bairrosDistancias = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase);
        CarregarDadosHardcoded();
    }
    
    private void CarregarDadosHardcoded()
    {
        // ===== DISTÂNCIAS DOS BAIRROS DE JOINVILLE ATÉ PERINI =====
        // Distâncias aproximadas de carro em quilômetros
        
        // BAIRROS PRÓXIMOS (até 10km) - SEM reembolso automático
        bairrosDistancias["Zona Industrial Tupy"] = 3.0;
        bairrosDistancias["Pirabeiraba"] = 4.2;
        bairrosDistancias["Santo Antônio"] = 5.5;
        bairrosDistancias["Bom Retiro"] = 6.5;
        bairrosDistancias["Costa e Silva"] = 7.1;
        bairrosDistancias["Glória"] = 7.8;
        bairrosDistancias["Centro"] = 8.1;
        bairrosDistancias["Saguaçu"] = 9.0;
        bairrosDistancias["América"] = 9.2;
        bairrosDistancias["Iririú"] = 9.5;
        bairrosDistancias["Jardim Iririú"] = 9.8;
        
        // BAIRROS MÉDIOS (10km+) - COM reembolso automático
        bairrosDistancias["Aventureiro"] = 10.6;
        bairrosDistancias["Boa Vista"] = 10.8;
        bairrosDistancias["Anita Garibaldi"] = 11.0;
        bairrosDistancias["Atiradores"] = 11.2;
        bairrosDistancias["Bucarein"] = 11.5;
        bairrosDistancias["Santa Catarina"] = 11.8;
        bairrosDistancias["Dona Francisca"] = 12.0;
        bairrosDistancias["Comasa"] = 12.5;
        bairrosDistancias["Jarivatuba"] = 13.0;
        bairrosDistancias["Fátima"] = 13.5;
        bairrosDistancias["João Costa"] = 13.8;
        bairrosDistancias["Espinheiros"] = 14.0;
        bairrosDistancias["Adhemar Garcia"] = 14.5;
        
        // BAIRROS DISTANTES (15km+) - COM reembolso automático
        bairrosDistancias["Vila Nova"] = 15.2;
        bairrosDistancias["Floresta"] = 15.4;
        bairrosDistancias["Morro do Meio"] = 16.3;
        bairrosDistancias["Nova Brasília"] = 16.5;
        bairrosDistancias["São Marcos"] = 17.0;
        bairrosDistancias["Boehmerwald"] = 17.5;
        bairrosDistancias["Parque Guarani"] = 18.0;
        bairrosDistancias["Professora"] = 18.5;
        bairrosDistancias["Jardim Paraíso"] = 19.0;
        bairrosDistancias["Profipo"] = 19.5;
        bairrosDistancias["Lourdes"] = 19.8;
        bairrosDistancias["Petrópolis"] = 20.0;
        bairrosDistancias["Itinga"] = 20.5;
        bairrosDistancias["Paranaguamirim"] = 21.0;
        bairrosDistancias["Ulysses Guimarães"] = 22.0;
        bairrosDistancias["Vila Cubatão"] = 23.0;
        bairrosDistancias["Ribeirão do Cubatão"] = 24.0;
        
        // BAIRROS ADICIONAIS (se necessário)
        bairrosDistancias["Guanabara"] = 16.0; // Estimativa para ponto intermediário
        
        // NOTA: Distâncias são aproximadas e podem variar conforme rota e trânsito
        // Valor mínimo para reembolso: 10km (definido na constante DISTANCIA_MINIMA_REEMBOLSO)
    }
    
    public bool ValidarRota(string origem, string destino, out double distancia, out string mensagem)
    {
        distancia = 0;
        mensagem = "";
        
        // Verificar se um dos endereços é Perini
        bool origemEhPerini = string.Equals(origem, ENDERECO_PERINI, StringComparison.OrdinalIgnoreCase);
        bool destinoEhPerini = string.Equals(destino, ENDERECO_PERINI, StringComparison.OrdinalIgnoreCase);
        
        if (!origemEhPerini && !destinoEhPerini)
        {
            mensagem = "ERRO: Um dos endereços deve ser 'Perini'!";
            return false;
        }
        
        // Determinar qual bairro usar para calcular distância
        string bairroParaCalcular = origemEhPerini ? destino : origem;
        
        if (bairrosDistancias.ContainsKey(bairroParaCalcular))
        {
            distancia = bairrosDistancias[bairroParaCalcular];
            mensagem = "Rota válida!";
            return true;
        }
        else
        {
            mensagem = $"ERRO: Bairro '{bairroParaCalcular}' não encontrado na lista de bairros!";
            return false;
        }
    }
    
    public bool EhElegivelParaReembolso(double distancia)
    {
        return distancia >= DISTANCIA_MINIMA_REEMBOLSO;
    }
    
    public List<string> ObterBairrosDisponiveis()
    {
        var bairros = new List<string>(bairrosDistancias.Keys);
        bairros.Add(ENDERECO_PERINI);
        bairros.Sort();
        return bairros;
    }
    
    public double ObterDistanciaBairro(string bairro)
    {
        if (bairrosDistancias.ContainsKey(bairro))
        {
            return bairrosDistancias[bairro];
        }
        return 0;
    }
    
    public string ObterEnderecoPerini()
    {
        return ENDERECO_PERINI;
    }
}