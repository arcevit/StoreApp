# Assignment

## Before starting your work

- Choose the solution and technology stack you are most comfortable with.
    - This repository contains a `.NET` project for a Product and Ingredients API.
    - A `Node-based` solution is also available. If you prefer this option, open the project located in the `store-app-node` folder.
- Before starting, we recommend that you familiarize yourself with the existing project.
- The solutions contains some setup that you can use and build upon while solving the issues stated below.
    - It is allowed to make your own project from scratch, if this is choosen a README file should be available
      which describes what is required to setup the project and run it.
- This assignment is intended to show how you approach everyday tasks as a developer.
    - The focus of this assignment is on your decision-making, structure, and communication.
    - Please write comments to reflect your thoughts throughout the implementation.
- If AI tools are used during development, please document:
    - What the tool was used for.
    - How you evaluated and applied the generated output.
- Clone this repository to your own private GitHub repository and invite evh@onlimited.dk as a collaborator before the deadline specified in the email.
- If you have any questions, feel free to reach out to us at the same email address as above.

Good luck!

## Implement the following

### 1. Add CRUD endpoints for Products
Create CRUD endpoints for managing ingredients in the application. This includes:
- Get details of a specific product
- Get a list of all products
- Create a new product
- Update an existing product
- Delete a product

### 2. Add CRUD endpoints for Ingredients
Create CRUD endpoints for managing ingredients in the application. This includes:
- Get a list of ingredients
- Get details of a specific ingredient
- Create a new ingredient
- Update an existing ingredient
- Delete an ingredient
- NOT REQUIRED - Seed database with ingredients on first startup.

### 3. Add functionalty to assign and delete ingredients to a product
- A product can have 0-N ingredients.
- An ingredient can be available for multiple products.
- It is important that only one ingredient can be removed at a time.
- NOT REQUIRED - Seed products in database with ingredients on first startup.

### 4. Add validation and feedback logic
- Add fitting validation logic in CRUD functionality for both ingredient and product.
- Add fitting feedback for CRUD endpoints for both ingredient and product.

### 5. Update InStock for ingredients.
For individual ingredients it should be possible to see if the ingredient is in stock or not.
Create functionality to set the `InStock` value of an ingredient to true or false.
Create functionality to set the `InStock` value for all existing ingredients to true or false.

### 6. Check availability for products.
Currently, the `Product` model has an `Available` property, but it is not working.
Implement functionality to inform whether a product is available or not.
This depends on whether all ingredients assigned to the product are in stock.

### 7. Add price to a product
Add price to the `Product` model and the database entity.
Update necessary functions to handle price when creating and updating a product.
Implement fitting validation for price.
