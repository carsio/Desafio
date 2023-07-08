
# Documentação da API de Autenticação

## Endpoints

### GET /api/auth/user

Retorna o usuário associado ao token fornecido.

#### Request

Headers:

- `Authorization`: `Bearer <token>`

#### Response

Retorna um objeto `UserDto` que representa o usuário.

```json
{
  "email": "string",
  "phoneNumber": "string",
  "isPhoneVerified": "boolean"
}
```

### POST /api/auth/register

Registra um novo usuário.

#### Request

Body:

```json
{
  "email": "string",
  "password": "string",
  "phoneNumber": "string",
}
```

#### Response

Retorna o id do usuário recém-criado.

```json
9620c7f1-4ea2-4aa8-92ab-26a2e608621a
```

### POST /api/auth/token

Obtém um token para um usuário existente.

#### Request

Body:

```json
{
  "email": "string",
  "password": "string"
}
```

#### Response

Retorna um objeto `TokenDto` que contém o token do usuário.

```json
{
  "token": "string"
}
```

### GET /api/auth/error

Endpoint de teste que lança uma exceção.

#### Request

Nenhum parâmetro necessário.

#### Response

Lança uma exceção.

## Exemplos de uso

Para usar esses endpoints, você pode usar uma ferramenta como o `curl` ou um cliente HTTP como o Postman.

Aqui está um exemplo de como você pode chamar o endpoint `POST /api/auth/register` com o `curl`:

```bash
curl -X POST -H "Content-Type: application/json" -d '{"email":"test@example.com","password":"password","phoneNumber":"1234567890","verificationCode":123456}' http://localhost:5000/api/auth/register
```

Substitua `http://localhost:5000` pelo URL base do seu aplicativo.

Lembre-se de que você precisa substituir os valores de exemplo nos exemplos de solicitação por valores reais.


# Documentação da API do WeatherForecastController

Este é um controlador de API mockado que retorna previsões do tempo. Ele é usado principalmente para validar o `JwtAuthorizationFilter`, que é um filtro personalizado que verifica se o token JWT fornecido na solicitação é válido.

## Endpoints

### GET /api/weatherforecast

Retorna uma lista de previsões do tempo.

#### Request

Headers:

- `Authorization`: `Bearer <token>`

#### Response

Retorna uma lista de objetos `WeatherForecast`. Cada objeto `WeatherForecast` representa uma previsão do tempo e contém as seguintes propriedades:

- `date`: A data da previsão do tempo.
- `temperatureC`: A temperatura em graus Celsius.
- `summary`: Uma descrição textual da previsão do tempo.

```json
[
  {
    "date": "2023-07-07",
    "temperatureC": 24,
    "summary": "Mild"
  },
  {
    "date": "2023-07-08",
    "temperatureC": 30,
    "summary": "Hot"
  },
  // ...
]
```

## Exemplos de uso

Para usar este endpoint, você pode usar uma ferramenta como o `curl` ou um cliente HTTP como o Postman.

Aqui está um exemplo de como você pode chamar o endpoint `GET /api/weatherforecast` com o `curl`:

```bash
curl -X GET -H "Authorization: Bearer <token>" http://localhost:5000/api/weatherforecast
```

Substitua `<token>` pelo token JWT que você obteve ao autenticar com o endpoint `POST /api/auth/token`. Substitua `http://localhost:5000` pelo URL base do seu aplicativo.

Lembre-se de que você precisa substituir os valores de exemplo nos exemplos de solicitação por valores reais.