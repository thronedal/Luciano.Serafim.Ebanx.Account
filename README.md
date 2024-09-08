# Luciano.Serafim.Ebanx.Account

Aplicação para processo seletivo Ebanx

## Estrutura

Asolução é desenvolvida em .Net 8, e utiliza banco de dados mongoDB.

A solução foi criada utilizando os conceitos de clean architeture, e a divisão dos projetos foi simplificada ficando:

- Api: Controllers;
- Core: Abstrações, models/entidades, UseCases, e Classes utilitárias;
- Infrastructure: Implementações de abstrações como por exemplo persistência;
- Bootstrap: projeto transversal que efetua a ligação entre as camandas;
- Tests: testes unitários.


## Possíveis problemas

Tive um erro ao utilizar volume para o Mongo no compose, o mongo não inicializava por problema de permissão para gravação

utilizando Ubunto 22.04.3

```yaml
  mongodb:
    image: mongodb/mongodb-community-server:latest
    ports:
      - "27017:27017"
    volumes:
      - './.data:/data/db'
```

## executar através do compose

A api deve inciar na seguinte [url](http://localhost:8080/swagger)

```bash
docker compose -f "compose.yml" up -d --build
```

## configurações

As configurações estão todas baseadas na execução via docker-compose, ou seja, utilizam o endereço de dentro da rede criada para o compose.

- [appsettings.json](Luciano.Serafim.Ebanx.Account.Api/appsettings.json)

Para utilizar o mongoDb utilize a configuração, a ausencia da chave "MongoDb" implica no uso de armazenamento em memória:

```json
{
  "MongoDb": {
    "Host": "172.31.255.105",
    "Port": "27017",
    "DatabaseName": "Ebanx"
  }
}
```

### extraído do log

> {"t":{"$date":"2024-09-08T14:42:33.384+00:00"},"s":"E",  "c":"STORAGE",  "id":22312,   "ctx":"initandlisten","msg":"Error creating journal directory","attr":{"directory":"/data/db/journal","error":"boost::filesystem::create_directory: Permission denied [system:13]: \"/data/db/journal\""}}

### solução

dar permissão na pasta local (./.data)

```bash
sudo chmod 777 ./.data
sudo chmod 777 ./.keycloak
```
