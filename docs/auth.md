# 🔐 Sécurité & Authentification

L'API est sécurisée via le standard **JWT (JSON Web Token)**. Elle est "Stateless" : le serveur ne garde pas de session en mémoire, tout est contenu dans le token signé.

## Flux d'Authentification (Workflow)

Ce diagramme montre comment un utilisateur obtient son accès et comment l'API valide ses requêtes suivantes.

```mermaid
sequenceDiagram
    autonumber
    participant Client as Utilisateur (Postman/Swagger)
    participant API as API (.NET 8)
    participant DB as Base de Données (SQLite)

    Note over Client, API: 1. Phase de Connexion
    Client->>API: POST /api/auth/login {email, password}
    API->>DB: Vérifie l'existence et le hash du mot de passe
    
    alt Identifiants Valides
        DB-->>API: Utilisateur OK
        API->>API: Génération du Token JWT (Signé avec clé secrète)
        API-->>Client: 200 OK + { "token": "ey..." }
    else Identifiants Invalides
        API-->>Client: 401 Unauthorized
    end

    Note over Client, API: 2. Phase d'Accès aux Ressources
    Client->>API: GET /api/products (Header: Bearer ey...)
    API->>API: Vérification de la signature et expiration du Token
    
    alt Token Valide
        API->>DB: Récupération des données
        DB-->>API: Liste des produits
        API-->>Client: 200 OK + [JSON Data]
    else Token Expiré ou Falsifié
        API-->>Client: 401 Unauthorized (Accès refusé)
    end