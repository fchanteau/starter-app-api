# NSwag

## 📄 Présentation

- NSwag permet d’automatiser la génération de clients HTTP fortement typés.
- Utile pour des projets front-end (Angular, React) ou d'autres back-ends (C#).
- Peut s’intégrer dans une pipeline CI/CD.

## 🔧 Utilisation

Dans ce starter, on utilise NSwag pour générer les **Input** / **Outputs** de notre API en types Typescript.

Cela nous oblige à bien documenter nos endpoints API avec les décorateurs correspondants.

Il y a une tâche **Post-build** qui s'execute qui effectue cette opération (voir [Starter.Webapi.csproj](../CleanArchiStarterTemplate/Starter.WebApi/Starter.WebApi.csproj))

La configuration pour NSwag se trouve dans le fichier [config.nswag](../CleanArchiStarterTemplate/Starter.WebApi/config.nswag)
