# 🚀 AdvancedDevSample

Bienvenue dans la documentation officielle du projet **AdvancedDevSample**.
Ce projet est une API RESTful construite avec **.NET 8**, suivant les principes du **Domain-Driven Design (DDD)** et de la **Clean Architecture**.

[![CI/CD & Documentation](https://github.com/Syhard87/AdvancedDevSample/actions/workflows/ci-cd.yml/badge.svg)](https://github.com/Syhard87/AdvancedDevSample/actions/workflows/ci-cd.yml)

## 🗺️ Vue d'ensemble du système

Voici comment l'application s'intègre dans son environnement :

```mermaid
graph LR
    User([Utilisateur / Client]) -- HTTPS + JWT --> API[API .NET 8]
    API -- Lecture/Écriture --> DB[(Base de Données SQLite)]
    API -- Logs --> Console[Sortie Console]

    style API fill:#6A0DAD,stroke:#333,stroke-width:2px,color:white
    style DB fill:#008000,stroke:#333,stroke-width:2px,color:white
    style User fill:#FFA500,stroke:#333,stroke-width:2px,color:white