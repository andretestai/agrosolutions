🌱 AgroSolutions – Plataforma de Agricultura de Precisão
MVP de uma plataforma IoT para monitoramento agrícola inteligente, desenvolvido com arquitetura de microsserviços em .NET 8.

📋 Sumário

Visão Geral
Arquitetura
Justificativa Técnica
Microsserviços
Requisitos
Como Rodar
Testes
CI/CD
Observabilidade
Endpoints


🎯 Visão Geral
A AgroSolutions é uma cooperativa agrícola que busca modernizar sua operação através da Agricultura 4.0. Esta plataforma permite que produtores rurais monitorem suas propriedades em tempo real, recebendo alertas automáticos sobre condições críticas como seca e risco de pragas.

🏗️ Arquitetura
A solução é baseada em microsserviços independentes, cada um com sua responsabilidade bem definida, comunicando-se de forma assíncrona via RabbitMQ.
[Produtor Rural]
       ↓
[Swagger / API]
       ↓
┌─────────────────────────────────────────┐
│           Kubernetes (minikube)          │
│                                         │
│  [IdentityService]  [PropertyService]   │
│       :5001              :5002          │
│                                         │
│  [IngestionService] [AlertService]      │
│       :5003              :5004          │
└─────────────────────────────────────────┘
       ↓                    ↑
[RabbitMQ :5672] ──────────┘
  fila: sensor.data

┌─────────────────────────────┐
│       Observabilidade        │
│  [Prometheus] → [Grafana]   │
│    :9090          :3000     │
└─────────────────────────────┘

[GitHub Actions CI/CD]

🧠 Justificativa Técnica
Microsserviços
Cada serviço foi desenhado com responsabilidade única (SRP), permitindo escalabilidade, manutenção e deploy independentes. A separação entre Identidade, Propriedades, Ingestão e Alertas reflete os contextos delimitados do domínio agrícola.
.NET 8
Escolhido por ser a versão LTS mais recente da plataforma, oferecendo alta performance, suporte nativo a containers e ecossistema maduro para desenvolvimento de APIs REST.
RabbitMQ
Utilizado para desacoplar o IngestionService do AlertService. Quando um sensor envia dados, o IngestionService publica na fila sensor.data e o AlertService consome de forma assíncrona, garantindo que a ingestão não seja bloqueada pelo processamento de alertas.
Kubernetes (minikube)
Orquestração dos containers garantindo alta disponibilidade, escalabilidade horizontal e gerenciamento declarativo da infraestrutura via manifests YAML.
Prometheus + Grafana
Stack de observabilidade que coleta métricas de todos os serviços (requisições HTTP, uso de CPU, memória) e as exibe em dashboards visuais em tempo real.
Listas em Memória
Para o MVP, optou-se por listas em memória ao invés de banco de dados, reduzindo a complexidade de infraestrutura e focando na demonstração da arquitetura e dos fluxos de negócio.
GitHub Actions
Pipeline de CI/CD automatizado que garante qualidade do código a cada push, executando build, testes unitários e build das imagens Docker.

🔧 Microsserviços
ServiçoPortaResponsabilidadeIdentityService5001Registro e login de produtores rurais com JWTPropertyService5002CRUD de propriedades e talhõesIngestionService5003Recebimento de dados de sensores via API RESTAlertService5004Processamento de regras e geração de alertas

📦 Requisitos

.NET 8 SDK
Docker Desktop
minikube
kubectl


🚀 Como Rodar
Com Docker Compose (recomendado)
bash# Sobe todos os serviços
docker-compose up --build
Serviços disponíveis:

IdentityService: http://localhost:5001/swagger
PropertyService: http://localhost:5002/swagger
IngestionService: http://localhost:5003/swagger
AlertService: http://localhost:5004/swagger
RabbitMQ Painel: http://localhost:15672 (guest/guest)
Prometheus: http://localhost:9090
Grafana: http://localhost:3000 (admin/admin)


Com Kubernetes (minikube)
bash# Inicia o minikube
minikube start

# Build das imagens
docker build -t identity-service:latest ./IdentityService
docker build -t property-service:latest ./PropertyService
docker build -t ingestion-service:latest ./IngestionService
docker build -t alert-service:latest ./AlertService

# Carrega imagens no minikube
minikube image load identity-service:latest
minikube image load property-service:latest
minikube image load ingestion-service:latest
minikube image load alert-service:latest

# Aplica os manifests
kubectl apply -f k8s/

# Verifica os pods
kubectl get pods

# Expõe os serviços
minikube service identity-service --url
minikube service property-service --url
minikube service ingestion-service --url
minikube service alert-service --url

🧪 Testes
O projeto possui 19 testes unitários cobrindo todos os microsserviços.
bashdotnet test AgroSolution.Tests/AgroSolution.Tests.csproj --verbosity normal
Cobertura dos testes
ServiçoTestesIdentityService5 testesPropertyService6 testesIngestionService4 testesAlertService4 testes

⚙️ CI/CD
Pipeline automatizado com GitHub Actions que executa a cada push na branch main:

✅ Restaurar dependências
✅ Build da solução
✅ Rodar testes unitários
✅ Build das imagens Docker


📊 Observabilidade
Prometheus
Coleta métricas de todos os serviços automaticamente via endpoint /metrics.
Grafana
Acesse http://localhost:3000 com admin/admin e visualize:

Total de requisições HTTP por serviço
Uso de CPU e memória
Latência das requisições


📡 Endpoints Principais
IdentityService (5001)
MétodoEndpointDescriçãoPOST/api/auth/registerRegistra produtor ruralPOST/api/auth/loginLogin, retorna JWT
PropertyService (5002)
MétodoEndpointDescriçãoPOST/api/propertiesCria propriedadeGET/api/propertiesLista propriedadesPOST/api/properties/{id}/fieldsAdiciona talhãoGET/api/properties/{id}/fieldsLista talhões
IngestionService (5003)
MétodoEndpointDescriçãoPOST/api/sensors/dataEnvia leitura de sensorGET/api/sensors/data/{fieldId}Histórico de leituras
AlertService (5004)
MétodoEndpointDescriçãoGET/api/alertsLista todos os alertas ativosGET/api/alerts/{fieldId}Alertas de um talhão

🔔 Regras de Alerta
RegraCondiçãoAlerta GeradoSecaUmidade do solo < 30% nas últimas 24hAlerta de SecaPragaTemperatura > 40°CRisco de Praga

👨‍💻 Desenvolvido para
Hackathon 8NETT – PosTech
