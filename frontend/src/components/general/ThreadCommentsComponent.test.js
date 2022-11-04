import {CommentOrder, CommentsClient} from "../../api";
import {internalsForRenderer, renderWithProviders} from "./__test-utilities__/testRenders";
import {ThreadCommentsComponent} from "./ThreadCommentsComponent";
import renderer from "react-test-renderer";
import {fireEvent, screen} from "@testing-library/react";

let commentsMock;
beforeEach(() => {
    commentsMock = jest.spyOn(CommentsClient.prototype, 'comments')
        .mockImplementation(() => Promise.resolve({comments: []}));
});
afterEach(() => {
    commentsMock.mockRestore();
});

const commentContent1 = "This is for testing1";
const commentContent2 = "This is for testing2";
const commentContent3 = "This is for testing3";
function createState(commentOrder = CommentOrder.OldestFirst, fetchingComments = false) {
    return {
        threadsReducer: {
            threadsById: {
                1: {
                    commentOrder: commentOrder,
                    postingComment: false,
                    numberOfComments: 0,
                    fetchingComments: fetchingComments
                }
            }
        },
        commentsReducer: {
            commentsById: {
                1: {
                    id: 1,
                    authorName: "Test Testerson",
                    content: commentContent1,
                    likes: 0,
                    likedByCurrentUser: false,
                    isChangingLikeStatus: false
                },
                2: {
                    id: 2,
                    authorName: "Test Testerson",
                    content: commentContent2,
                    likes: 0,
                    likedByCurrentUser: false,
                    isChangingLikeStatus: false
                },
                3: {
                    id: 3,
                    authorName: "Test Testerson",
                    content: commentContent3,
                    likes: 0,
                    likedByCurrentUser: false,
                    isChangingLikeStatus: false
                }
            },
            threadIdToCommentIds: {
                1: [1, 2, 3]
            }
        }
    }
}

function DropDownText(commentOrder) {
    if (commentOrder === CommentOrder.NewestFirst) {
        return "Newest first";
    } else {
        return "Oldest first";
    }
}

function renderComponent(commentOrder, fetchingComments) {
    renderWithProviders(
        <ThreadCommentsComponent threadId={1}/>,
        {preloadedState: createState(commentOrder, fetchingComments)});

    const dropdown = screen.getByText(DropDownText(commentOrder));

    return {dropdown};
}

// Needed for CommentCreationComponent.
function createNodeMock(element) {
    return document.createElement('div');
}

test('renders correctly', () => {
    const internalDom = internalsForRenderer(
        <ThreadCommentsComponent threadId={1}/>,
        {preloadedState: createState()});
    const tree = renderer.create(internalDom, {createNodeMock}).toJSON();
    expect(tree).toMatchSnapshot();
});

describe('comment order changed,', () => {
    test('when oldest first is selected, gets comments with the oldest first parameter', async () => {
        // Arrange
        const {dropdown} = renderComponent(CommentOrder.NewestFirst);

        // Act
        fireEvent.click(dropdown);
        await new Promise(process.nextTick);

        const oldestFirstOption = screen.getByText("Oldest first");
        fireEvent.click(oldestFirstOption);

        // Assert
        expect(commentsMock).toBeCalledWith(1, CommentOrder.OldestFirst);
    });

    test('when newest first is selected, gets comments with the newest first parameter', async () => {
        // Arrange
        const {dropdown} = renderComponent(CommentOrder.OldestFirst);

        // Act
        fireEvent.click(dropdown);
        await new Promise(process.nextTick);

        const newestFirstOption = screen.getByText("Newest first");
        fireEvent.click(newestFirstOption);

        // Assert
        expect(commentsMock).toBeCalledWith(1, CommentOrder.NewestFirst);
    });
});

describe('loading spinner', () => {
    test('when not fetching comments for thread, is not rendered', () => {
        // Arrange
        renderComponent(CommentOrder.OldestFirst, false);

        // Assert

        expect(screen.queryByTestId('fetchingCommentsSpinner')).toBeNull();
    });

    test('when fetching comments for thread, is rendered', async () => {
        // Arrange
        renderComponent(CommentOrder.OldestFirst, true);

        // Assert
        const component = screen.getByTestId('fetchingCommentsSpinner');
        expect(component).toHaveStyle('display: block');
    });
});

test('comments, when present in the store, are rendered', () => {
    // Arrange
    renderComponent(CommentOrder.OldestFirst, false);

    // Assert
    expect(screen.getByText(commentContent1)).toBeTruthy();
    expect(screen.getByText(commentContent2)).toBeTruthy();
    expect(screen.getByText(commentContent3)).toBeTruthy();
});
