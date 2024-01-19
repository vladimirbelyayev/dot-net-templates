dotnet ef migrations add Initial --project WebApiTemplate --context WebApiTemplate.Connectors.Database.WebApiTemplateDbContext -o Connectors/Database/Migrations
dotnet ef database update --project WebApiTemplate --context WebApiTemplate.Connectors.Database.WebApiTemplateDbContext

