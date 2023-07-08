# Documentação da API de Validação de Número de Telefone

## Endpoints

### POST /api/phonenumbervalidation

Este endpoint é usado para validar um código de verificação recebido via SMS.

#### Request

O corpo da solicitação deve ser um JSON que representa o objeto `VerificationCodeDto`:

```json
{
    "code": 123456
}
```

O cabeçalho da solicitação deve incluir o token JWT no campo `Authorization`:

```
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

#### Response

O endpoint faz uma chamada gRPC para a API de Usuários para validar o código de verificação. Se a validação for bem-sucedida, o endpoint retornará um status HTTP 200 e um objeto JSON que representa o resultado da validação:

```json
{
    "valid": true,
    "error": ""
}
```

Se a validação falhar, o campo `valid` será `false` e o campo `error` conterá uma mensagem de erro.

### Notas

Este endpoint faz uma chamada gRPC para a API de Usuários para validar o código de verificação. A URL do servidor gRPC é `https://localhost:5001`, mas isso pode ser alterado dependendo da configuração do seu ambiente.

O objeto `PhoneNumberValidation.PhoneNumberValidationClient` é usado para fazer a chamada gRPC. Este objeto é gerado a partir do arquivo de definição de serviço gRPC (`.proto`).