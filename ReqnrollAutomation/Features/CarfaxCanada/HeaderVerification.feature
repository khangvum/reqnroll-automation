@regression @carfaxcanada @header
Feature: Header Verification

As a user
I want to ensure the header links are functioning properly
So that I can navigate to the correct pages from the header

@smoke
Scenario: All header navigation links redirect to their correct pages
	Given I am on the CARFAX Canada home page
	Then all header links should navigate to their expected destinations
