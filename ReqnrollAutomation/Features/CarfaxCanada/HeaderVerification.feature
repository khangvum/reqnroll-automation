@regression @carfaxcanada @header
Feature: Header Verification

As a user
I want to ensure the header links are functioning properly
So that I can navigate to the correct pages from the header

@smoke
Scenario Outline: Header navigation links redirect to the correct pages
	Given I am on the CARFAX Canada home page
	When I hover over the <section> section in the header
	And I click on the <sub_section> subsection in the header
	Then the <sub_section> link should navigate to "<expected_URL>"

Examples: Vehicle History Links
	| section         | sub_section             | expected_URL                                                 |
	| Vehicle History | Vehicle History Reports | https://www.carfax.ca/vehicle-history/vehicle-history-report |
	| Vehicle History | View a Sample Report    | https://www.carfax.ca/vehicle-history/sample-report          |

Examples: Vehicle Fraud Links
	| section       | sub_section                     | expected_URL                                          |
	| Vehicle Fraud | What is VIN Fraud?              | https://www.carfax.ca/what-is-vin-fraud               |
	| Vehicle Fraud | VIN Fraud Check                 | https://www.carfax.ca/vin-fraud-check                 |
	| Vehicle Fraud | Vehicle Monitoring Subscription | https://www.carfax.ca/vehicle-monitoring-subscription |

Examples: What's My Car Worth Links
	| section             | sub_section         | expected_URL                                                 |
	| What’s My Car Worth | Car Value           | https://www.carfax.ca/whats-my-car-worth/car-value/ymm       |
	| What’s My Car Worth | History Based Value | https://www.carfax.ca/whats-my-car-worth/history-based-value |

Examples: Tools Links
	| section | sub_section  | expected_URL                             |
	| Tools   | VIN Decoder  | https://www.carfax.ca/tools/vin-decode   |
	| Tools   | Recall Check | https://www.carfax.ca/tools/recall-check |
	| Tools   | Car Care     | https://www.carfax.ca/Service            |

Examples: Resources Links
	| section   | sub_section | expected_URL                              |
	| Resources | Learn       | https://www.carfax.ca/learn               |
	| Resources | Support     | https://support.carfax.ca/en/support/home |