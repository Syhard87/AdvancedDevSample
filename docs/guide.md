# Guide du Développeur (CI/CD)

Ce guide explique comment fonctionne le pipeline d'intégration continue et comment lancer le projet en local.

## Pipeline CI/CD (`GitHub Actions`)

Le fichier `.github/workflows/ci-cd.yml` définit l'automatisation du projet. Il se déclenche à chaque `push` sur la branche `main`.

### Étapes du Pipeline

Le pipeline est visualisé ci-dessous. Il s'assure que le code est propre avant de déployer la documentation.

```mermaid
graph LR;
    Start((Push sur Main)) --> Checkout;
    
    subgraph Build & Test
        Checkout[Récupération Code] --> Setup[.NET Setup];
        Setup --> Restore[Restore Nuget];
        Restore --> Build[Compilation];
        Build --> Test[Tests Unitaires];
    end

    subgraph Deploy
        Test --> CheckMain{Sur Main ?};
        CheckMain -- Oui --> DeployDocs[Deploy MkDocs];
        CheckMain -- Non --> Stop((Fin));
    end

    style Build & Test fill:#e1f5fe,stroke:#01579b,stroke-width:2px;
    style Deploy fill:#e8f5e9,stroke:#1b5e20,stroke-width:2px;
```

1.  **Build & Test** :
    - Installe .NET 8.
    - Restaure les dépendances (`dotnet restore`).
    - Compile le projet (`dotnet build`).
    - Lance les tests (`dotnet test`). **Si les tests échouent, le déploiement est annulé.**

2.  **Deploy Docs** :
    - Ne s'exécute que si les tests passent ET qu'on est sur la branche `main`.
    - Installe Python et MkDocs.
    - Publie la documentation sur GitHub Pages via `mkdocs gh-deploy`.

## Guide de Démarrage Local

Pour travailler sur ce projet en local, suivez ces instructions.

### Prérequis
- .NET SDK 8.0
- Un éditeur de code (Visual Studio, VS Code, Rider)

### Lancer l'API
```bash
# Aller dans le dossier de l'API
cd AdvancedDevSample.Api

# Lancer l'application
dotnet run
```
L'API sera accessible sur `https://localhost:7001` (ou port similaire). Swagger est disponible sur `/swagger`.

### Lancer les Tests
```bash
# Aller à la racine de la solution
dotnet test
```

### Visualiser la Documentation
Assurez-vous d'avoir Python installé.
```bash
# Installer MkDocs et le thème Material
pip install mkdocs-material

# Lancer le serveur de documentation
mkdocs serve
```
La documentation sera accessible sur `http://127.0.0.1:8000`.