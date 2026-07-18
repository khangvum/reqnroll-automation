@regression
Feature: Accessibility Verification

As a user
I want to ensure the language and accessibility toggles are functioning correctly
So that I can use the application in my preferred language and with accessibility features enabled

Background:
	Given I am on the CARFAX Canada home page

@smoke
Scenario Outline: Language toggle switches between English and French
	Given the home page is displayed in <initial_language>
	When I click on the language toggle
	Then the home page should switch to <resulting_language>
	And the main heading should be "<main_heading>"

Examples:
	| initial_language | resulting_language | main_heading                                                   |
	| English          | French             | Le rapport d’historique de véhicule le plus complet au Canada. |
	| French           | English            | Canada’s most comprehensive vehicle history report.            |

