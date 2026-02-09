```markdown
# 👨‍💻 Guide du Développeur & CI/CD

Cette section détaille les processus d'automatisation mis en place pour garantir la qualité du code et la génération de cette documentation.

## 1. Pipeline CI/CD (GitHub Actions)

À chaque fois que du code est envoyé sur GitHub (`git push`), un robot exécute automatiquement les étapes suivantes pour valider le projet.

```mermaid
graph LR
    Dev[Développeur] -->|git push| GitHub[GitHub Repo]
    
    subgraph CI_CD [Pipeline GitHub Actions]
        direction TB
        Trigger(Déclencheur) --> JobTest[Job: Build & Test]
        JobTest --> Step1(Restaurer Nuget)
        Step1 --> Step2(Compiler .NET 8)
        Step2 --> Step3{Lancer les Tests}
        
        Step3 -->|Succès ✅| JobDoc[Job: Deploy Docs]
        Step3 -->|Échec ❌| Stop(Arrêt du Pipeline + Alerte)
        
        JobDoc --> GenDoc(Génération MkDocs)
        GenDoc --> Deploy(Publication Gh-Pages)
    end

    Deploy --> Web[Site de Documentation en ligne]
    
    style Step3 fill:#ff9,stroke:#333,stroke-width:2px
    style Web fill:#9f9,stroke:#333,stroke-width:2px