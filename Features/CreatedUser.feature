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
        |name         |email                   |
        |Joao Thomaz O|joaoasdf@teste.com      |
        When i send the request post
        Then the user named with email must be registered


@mytag
Scenario: User registration with email only should not be allowed
    Given filling in the registration of a user with email as follows
    |email              |
    |test@test.com      |
    When i send the request post
    Then the system should respond with a BadRequest status code and message "The name field is required"
    