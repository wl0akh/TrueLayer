Feature: GetPokemon

    Scenario: Success with valid pokemon Name
        Given a valid pokemon name as pikachu
        And pokemon has pikachu details
        And translate api is up
        When get on pokemon end point is called
        Then 200 status is return
        And response should come from origin
        And response body contains pokimon model

    Scenario: Success with valid pokemon Name from cache
        Given a valid pokemon name as pikachu
        And pokemon has pikachu details
        And translate api is up
        And get on pokemon end point is already called
        When get on pokemon end point is called
        Then 200 status is return
        And response should come from cache
        And response body contains pokimon model

    Scenario: Failure invalid pokemon Name
        Given a invalid pokemon name as pikachu
        When get on pokemon end point is called
        Then 404 status is return