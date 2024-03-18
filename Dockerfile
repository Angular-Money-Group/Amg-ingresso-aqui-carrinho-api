FROM mcr.microsoft.com/dotnet/sdk:7.0 as build
WORKDIR /app
COPY . .
RUN dotnet restore
RUN dotnet publish -o /app/published-app

FROM mcr.microsoft.com/dotnet/aspnet:7.0 as runtime
WORKDIR /app
COPY --from=build /app/published-app /app
##config 
RUN apt-get update
RUN apt-get install -y locales
RUN sed -i -e 's/# pt_BR.UTF-8 UTF-8/pt_BR.UTF-8 UTF-8/' /etc/locale.gen && \
    locale-gen
ENV LC_ALL pt_BR.UTF-8 
ENV LANG pt_BR.UTF-8  
ENV LANGUAGE pt_BR:pt

ENTRYPOINT [ "dotnet", "/app/Amg-ingressos-aqui-carrinho-api.dll" ]