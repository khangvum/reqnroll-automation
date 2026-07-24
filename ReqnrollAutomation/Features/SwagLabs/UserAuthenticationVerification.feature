@regression @userauthentication
Feature: User Authentication Verification

As a QA engineer
I want to test the login page's user authentication functionality with different account types
So that I can confirm that user authentication functionality works as expected for various scenarios

Background:
	Given I am on the Swag Labs login page

@smoke
Scenario: Logging in as standard user is successful
	When I log in as standard user
	Then I should be logged in successfully

Scenario Outline: Logging in as other account types displays appropriate error messages
	When I log in as <account_type> user
	Then I should see a(n) <error_type> message

Examples:
	| account_type | error_type          |
	| locked-out   | lockout             |
	| invalid      | invalid credentials |