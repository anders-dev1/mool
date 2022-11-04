const sharedPassword = "!TestUser1";

function signInOnLandingPage(email:string){
  cy.get('[data-testid="username"]').type(email);
  cy.get('[data-testid="password"]').type(sharedPassword);
  cy.get('[data-testid="submit"]').click();
}

it('can sign up and log in', () => {
  // Arrange
  cy.task('db:emptyDataSet');

  // Act
  cy.visit("localhost:3000");

  // Sign up
  cy.get('[data-testid="signup"]').click();

  const email = "signupandlogintestuser@testmail.com";

  cy.get('[data-testid="firstName"]').type("Test");
  cy.get('[data-testid="lastName"]').type("User");
  cy.get('[data-testid="email"]').type(email);
  cy.get('[data-testid="password"]').type(sharedPassword);
  cy.get('[data-testid="repeat-password"]').type(sharedPassword);

  cy.get('[data-testid="signupsubmit"]').click();

  cy.get('[data-testid="gotosignin"]').click();

  // Sign in
  signInOnLandingPage(email);

  // Assert that navbar and post functionality is visible.
  cy.contains('Log out').should('be.visible');
  cy.contains('What is on your mind?').should('be.visible');
});

it('can create a new thread', () => {
  // Arrange
  cy.task('db:userOnlyDataSet');

  // Act
  cy.visit("localhost:3000");
  signInOnLandingPage("testemail@UserOnlyDataSet.com");

  // Create thread
  cy.get('[data-testid="threadCreationTextArea"]').type("This is a test.");
  cy.get('[data-testid="threadCreationTextArea"]').should("have.value", "This is a test.");

  cy.get('[data-testid="submitThreadBtn"]').click();

  // Assert
  cy.get('[data-testid="threadCreationTextArea"]').should("have.value", "");
  cy.get('[data-testid="threadContent"]').first().should("have.text", "This is a test.");
});

it('can like and unlike thread', () => {
  // Arrange
  cy.task('db:userWithCreatedThreadSet');

  // Act + Assert
  cy.visit("localhost:3000");
  signInOnLandingPage("testemail1@UserWithCreatedThreadSet.com");
  cy.get('[data-testid="threadLikeBtn"]').first().find('[data-testid="number"]').should("contain", "0");
  cy.get('[data-testid="threadLikeBtn"]').first().click();
  cy.get('[data-testid="threadLikeBtn"]').first().find('[data-testid="number"]').should("contain", "1");
});

it('can comment', () => {
  // Arrange
  const commentText = "Hello, this is a comment test.";

  cy.task('db:userWithCreatedThreadSet');

  // Act
  cy.visit("localhost:3000");
  signInOnLandingPage("testemail1@UserWithCreatedThreadSet.com");
  cy.get('[data-testid="threadCommentBtn"]').click();

  cy.get('[data-testid="commentCreationTextArea"]').type(commentText);
  cy.get('[data-testid="commentCreationTextArea"]').should("have.value", commentText);

  cy.get('[data-testid="commentCreationPostBtn"]').click();

  // Assert
  cy.get('[data-testid="commentCreationTextArea"]').should("have.value", "");
  cy.get('[data-testid="commentContent"]').should("have.text", commentText);
});

it('can like and unlike comment', () => {
  // Arrange
  cy.task('db:userWithCreatedThreadWithCreatedCommentSet');

  // Act + Assert
  cy.visit("localhost:3000");
  signInOnLandingPage("testemail1@UserWithCreatedThreadWithCreatedComment.com");
  cy.get('[data-testid="threadCommentBtn"]').click();

  cy.get('[data-testid="commentLikeBtn"]').first().find('[data-testid="number"]').should("contain", "0");
  cy.get('[data-testid="commentLikeBtn"]').click();
  cy.get('[data-testid="commentLikeBtn"]').first().find('[data-testid="number"]').should("contain", "1");
});