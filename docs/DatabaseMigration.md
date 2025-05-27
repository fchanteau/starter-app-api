# Mettre à jour le schéma de la base de données

## Entity Framework Migrations

### 🧱 Créer une migration

```bash
# on se met dans le projet Api car il a les dépendances nécessaires pour les migrations EF
cd ./Starter.Api

# On crée la migration dans le projet Infrastructure et le dossier Database/Migrations
dotnet ef --project ..\Starter.Infrastructure\ migrations add -o Database/Migrations NomDeLaMigration
```

### 📦 Appliquer une migration à la base de données

Pour appliquer la dernière migration crée, on a juste à executer la WebApi. Au démarrage, on applique toutes les migrations en attente à la base de données.

Voir le fichier [StarterContextInitializer.cs](../CleanArchiStarterTemplate/Starter.Infrastructure/Database/StarterContextInitializer.cs)

### 🔍 Vérifier l'état des migrations

```bash
# Affiche les migrations déjà créées.
dotnet ef migrations list
```

### 🧹 Annuler la dernière migration (non appliquée)

```bash
# Supprime la dernière migration si database update n’a pas encore été exécuté.
dotnet ef migrations remove
```

### ⚠️ Revenir à une migration spécifique

```bash
# Restaure l’état de la base à la migration indiquée.
dotnet ef database update NomDeLaMigration
```

### 🗃️ Générer un script SQL (optionnel)

```bash
# Produit un script SQL que vous pouvez exécuter manuellement (utile pour production).
dotnet ef migrations script -o migration.sql
```
