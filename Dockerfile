FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /api

COPY src/Backend ./Backend
COPY src/Shared ./Shared

WORKDIR Backend/MyRecipeBook.Api

RUN dotnet publish -c Release -o /api/output

FROM mcr.microsoft.com/dotnet/aspnet:10.0

WORKDIR /api

COPY --from=build /api/output .

# roda como usuário sem privilégios (boa prática de segurança)
USER app

ENTRYPOINT ["dotnet", "MyRecipeBook.Api.dll"]