@regression @carfaxcanadawebsite @footer
Feature: Footer Component Verification

As a user
I want to ensure that the footer component of the CARFAX Canadas website displays the correct copyright information and links
So that I can access the relevant information and navigate to the company's links

Background:
	Given I am on the CARFAX Canada home page

@smoke
Scenario: CARFAX Canada website disclaimer information is displayed correctly
	Then the CARFAX Canada website disclaimer text should be visible
	And the CARFAX Canada website disclaimer text should display correctly

Scenario: Footer navigation links redirect to their correct pages
	Then all footer links should navigate to their expected destinations

Scenario: Social media links in the CARFAX Canada website footer are functional and navigate to the correct URLs
	Given the CARFAX Canada website footer contains links to social media pages
	Then all CARFAX Canada website social media links should navigate to their expected destinations