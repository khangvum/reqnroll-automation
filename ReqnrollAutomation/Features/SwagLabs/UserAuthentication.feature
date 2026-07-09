@smoke @regression
Feature: User Authentication

Test the login page's user authentication functionality with different account types

Background:
	Given I am on the Swag Labs login page

Scenario: Log in as a standard user
	When I enter "standard" user credentials
	Then I should be logged in successfully

Scenario: Log in as a locked-out user
	When I enter "locked-out" user credentials
	Then I should see a lockout message

Scenario: Log in with invalid credentials
	When I enter "invalid" user credentials
	Then I should see an error message
