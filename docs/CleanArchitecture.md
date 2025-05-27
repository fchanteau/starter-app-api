# Clean Architecture dans une Application .NET

## Introduction

La Clean Architecture est une approche architecturale qui vise à organiser le code d'une application de manière modulaire, maintenable et testable. Elle est conçue pour séparer les préoccupations en différentes couches bien définies.

## Principes Clés

- **Séparation des préoccupations** : Chaque couche a une responsabilité distincte.
- **Dépendances dirigées vers l'intérieur** : Les couches internes ne dépendent pas des couches externes.
- **Testabilité** : Les composants peuvent être testés indépendamment.
- **Indépendance vis-à-vis des frameworks** : L'application n'est pas liée à un framework spécifique.

## 📌 Les 4 Couches Principales

La Clean Architecture est divisée en quatre couches principales qui suivent le principe des dépendances dirigées vers l’intérieur (les couches externes ne doivent pas influencer les couches internes).

### 🎯 1. Domain (Cœur Métier)

✅ Rôle :

- Contient les entités métier et les règles métier les plus fondamentales.
- N'a aucune dépendance vers d'autres couches.

🛑 Ne contient pas de frameworks, de dépendances externes (comme Entity Framework ou MediatR).

### ⚙️ 2. Application (Cas d’Utilisation)

✅ Rôle :

- Contient la logique métier spécifique sous forme de Cas d’Utilisation (Use Cases).
- Utilise MediatR pour gérer les requêtes.
- Dépend uniquement de la couche Domain.

🛑 Ne contient pas d’accès aux bases de données ni de logique liée à l’infrastructure.

### 🏛 3. Infrastructure (Accès aux Données & Services Externes)

✅ Rôle :

- Implémente les interfaces définies dans Application.
- Contient les implémentations des repositories, les APIs externes, la configuration de la base de données (EF Core).

🛑 Ne contient pas de logique métier (juste de l’implémentation technique).

### 🌍 4. WebAPI (Interface Utilisateur & API)

✅ Rôle :

- Contient les Controllers, les Middlewares, les Configurations de l’API.
- Utilise Application pour exécuter la logique métier.
- Utilise MediatR pour transmettre les requêtes.

🛑 Ne contient pas de logique métier, seulement la gestion des requêtes/réponses.

### 🔄 Flux des Dépendances

📊 Les couches suivent le principe des dépendances dirigées vers l’intérieur :

- 1️⃣ WebAPI envoie une requête HTTP via un Controller
- 2️⃣ Application gère la requête via MediatR et les Use Cases
- 3️⃣ Infrastructure récupère les données via Entity Framework et les repositories
- 4️⃣ Application renvoie la réponse à WebAPI
- 5️⃣ WebAPI retourne la réponse au client

### 📌 Diagramme des dépendances

```css
[ WebAPI ] → [ Application ] → [ Domain ]
                    ↑
            [ Infrastructure ]
```

- 👉 WebAPI dépend de Application
- 👉 Application dépend de Domain
- 👉 Infrastructure dépend de Application et Domain

## ✅ Pourquoi utiliser la Clean Architecture ?

| 🏆 **Avantage**                   | 🚀 **Explication**                                                          |
| --------------------------------- | --------------------------------------------------------------------------- |
| **Maintenabilité**                | Les couches sont indépendantes et faciles à modifier.                       |
| **Testabilité**                   | On peut tester `Application` sans dépendre de `WebAPI` ou `Infrastructure`. |
| **Scalabilité**                   | Facile d'ajouter de nouvelles fonctionnalités sans impacter l'architecture. |
| **Indépendance des technologies** | On peut changer la base de données, gestions de mails, ...                  |

---

## 📌 Résumé des Couches

| **Couche**         | **Responsabilité**                             | **Exemples**                                                   |
| ------------------ | ---------------------------------------------- | -------------------------------------------------------------- |
| **Domain**         | Logique métier de base (Entités, règles)       | `User.cs`                                                      |
| **Application**    | Cas d’utilisation (Handlers, DTOs, Interfaces) | `GetUserQuery.cs`, `GetUserQueryHandler.cs`, `IUserRepository` |
| **Infrastructure** | Accès aux données et services externes         | `UserRepository.cs`, `DbContext.cs`                            |
| **WebAPI**         | Interface utilisateur et API                   | `UserController.cs`, `RoleAuthorizationMiddleware.cs`          |

## 🚀 Conclusion

La Clean Architecture permet de structurer une application .NET de manière claire et évolutive. En suivant ces principes :

- ✅ On sépare la logique métier du code technique.
- ✅ On facilite les tests unitaires.
- ✅ On peut remplacer des technologies sans impacter toute l'application.
