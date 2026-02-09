# 🏗️ Architecture Logicielle

Le projet **AdvancedDevSample** repose sur les principes de la **Clean Architecture** (aussi connue sous le nom d'Architecture Hexagonale). L'objectif principal est de séparer le cœur métier des détails techniques.

## Diagramme de Dépendances

Le diagramme ci-dessous illustre le flux de dépendance strict : **Le centre (Domain) ne dépend de rien.** Les couches externes dépendent des couches internes.

```mermaid
graph TD
    subgraph Core [Cœur Métier]
        Domain[Layer: Domain<br/>(Entités, Règles, Interfaces)]
        Application[Layer: Application<br/>(Services, DTOs, Use Cases)]
    end

    subgraph Infra [Détails Techniques]
        Infrastructure[Layer: Infrastructure<br/>(EF Core, Repositories, JWT)]
        API[Layer: API<br/>(Controllers, Middleware)]
    end

    API --> Application
    API --> Infrastructure
    Infrastructure --> Application
    Application --> Domain
    Infrastructure --> Domain

    style Domain fill:#f9f,stroke:#333,stroke-width:2px,color:black
    style Application fill:#bbf,stroke:#333,stroke-width:2px,color:black
    style Infrastructure fill:#dfd,stroke:#333,stroke-width:2px,color:black
    style API fill:#ffd,stroke:#333,stroke-width:2px,color:black