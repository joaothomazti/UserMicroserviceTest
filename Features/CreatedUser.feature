Feature: Register User

@mytag
    Scenario: Register user with name only
        Given filling in the registration of a user with name as follows
        |name       |
        |teste User |
        When i send the request POST
        Then the user named must be registered


@mytag
    Scenario: Register user with name and email
        Given filling in the registration of a user with name and email as follows
        |name         |email                   |
        |teste        |teste@teste.com         |
        When i send the request POST
        Then the user named with email must be registered


@mytag
Scenario: User registration with email only should not be allowed
    Given filling in the registration of a user with email as follows
    |email              |
    |test@test.com      |
    When i send the request POST
    Then the system should respond with a BadRequest status code and message "The name field is required"
    

@mytag
Scenario: Update user email
    Given an existing user with the following details and I send a PUT request to update the user's email
    | id | name        | newEmail              |
    | 1  | teste       | teste@newemail.com    |
    Then the system should respond with a status code 200
    And the user's email should be updated to "teste@newemail.com"



@mytag
Scenario: User delete
    Given an existing user with the following details
    | id | name          |
    | 4  | teste User    |
    When I send a DELETE request to delete the user with ID 4
    Then the system should respond with a successful deletion status code and message "UserDeleted"
    And the user with ID 4 should no longer exist