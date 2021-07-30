FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim as build
WORKDIR /app
EXPOSE 80
COPY Shared/FreeCourse.Shared/*.csproj Shared/FreeCourse.Shared/
COPY Services/Catalog/FreeCource.Service.Catalog/*.csproj Services/Catalog/FreeCource.Service.Catalog/
RUN dotnet restore Services/Catalog/FreeCource.Service.Catalog/*.csproj
COPY . .
RUN dotnet publish Services/Catalog/FreeCource.Service.Catalog/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet","FreeCource.Service.Catalog.dll" ]
