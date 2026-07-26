@regression @swaglabs @footer
Feature: Footer Component Verification

As a user
I want to ensure that the footer component of the Swag Labs application displays the correct copyright information and social media links
So that I can access the relevant information and navigate to the company's social media pages

Background:
	Given I am on the Swag Labs login page
	And I am logged in as standard user

@smoke
Scenario: Swag Labs footer copyright information is displayed correctly
	Then the Swag Labs footer copyright text should be visible
	And the Swag Labs footer copyright text should display correctly

Scenario Outline: Social media links in the Swag Labs footer are functional and navigate to the correct URLs
	Given the Swag Labs footer contains a link to <platform> social media page
	When I click on the Swag Labs <platform> social media link
	Then the Swag Labs <platform> link should navigate to "<expected_URL>"

Examples:
	| platform | expected_URL                                 |
	| Twitter  | https://x.com/saucelabs                      |
	| Facebook | https://www.facebook.com/saucelabs           |
	| LinkedIn | https://www.linkedin.com/company/sauce-labs/ |
