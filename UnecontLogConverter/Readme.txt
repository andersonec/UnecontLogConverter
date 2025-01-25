Desenvolvido por:
	Anderson Martins dos Santos

Utlizando alguns conceitos de boas práticas de programação.
Eu particularmente gosto de trabalhar com algo parecido com arquitetura em camadas, onde possuímos as camadas:
	Infra - Gerenciamento de persistência de dados e configurações.
	Services - Serviços de interação do usuário com configuraçoes de segurança para evitar uso direto das entidades de banco.
	Presentations - Camada final, onde são disponibilizados os serviços para integração do cliente.

Mapeamento entre classes
	Mapeamento de Entidades x Models com Auto Mapper;

Persistência de Dados
	Utilizando o Entity Framework Core para persistência de dados por ser um projeto simples, visando um desenvolvimento agilizado.
	Mas para queries mais robustas eu utilizaria o Dapper, pois o EFCore possui uma grande limitação na montagem
	de queries, e em questão de performance o Dapper também se sai bem melhor.


** INFORMAÇÕES PARA BUILD **
	Podem rodar diretamente o projeto, ele vai gerar o banco a partir de Migrations.

	Para deploy em um servidor com docker:

		docker-compose up -d --build

	Todas as dependências estão configuradas, então é só rodar o comando

** IMPORTANTE **
	Mudem a ConnectionString do banco de dados em appsettings.Development.json.
	No appsettings.json deixei configurado para trabalhar com vari�veis de ambiente, caso queiram configurar em um servidor, por exemplo 
	a Library dos Pipelines do Azure DevOps.