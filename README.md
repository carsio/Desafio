# Descrição do Projeto

Este projeto consiste em duas APIs .NET Core: a API de Usuários (Core) e a API de Notificação (Notification).

A API de Usuários é responsável pelo gerenciamento de usuários, incluindo registro, autenticação e recuperação de informações do usuário. A API de Notificação é responsável por enviar notificações para os usuários, como mensagens SMS para validação de números de telefone.

O fluxo de chamadas de endpoints é o seguinte:

1. **Registro de Usuário**: O usuário se registra usando o endpoint `api/auth/register` na API de Usuários. As informações do usuário, incluindo email, senha e número de telefone, são fornecidas no corpo da solicitação.

2. **Obtenção de Token**: Após o registro bem-sucedido, o usuário pode obter um token JWT usando o endpoint `api/auth/token`. O token JWT é usado para autenticar o usuário nas solicitações subsequentes.

3. **Recuperação de Informações do Usuário**: O usuário pode recuperar suas informações usando o endpoint `api/auth/user`. Este endpoint retorna as informações do usuário, incluindo se o número de telefone do usuário foi verificado (`isPhoneVerified`).

4. **Envio de SMS**: Se o usuário forneceu um número de telefone durante o registro, a API de Notificação enviará um SMS para o número de telefone do usuário com um código de 6 dígitos para validação.

5. **Validação de Número de Telefone**: O usuário pode validar seu número de telefone usando o endpoint `api/phonenumbervalidation/` na API de Notificação. O usuário fornece o código de 6 dígitos recebido via SMS e o token JWT no cabeçalho da solicitação. A API de Notificação confirma a validação fazendo uma chamada gRPC para a API de Usuários.

6. **Verificação de Autenticação**: O usuário pode verificar se está autenticado usando o endpoint `api/weatherforecast` na API de Usuários. Este endpoint retorna informações mockadas sobre o tempo e é útil para verificar se o usuário está autenticado.

Após a validação do número de telefone, o campo `isPhoneVerified` nas informações do usuário retornadas pelo endpoint `api/auth/user` será `true`.

Este projeto demonstra como implementar um sistema de autenticação e autorização usando JWT, como enviar notificações via SMS, como validar números de telefone e como usar gRPC para comunicação entre microserviços.

Para mais informações sobre a API acesse [Documentação da API de Autenticação](Core/README.md) e [Documentação da API de Notificação](Notification/README.md).

## Pré-requisitos

- .NET 7.0 SDK
- Uma conta do Azure com uma instância do Cosmos DB
- Uma conta do Azure com uma instância do Service Bus
- Uma conta do Twilio

## Configuração

1. Clone o repositório:

```bash
git clone https://github.com/carsio/desafiob.git
```

2. Navegue até o diretório do projeto:

```bash
cd desafiob
```

3. Atualize as strings de conexão e as configurações JWT e Twilio nos arquivos appsettings.json dos projetos Core e Notification.

No projeto Core:

```json
"ConnectionStrings": {
  "CosmosDBConnection": "<sua-string-de-conexão-do-cosmosdb>",
  "ServiceBusConnection": "<sua-string-de-conexão-do-servicebus>"
},
"JWT": {
  "Key": "<sua-chave-secreta>",
  "Issuer": "http://localhost:5000",
  "Audience": "http://localhost:5000"
}
```

No projeto Notification:

```json
"ConnectionStrings": {
  "ServiceBusConnection": "<sua-string-de-conexão-do-servicebus>"
},
"Twilio": {
  "AccountSid": "<seu-twilio-account-sid>",
  "AuthToken": "<seu-twilio-auth-token>",
  "FromPhoneNumber": "<seu-twilio-phone-number>"
}
```

## Instalação

1. Instale as dependências do projeto:

```shell
dotnet restore
```

2. Compile o projeto

```bash
dotnet build
```

## Execução

1. Inicie o projeto Core:

```bash
cd Core
dotnet run
```

2. Inicie o projeto Notification em um novo terminal:

```bash
cd Core
dotnet run
```

## Testes

Para executar os testes, navegue até o diretório do projeto de teste e execute o comando `dotnet test`:


```bash
cd Test
dotnet test
```

# Melhorias / Implementações Futuras

## Testes Unitários

Atualmente, o projeto possui alguns testes unitários, mas a cobertura de testes ainda pode ser melhorada. Planejamos adicionar mais testes unitários para garantir que todas as partes críticas do código estejam sendo testadas adequadamente. Isso ajudará a identificar e corrigir bugs mais cedo no ciclo de desenvolvimento.

## Mockar Ambientes Externos

Nos testes de integração, estamos atualmente interagindo com serviços externos reais, como o Cosmos DB e o Service Bus. Isso pode levar a alguns problemas, como a lentidão dos testes e a dependência de serviços externos para os testes. Planejamos mockar esses ambientes externos nos testes de integração para tornar os testes mais rápidos e independentes.

## Dockerização da Aplicação

No momento, a aplicação precisa ser instalada e configurada manualmente em cada ambiente. Isso pode ser um processo demorado e propenso a erros. Para resolver isso, planejamos dockerizar a aplicação. Isso permitirá que a aplicação seja facilmente instalada e executada em qualquer ambiente que suporte o Docker, simplificando o processo de implantação e garantindo a consistência entre os ambientes de desenvolvimento, teste e produção.