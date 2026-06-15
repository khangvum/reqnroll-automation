@smoke @regression
Feature: Footer Component Verification

Test the presence and functionality of the footer component on the inventory page

Background:
	Given I am on the Swag Labs login page
	When I enter standard user credentials

Scenario: Verify footer copyright information
	Then the footer copyright text should be visible
	And the footer copyright text should display correctly

Scenario Outline: Verify social media links in the footer
	Then the footer should contain a link to "<socialMediaPlatform>"
	And the "<socialMediaPlatform>" link should navigate to the correct URL

	Examples:
	| socialMediaPlatform | expectedURL									 |
	| Twitter             | https://twitter.com/saucelabs				 |
	| Facebook            | https://www.facebook.com/saucelabs			 |
	| LinkedIn            | https://www.linkedin.com/company/sauce-labs/ |