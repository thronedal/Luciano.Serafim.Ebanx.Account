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
