## UserMicroserviceTest

Pasta com o testes criados para a Api.

Nesta tabela pode ser alterado os dados para os dados que gostaria de inserir no teste.

Scenario: Register user with name and email
    Given filling in the registration of a user with name and email as follows
    |name         |email                   |
    |teste        |teste@teste.com         | //podera ser alterado esses dados para executar o seu teste
    When i send the request POST
    Then the user named with email must be registered


