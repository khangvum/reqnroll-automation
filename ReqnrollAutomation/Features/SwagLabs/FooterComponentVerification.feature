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

Scenario: Social media links in the Swag Labs footer are functional and navigate to the correct URLs
	Given the Swag Labs footer contains links to social media pages
	Then all Swag Labs social media links should navigate to their expected destinations