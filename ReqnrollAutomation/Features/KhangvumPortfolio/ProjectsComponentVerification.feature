@regression @khangvumportfolio @projects
Feature: Projects Component Verification

As a visitor
I want to ensure the project cards are functioning properly
So that I can view the correct GitHub repositories when I click on a project card

@smoke
Scenario: Project cards navigate to their correct GitHub repositories
	Given I am on the Khangvum Portfolio page
	When I click on a random project card
	Then the project card should navigate to the correct GitHub repository
