# Plataforma de Caronas Corporativas - Requisitos

## Requisitos Funcionais (RF)

### RN-001: Cadastro de Motorista e Veículo ✔
- RF-001: O sistema deve permitir o cadastro de um motorista mediante a apresentação de CNH válida.
- RF-002: O sistema deve gerar um ID único automaticamente para cada motorista cadastrado.
- RF-003: O sistema deve permitir o cadastro de um veículo, definindo sua Placa e Capacidade Máxima de Assentos.
- RF-004: O sistema deve garantir que a capacidade total de assentos (passageiros + motorista) não exceda o limite legal do veículo.

### RN-002: Controle de Duplicidade ✔
- RF-005: O sistema deve impedir o cadastro de colaboradores (motoristas ou passageiros) com matrícula idêntica.

### RN-003: Oferta de Rota ✔
- RF-006: O sistema deve permitir que um motorista defina uma Rota com Endereço de Partida e Chegada obrigatórios (sempre para a sede da empresa).
- RF-007: O sistema deve permitir que o motorista defina o Horário de Partida da Rota.
- RF-008: O sistema deve garantir que o Horário de Partida não possa ter variação superior a 15 minutos em relação ao horário padrão de trabalho do motorista.

### RN-004: Reserva de Assento
- RF-009: O sistema deve permitir a reserva de assento por um colaborador (passageiro elegível) em uma rota, desde que não seja o motorista da rota. ok 
- RF-010: O sistema deve atualizar imediatamente os assentos disponíveis após a confirmação de uma reserva. ok
- RF-011: O sistema deve calcular a Ocupação Média de passageiros por veículo em um determinado período. **(FALTA)**
- RF-012: O sistema deve executar um algoritmo para encontrar motoristas e passageiros com rotas e horários compatíveis (Pareamento de Rotas). ok
- RF-013: O sistema deve registrar a confirmação da presença dos passageiros (Check-in) no início da carona e a confirmação de chegada (Check-out) no final da carona. ok

### RN-005: Solicitação de Reembolso ✔
- RF-014: O sistema deve emitir o reembolso automaticamente após o check-in e check-out de 100% dos passageiros.
- RF-015: O sistema deve calcular o valor do reembolso com base na tabela fixa de custo por km previamente cadastrada.

### RN-006: Validação de Reembolso ✔
- RF-016: O sistema deve processar reembolsos apenas se a carona tiver sido realizada em um raio mínimo de 10 km da sede da empresa.

### RN-007: Níveis de Ocupação Mínima ✔
- RF-017: O sistema deve alertar o motorista se a carona tiver menos de 2 passageiros até 1 hora antes da partida (risco de cancelamento). 

### RN-008: Alertas Automáticos ✔
- RF-018: O sistema deve enviar uma notificação push para todos os envolvidos (Carona Iminente) 15 minutos antes do horário de partida.
- RF-019: O sistema deve enviar um alerta imediato ao Coordenador de Logística se uma Rota Crítica for cancelada.

### RN-009: Relatórios Obrigatórios
- RF-020: O sistema deve gerar relatórios obrigatórios contendo a frequência de uso por colaborador.
- RF-021: O sistema deve gerar relatórios obrigatórios contendo a distância total percorrida e os reembolsos emitidos.
Objetivos Secundários
- RF-022: O sistema deve gerar relatórios gerenciais sobre a pegada de carbono reduzida.
Objetivos Principais
- RF-023: O sistema deve fornecer visibilidade em tempo real dos assentos disponíveis por rota.
- RF-024: O sistema deve manter histórico completo e auditável de rotas e ocupação.
Requisitos Não Funcionais (RNF)

-------

### RNF de Usabilidade e Acessibilidade
- Usabilidade: RNF-001: O sistema deve ter uma interface que minimize a comunicação manual e a criação de grupos de caronas. (Baseado em: Problema Atual: Tempo excessivo na comunicação manual.)

### RNF de Desempenho e Performance
- Performance: RNF-002: O sistema deve garantir a redução de 90% no tempo para parear rotas. (Baseado em: Benefício Operacional.)
- Performance: RNF-003: O sistema deve garantir a redução de 40% no tempo para processar reembolsos de motoristas. (Baseado em: Benefício Operacional.)

### RNF de Segurança e Confiabilidade
- Confiabilidade: RNF-004: O sistema deve garantir 99% de eliminação dos erros de comunicação na confirmação de assentos. (Baseado em: Benefício Operacional.)
 - Confiabilidade: RNF-005: O sistema deve garantir 100% de rastreabilidade completa das caronas realizadas. (Baseado em: Benefício Operacional.)
- Confiabilidade: RNF-006: O sistema deve gerar Dados confiáveis para políticas de mobilidade urbana e melhoria no engajamento e clima organizacional. (Baseado em: Benefício Estratégico.)
- Segurança: RNF-007: A responsabilidade sobre o seguro do veículo é exclusiva do motorista (Esta é uma restrição do sistema, não um requisito de software, mas é uma premissa de segurança). (Baseado em: Premissa.)

### RNF de Integração e Tecnológicos
- Integração: RNF-008: O sistema deve integrar-se com Google Maps API para geolocalização (tracking) e roteamento. (Baseado em: Premissa.)
- Integração: RNF-009: O sistema deve ser base para futuras integrações com aplicativos de navegação. (Baseado em: Benefício Estratégico.)
- Integração: RNF-010: O sistema deve facilitar integrações com sistemas de crachá/acesso. (Baseado em: Objetivo Secundário.)

### RNF de Implantação e Operação
- Operação: RNF-011: O cadastro de CNH e documentação do veículo será validado manualmente pelo RH. (Baseado em: Premissa.)
- Limite: RNF-012: A Plataforma será utilizada por até 300 colaboradores elegíveis. (Baseado em: Premissa.)
