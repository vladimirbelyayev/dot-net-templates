Create SQLLite DB (Only if not already present)
```
dotnet ef migrations add Initial --project FabelConnect --context WebApiTemplate.Connectors.Database.WebApiTemplateDbContext -o Connectors/Database/Migrations
```

Update databse to latest
```
dotnet ef database update --project FabelConnect --context WebApiTemplate.Connectors.Database.WebApiTemplateDbContext
```
