# Guide du Développeur (CI/CD)

Ce guide explique comment fonctionne le pipeline d'intégration continue et comment lancer le projet en local.

## Pipeline CI/CD (`GitHub Actions`)

Le fichier `.github/workflows/ci-cd.yml` définit l'automatisation du projet. Il se déclenche à chaque `push` sur la branche `main`.

### Étapes du Pipeline

Le pipeline est visualisé ci-dessous. Il s'assure que le code est propre avant de déployer la documentation.

```mermaid
graph LR
    Start((Push sur Main)) --> Checkout
    
    subgraph Build_Test [Build & Test]
        Checkout[Récupération Code] --> Setup[.NET Setup]
        Setup --> Restore[Restore Nuget]
        Restore --> Build[Compilation]
        Build --> Test[Tests Unitaires]
    end

    subgraph Deploy [Deploiement]
        Test --> CheckMain{Sur Main ?}
        CheckMain -- Oui --> DeployDocs[Deploy MkDocs]
        CheckMain -- Non --> Stop((Fin))
    end

    style Build_Test fill:#e1f5fe,stroke:#01579b,stroke-width:2px
    style Deploy fill:#e8f5e9,stroke:#1b5e20,stroke-width:2px