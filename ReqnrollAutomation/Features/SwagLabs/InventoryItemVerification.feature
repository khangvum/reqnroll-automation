@regression @swaglabs @inventory
Feature: Inventory Item Verification

As a user
I want to ensure that the inventory items on the Swag Labs product page display accurate details and correct pricing
So that I can successfully browse and verify products before making a purchase

Background:
	Given I am on the Swag Labs login page
	And I am logged in as standard user

@smoke
Scenario: Adding inventory items reflects correct details and pricing in the cart
	When I add inventory items to the cart
	And I navigate to the cart page
	Then the cart should display the correct number of items added
	And the items' details and pricing should match the product page
