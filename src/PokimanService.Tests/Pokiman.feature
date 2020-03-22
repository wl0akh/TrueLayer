Feature: GetPokiman

    Scenario: Success with valid pokiman Name
        Given a valid pokiman name
        When get on pokiman end point is called
        Then 200 status is return
        And response body contains name
        And response body contains description

    Scenario: Failure invalid pokiman Name
        Given a invalid pokiman name
        When get on pokiman end point is called
        Then 404 status is return