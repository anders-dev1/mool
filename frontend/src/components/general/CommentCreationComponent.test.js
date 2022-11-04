import {internalsForRenderer, renderWithProviders} from "./__test-utilities__/testRenders";
import {CommentCreation} from "./CommentCreationComponent";
import renderer from "react-test-renderer";
import {fireEvent, act, screen} from "@testing-library/react";
import {CommentOrder, CommentsClient} from "../../api";

const validCommentContent = "This is a test comment";
const mockedThreadId = 1;
let commentCreateMock;
beforeEach(() => {
    commentCreateMock = jest.spyOn(CommentsClient.prototype, 'create')
        .mockImplementation(() => {
            // We need this delay for the "submit button when submitting should show loading spinner" test.
            // Otherwise, the loading spinner will never show.
            return new Promise((resolve) => setTimeout(resolve, 100));
        });

    // Must mock this because the updateCommentsForThread action is called when a comment is posted.
    jest.spyOn(CommentsClient.prototype, 'comments')
        .mockImplementation(() => Promise.resolve({comments: []}));
});

afterEach(() => {
    commentCreateMock.mockRestore();
});

function createState(){
    return {
        threadsReducer: {
            threadsById: {
                1: {
                    commentOrder: CommentOrder.OldestFirst,
                    postingComment: false,
                    numberOfComments: 0
                }
            }
        }
    }
}

function renderForm() {
    renderWithProviders(
        <CommentCreation threadId={mockedThreadId}/>,
        {preloadedState: createState()});

    const textArea = screen.getByRole("textbox");
    const submitButton = screen.getByRole("button");

    return {textArea, submitButton}
}

// The nested TextArea component needs a div passed as a forwardRef to figure out how tall it should be.
// This is not necessary for this test which is why we just pass a simple div through the createNodeMock option in
// react-test-renderer.
function createNodeMock(element) {
    return document.createElement('div');
}
test('renders correctly', () => {
    const internalDom = internalsForRenderer(
        <CommentCreation threadId={1}/>,
        {preloadedState: createState()});

    const tree = renderer.create(internalDom, {createNodeMock}).toJSON();
    expect(tree).toMatchSnapshot();
});

describe('submit button clicked,', () => {
    test('when form valid, should post form to backend with provided information', async () => {
        // Arrange
        const {textArea, submitButton} = renderForm();

        // Act
        await act(async () => {
            fireEvent.change(textArea, {target: {value: validCommentContent}});
            await new Promise(process.nextTick);

            fireEvent.click(submitButton);
            await new Promise(process.nextTick);
        });

        // Assert
        expect(commentCreateMock).toBeCalledWith(mockedThreadId, validCommentContent);
    });

    test('when form valid, should show loading spinner', async () => {
        // Arrange
        const {textArea, submitButton} = renderForm();

        // Act
        await act(async () => {
            fireEvent.change(textArea, {target: {value: validCommentContent}});
            await new Promise(process.nextTick);

            fireEvent.click(submitButton);
            await new Promise(process.nextTick);
        });

        // Assert
        const spinner = screen.getByTestId('spinner');
        expect(spinner).toHaveStyle('display: flex');
    });

    test('should reset form', async () => {
        // Arrange
        const {textArea, submitButton} = renderForm();

        // Act
        await act(async () => {
            fireEvent.change(textArea, {target: {value: validCommentContent}});
            await new Promise(process.nextTick);

            fireEvent.click(submitButton);
            await new Promise(process.nextTick);
        });

        // Assert
        expect(textArea.value).toEqual("");
    });

    test('when no comment content specified, should show validation error', async () => {
        // Arrange
        const {textArea, submitButton} = renderForm();

        // Act
        await act(async () => {
            fireEvent.click(submitButton);
            await new Promise(process.nextTick);
        });

        // Assert
        const element = screen.getByText("Required");
        expect(element).toBeTruthy();
    });

    test('when comment content shorter than 10 characters, should show validation error', async () => {
        // Arrange
        const {textArea, submitButton} = renderForm();

        // Act
        await act(async () => {
            fireEvent.change(textArea, {target: {value: "9chars000"}});
            await new Promise(process.nextTick);

            fireEvent.click(submitButton);
            await new Promise(process.nextTick);
        });

        // Assert
        const element = screen.getByText("Must be 10 or more characters");
        expect(element).toBeTruthy();
    });
});