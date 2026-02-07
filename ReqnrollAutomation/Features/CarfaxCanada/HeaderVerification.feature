@regression
Feature: Header Verification

As a user
I want to ensure the header links are functioning properly
So that I can navigate to the correct pages from the header

#Scenario: Carfax Canada logo redirects to the home page
#	Given I am on any page
#	When I click on the Carfax Canada logo
#	Then I should be redirected to the home page

Scenario Outline: Vehicle History links redirect to the correct pages
	Given I am on the CARFAX Canada home page
	When I hover over the Vehicle History section in the header
	And I click on the <sub_section> subsection in the header
	Then the <sub_section> link should navigate to "<expected_URL>"

Examples:
	| sub_section             | expected_URL                                                 |
	| Vehicle History Reports | https://www.carfax.ca/vehicle-history/vehicle-history-report |
	| View a Sample Report    | https://www.carfax.ca/vehicle-history/sample-report          |

Scenario Outline: Vehicle Fraud links redirect to the correct pages
	Given I am on the CARFAX Canada home page
	When I hover over the Vehicle Fraud section in the header
	And I click on the <sub_section> subsection in the header
	Then the <sub_section> link should navigate to "<expected_URL>"

Examples:
	| sub_section                     | expected_URL                                          |
	| What is VIN Fraud?              | https://www.carfax.ca/what-is-vin-fraud               |
	| VIN Fraud Check                 | https://www.carfax.ca/vin-fraud-check                 |
	| Vehicle Monitoring Subscription | https://www.carfax.ca/vehicle-monitoring-subscription |