@regression @khangvumportfolio @footer
Feature: Footer Component Verification

As a vistor
I want to ensure that the footer component of the Khangvum Portfolio website displays the correct copyright information and links
So that I can access the relevant information and navigate to the author's links

Background:
	Given I am on the Khangvum Portfolio page

@smoke
Scenario: Social media links in the Khangvum Portfolio website footer are functional and navigate to the correct URLs
	Given the Khangvum Portfolio website footer contains links to social media pages
	Then all Khangvum Portfolio website social media links should navigate to their expected destinations
