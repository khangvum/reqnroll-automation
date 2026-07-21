@smoke @regression
Feature: Footer Component Verification

As a user
I want to ensure that the footer component of the Swag Labs application displays the correct copyright information and social media links
So that I can access the relevant information and navigate to the company's social media pages

Background:
	Given I am on the Swag Labs login page
	When I log in as standard user

Scenario: Footer copyright information is displayed correctly
	Then the footer copyright text should be visible
	And the footer copyright text should display correctly

Scenario Outline: Social media links in the footer are functional and navigate to the correct URLs
	Then the footer should contain a link to <platform>
	And the <platform> link should navigate to "<expectedURL>"

Examples:
	| platform | expectedURL                                  |
	| Twitter  | https://x.com/saucelabs                      |
	| Facebook | https://www.facebook.com/saucelabs           |
	| LinkedIn | https://www.linkedin.com/company/sauce-labs/ |
