Feature: Register User

@mytag
    Scenario: Register user with name only
        Given filling in the registration of a user with name as follows
        |name       |
        |Joao Thomaz|
        When i send the request post
        Then the user named must be registered



@mytag
    Scenario: Register user with name and email
        Given filling in the registration of a user with name and email as follows
        |name       |email               |
        |Joao Thomaz|joao@teste.com      |
        When I send the request post
        Then the user named with email must be registered