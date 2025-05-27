# Starter App Clean Architecture

## Installer le template

```bash
git clone https://github.com/mon-org/MyStarterTemplate.git #(TODO CHANGE TO REAL REPO)
dotnet new install .\CleanArchiStarterTemplate\
```

## Mettre à jour le template

```bash
dotnet new uninstall # Check for the name of the template and re exec uninstall with it
dotnet new install ./CleanArchiStarterTemplate\
```

## Utiliser le template

```bash
# Créer le template et remplace toutes les occurences de 'Starter' par 'MonAppli'
dotnet new cleanarch -n MonAppli
```

## Stack technique

### Développement

- [ErrorOr](https://github.com/amantinband/error-or)
- [MediatR](https://github.com/jbogard/MediatR)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)

### Tests

- xUnit
- [NSubstitute](https://nsubstitute.github.io/)
- [FluentAssertions](https://fluentassertions.com/)
