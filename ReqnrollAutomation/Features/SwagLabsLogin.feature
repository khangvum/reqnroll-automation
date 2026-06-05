@smoke
Feature: Swag Labs Login

Test the Swag Labs login page

Scenario: Log in with valid credentials
	Given I am on the Swag Labs login page
	When I enter valid credentials
	Then I should be logged in successfully

Scenario: Log in with invalid credentials
	Given I am on the Swag Labs login page
	When I enter invalid credentials
	Then I should see an error message