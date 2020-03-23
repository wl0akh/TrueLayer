Feature: GetPokemon

    Scenario: Success with valid pokemon Name
        Given a valid pokemon name
        When get on pokemon end point is called
        Then 200 status is return
        And response body contains name
        And response body contains description

    Scenario: Failure invalid pokemon Name
        Given a invalid pokemon name
        When get on pokemon end point is called
        Then 404 status is return