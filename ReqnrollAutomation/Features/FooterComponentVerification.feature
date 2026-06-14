@smoke @regression
Feature: Footer Component Verification

Test the presence and functionality of the footer component on the inventory page

Background:
	Given I am on the Swag Labs login page
	When I enter standard user credentials

Scenario: Verify footer copyright information
	Then the footer copyright text should be visible
	And the footer copyright text should display correctly
