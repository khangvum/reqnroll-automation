@regression @carfaxcanadawebsite @header
Feature: Header Component Verification

As a user
I want to ensure the header links are functioning properly
So that I can navigate to the correct pages from the header

@smoke
Scenario: Header navigation links redirect to their correct pages
	Given I am on the CARFAX Canada home page
	Then all header links should navigate to their expected destinations

Scenario: Clicking the CARFAX Canada logo redirects to the homepage from any subpage
	Given I am on a random CARFAX Canada subpage
	When I click on the CARFAX Canada logo
	Then I should be redirected to the CARFAX Canada home page
