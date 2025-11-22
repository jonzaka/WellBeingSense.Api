# WellBeingSense.API
Plataforma de Monitoramento de Bem-Estar e Ambiente Organizacional

Projeto academico desenvolvido como parte da Global Solution (2o semestre) da FIAP, na disciplina de Software Development C#.

---

## 1. Visao Geral

O WellBeingSense.API e uma API REST desenvolvida em C# com ASP.NET Core, cujo objetivo e simular uma plataforma de monitoramento de bem-estar no trabalho.

A ideia central e combinar:

- Dados subjetivos dos funcionarios (humor, nivel de estresse, sintomas);
- Dados objetivos de sensores ambientais (ruido, temperatura, luminosidade);
- Um modulo de avaliacao de risco simples, que gera alertas automaticos;
- Endpoints para um futuro dashboard gerencial.

Isso se conecta diretamente ao tema "O Futuro do Trabalho", mostrando como a tecnologia pode apoiar a prevencao de burnout, melhorar ergonomia e transformar RH e Facilities em areas mais preditivas do que reativas.

---

## 2. O Problema (Por que este projeto existe)

O esgotamento (burnout), o estresse cronico e os problemas ergonomicos sao desafios reais no ambiente corporativo. Empresas muitas vezes:

- So descobrem o problema quando o funcionario ja esta afastado;
- Dependem apenas de pesquisas de clima anuais;
- Nao cruzam dados de ambiente (ruido, temperatura) com relatorios de saude e bem-estar.

Isso gera:

- Absenteismo;
- Baixa produtividade;
- Rotatividade;
- Custos elevados com saude e afastamentos.

O projeto propoe um esboco de solucao tecnica para gerar dados continuos e acionaveis.

---

## 3. A Solucao (O que a API faz)

A API do WellBeingSense foi pensada como o "cerebro" da plataforma. Ela contempla:

1. Registro de funcionarios (Employees);
2. Registro de check-ins de bem-estar (WellBeingCheckins), representando dados subjetivos;
3. Registro de leituras de ambiente (EnvironmentReadings), simulando sensores IoT;
4. Geracao automatica de alertas (RiskAlerts) com base em regras simples de risco;
5. Endpoints de dashboard para consultas gerenciais (resumo e alertas).

Regras principais implementadas:

- Se um funcionario faz check-in com:
  - StressLevel = Alto
  - Mood (humor) muito baixo (por exemplo, 1 ou 2)
  entao e gerado um alerta de possivel risco de burnout para o departamento;

- Se uma leitura de ambiente apresenta:
  - Ruido acima de 80 dB; ou
  - Temperatura acima de 28 graus Celcius;
  entao e gerado um alerta para a area fisica correspondente (ex.: "Ala B").

Essas regras sao simplificadas, mas ilustram como a correlacao entre ambiente e bem-estar pode ser feita.

---

## 4. Arquitetura da Solucao (Mermaid)

```mermaid
flowchart LR

    U[Funcionario]:::actor --> API
    S[Sensores IoT (simulados)]:::actor --> API
    HR[Gestor / RH]:::actor --> Dashboard

    subgraph API[WellBeingSense.API - ASP.NET Core]
        direction TB

        subgraph Controllers
            EmpCtrl[EmployeesController]
            CheckCtrl[WellBeingCheckinsController]
            EnvCtrl[EnvironmentReadingsController]
            DashCtrl[DashboardController]
        end

        subgraph Services
            RiskSvc[RiskEvaluationService]
        end
    end

    DB[(SQLite Database
Employees
WellBeingCheckins
EnvironmentReadings
RiskAlerts)]:::db

    API --> DB
    Services --> DB
    HR --> DashCtrl

    classDef actor fill:#1d3557,stroke:#ffffff,color:#ffffff;
    classDef db fill:#5e548e,stroke:#ffffff,color:#ffffff;
```

Esse diagrama mostra o fluxo geral:

- Funcionarios e sensores enviam dados para a API;
- Controllers recebem as requisicoes e usam o AppDbContext e o RiskEvaluationService;
- Os dados sao persistidos em um banco SQLite (local, simples para execucao);
- O gestor acessa os endpoints de dashboard para consultar resumo e alertas.

---

## 5. Tecnologias Utilizadas

- C# 9 / .NET 9;
- ASP.NET Core Web API;
- Entity Framework Core com SQLite;
- Swagger para documentacao e testes dos endpoints.

---

## 6. Como Rodar o Projeto

### 6.1. Requisitos

- .NET SDK 9.0 instalado;
- Editor de codigo (Visual Studio Code, Visual Studio, Rider, etc.).

### 6.2. Passos

1. Clonar o repositorio (ou extrair a pasta do projeto):

```bash
git clone https://github.com/SEU-USUARIO/WellBeingSense.Api.git
```

2. Acessar a pasta do projeto (onde esta o arquivo .csproj):

```bash
cd WellBeingSense.Api/wellbeingsense.api
```

