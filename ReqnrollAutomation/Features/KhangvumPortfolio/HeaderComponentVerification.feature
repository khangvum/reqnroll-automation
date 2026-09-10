@regression @khangvumportfolio @header
Feature: Header Component Verification

As a vistor
I want to ensure the header links are functioning properly
So that I can navigate to the correct sections from the header

@smoke
Scenario: Header navigation links scroll to their correct sections
	Given I am on the Khangvum Portfolio page
	Then all header links should navigate to their expected sections
