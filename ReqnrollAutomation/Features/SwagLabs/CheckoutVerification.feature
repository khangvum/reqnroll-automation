@regression @swaglabs @checkout
Feature: Checkout Verification

As a user
I want to ensure that the checkout process on the Swag Labs platform functions correctly
So that I can successfully complete purchases and receive accurate order confirmations

Background:
	Given I am on the Swag Labs login page
	And I am logged in as standard user
	When I add inventory items to the cart
	And I navigate to the cart page

@smoke
Scenario: Completing the checkout process successfully finishes the order
	When I proceed to the checkout information page
	And I provide valid checkout details
	And I click the continue button
	And I finish the checkout overview
	Then the checkout completion page should display a successful order confirmation message