3. Restaurar dependencias (opcional, o dotnet run ja faz isso automaticamente):

```bash
dotnet restore
```

4. Rodar a API:

```bash
dotnet run
```

5. Acessar o Swagger no navegador:

- URL padrao: http://localhost:5000/swagger

A partir do Swagger, e possivel testar todos os endpoints.

---

## 7. Modelos (Entidades Principais)

### 7.1. Employee

Representa um funcionario da empresa.

Campos principais:

- Id (int)
- Name (string)
- Department (string)
- Role (string)
- IsActive (bool)

### 7.2. WellBeingCheckin

Representa um check-in de bem-estar feito por um funcionario.

- Id (int)
- EmployeeId (int)
- Timestamp (DateTime)
- Mood (int, de 1 a 5)
- StressLevel (enum: Baixo, Medio, Alto)
- Symptoms (string)

### 7.3. EnvironmentReading

Representa uma leitura de ambiente (simulando sensores IoT).

- Id (int)
- Area (string)
- Timestamp (DateTime)
- NoiseLevelDb (double)
- TemperatureCelsius (double)
- LuminosityLux (double)

### 7.4. RiskAlert

Representa um alerta gerado automaticamente pela API.

- Id (int)
- GeneratedAt (DateTime)
- Target (string) - pode ser um departamento ou uma area fisica
- Severity (enum: Baixo, Medio, Alto)
- Message (string)

---

## 8. Endpoints Principais

### 8.1. Employees

- GET /api/v1/Employees  
  Lista todos os funcionarios cadastrados.

- GET /api/v1/Employees/{id}  
  Busca um funcionario pelo Id.

- POST /api/v1/Employees  
  Cria um novo funcionario.

Exemplo de corpo para POST:

```json
{
  "name": "Ana Silva",
  "department": "TI",
  "role": "Desenvolvedora",
  "isActive": true
}
```

### 8.2. WellBeingCheckins

- GET /api/v1/WellBeingCheckins  
  Lista os check-ins com o funcionario relacionado.

- POST /api/v1/WellBeingCheckins  
  Cria um novo check-in de bem-estar.

Exemplo de corpo para POST (gera alerta, dependendo dos valores):

```json
{
  "employeeId": 1,
  "mood": 2,
  "stressLevel": 3,
  "symptoms": "Dor de cabeca"
}
```

Se o nivel de estresse for Alto (3) e o humor baixo (por exemplo, 1 ou 2), o RiskEvaluationService cria um RiskAlert automaticamente.

### 8.3. EnvironmentReadings

- GET /api/v1/EnvironmentReadings  
  Lista as leituras ambientais mais recentes.

- POST /api/v1/EnvironmentReadings  
  Registra uma leitura de ambiente.

Exemplo de corpo:

```json
{
  "area": "Ala B",
  "noiseLevelDb": 90,
  "temperatureCelsius": 30.5,
  "luminosityLux": 150
}
```

Se o ruido passar de 80 dB ou a temperatura passar de 28 graus, um RiskAlert e gerado.

### 8.4. Dashboard

- GET /api/v1/Dashboard/alerts  
  Retorna a lista de alertas gerados, ordenados do mais recente para o mais antigo.

- GET /api/v1/Dashboard/summary  
  Retorna um resumo com:
  - Quantidade total de check-ins;
  - Quantidade de check-ins com estresse alto;
  - Media de humor e percentual de estresse alto por departamento.

---

## 9. Estrutura do Projeto

Estrutura simplificada do projeto:

```
WellBeingSense.Api/
 ├── Controllers/
 │   ├── EmployeesController.cs
 │   ├── WellBeingCheckinsController.cs
 │   ├── EnvironmentReadingsController.cs
 │   └── DashboardController.cs
 ├── Data/
 │   └── AppDbContext.cs
 ├── Models/
 │   ├── Employee.cs
 │   ├── WellBeingCheckin.cs
 │   ├── EnvironmentReading.cs
 │   ├── RiskAlert.cs
 │   └── Enums.cs
 ├── Services/
 │   └── RiskEvaluationService.cs
 ├── Program.cs
 └── appsettings.json
```

---

## 10. Conclusao

Este projeto demonstra uma API simples, mas completa, que simula uma plataforma de monitoramento de bem-estar alinhada ao tema "O Futuro do Trabalho".

A partir desta base, seria possivel integrar:

- Aplicativos mobile para coleta de check-ins;
- Dispositivos reais de IoT para leituras de ambiente;
- Dashboards mais visuais para RH, Facilities e gestao.

A ideia principal e mostrar que o uso combinado de dados subjetivos (como as pessoas se sentem) e dados objetivos (como o ambiente esta) pode apoiar decisoes mais humanas, preventivas e baseadas em evidencia.

---

## 11. Autor

Projeto desenvolvido por aluno de Engenharia de Software da FIAP, para fins academicos, na disciplina de Software Development C#.
