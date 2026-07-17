@smoke @regression
Feature: User Authentication Verification

As a QA engineer
I want to test the login page's user authentication functionality with different account types
So that I can confirm that user authentication functionality works as expected for various scenarios

Background:
	Given I am on the Swag Labs login page

Scenario Outline: Log in with different account types
	When I log in as "<account_type>" user
	Then <expected_outcome>

Examples:
	| account_type | expected_outcome                   |
	| standard     | I should be logged in successfully |
	| locked-out   | I should see a lockout message     |
	| invalid      | I should see an error message      |