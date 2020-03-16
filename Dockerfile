# build
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as build
RUN mkdir app
WORKDIR /app
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o out

# run
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build /app/out/. .
CMD dotnet azure-academy.dll