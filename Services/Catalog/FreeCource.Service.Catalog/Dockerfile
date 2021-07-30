FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim as build
WORKDIR /app
EXPOSE 80
COPY Shared/FreeCourse.Shared/*.csproj Shared/FreeCourse.Shared/
COPY Services/Catalog/FreeCourse.Service.Catalog/*.csproj Services/Catalog/FreeCourse.Service.Catalog/
RUN dotnet restore Services/Catalog/FreeCourse.Service.Catalog/*.csproj
COPY . .
RUN dotnet publish Services/Catalog/FreeCourse.Service.Catalog/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet","FreeCourse.Service.Catalog.dll" ]
