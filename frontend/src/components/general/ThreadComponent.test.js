import {CommentOrder, CommentsClient, LikeThreadClient} from "../../api";
import {internalsForRenderer, renderWithProviders} from "./__test-utilities__/testRenders";
import {ThreadComponent} from "./ThreadComponent";
import renderer from "react-test-renderer";
import {fireEvent, screen} from "@testing-library/react";
import {testTheme} from "./__test-utilities__/TestTheme";
import {findActionPillByIcon, findActionPillNumberByIcon} from "./__test-utilities__/actionPillTestHelper";
import {findSpinner} from "./__test-utilities__/spinnerTestHelper";

const testThreadId = 1;
const testThreadAuthor = "Thread Threadson";
const testThreadContent = "The thread";
const testCommentContent = "The comment";

function createState({
                         threadLiked = false,
                         likeStatusChanging = false,
                         commentsToggledOn = false,
                         numberOfComments = 1
                     } = {}) {
    return {
        threadsReducer: {
            threadsById: {
                1: {
                    id: testThreadId,
                    author: testThreadAuthor,
                    content: testThreadContent,
                    likes: 0,
                    likedByCurrentUser: threadLiked,
                    numberOfComments: numberOfComments,
                    commentOrder: CommentOrder.OldestFirst,
                    isChangingLikeStatus: likeStatusChanging,
                    showingCommentSection: commentsToggledOn,
                    fetchingComments: false,
                    postingComment: false
                }
            }
        },
        commentsReducer: {
            commentsById: {
                1: {
                    id: 1,
                    authorName: "Test Testerson",
                    content: testCommentContent,
                    likes: 0,
                    likedByCurrentUser: false,
                    isChangingLikeStatus: false
                }
            },
            threadIdToCommentIds: {
                1: [1]
            }
        }
    }
}

function renderComponent(state = createState()) {
    renderWithProviders(
        <ThreadComponent isTest threadId={testThreadId}/>,
        {preloadedState: state});
}

test("renders correctly", () => {
    const internalDom = internalsForRenderer(
        <ThreadComponent isTest threadId={testThreadId}/>,
        {preloadedState: createState()});
    const tree = renderer.create(internalDom, {}).toJSON();
    expect(tree).toMatchSnapshot();
});

test("should show name of author", () => {
    // Arrange
    renderComponent();

    // Assert
    expect(screen.getByText(testThreadAuthor)).toBeVisible();
});

test("should show the content of the thread", () => {
    // Arrange
    renderComponent();

    // Assert
    expect(screen.getByText(testThreadContent)).toBeVisible();
});

describe("like action pill click,", () => {
    describe("when tread is not liked,", () => {
        const responseDelay = 100;
        let likeCallMock;
        beforeEach(() => {
            likeCallMock = jest.spyOn(LikeThreadClient.prototype, 'like')
                .mockImplementation(() => {
                    // We need this delay for the loading spinner test.
                    // Otherwise, the loading spinner will never show.
                    return new Promise((resolve) => setTimeout(resolve, responseDelay))
                });
        });

        afterEach(() => {
            likeCallMock.mockRestore();
        });

        test("should change action pill to liked color", async () => {
            // Arrange
            const state = createState();
            renderComponent(state);

            // Act
            const likeButton = findActionPillByIcon("👍");
            fireEvent.click(likeButton);
            await new Promise((resolve) => setTimeout(resolve, responseDelay));

            // Assert
            expect(findActionPillByIcon("👍")).toHaveStyle(`background-color: ${testTheme.successColor};`);
        });

        test("should call backend like endpoint", async () => {
            // Arrange
            const state = createState();
            renderComponent(state);

            // Act
            const likeButton = findActionPillByIcon("👍");
            fireEvent.click(likeButton);
            await new Promise(process.nextTick);

            // Assert
            expect(likeCallMock).toHaveBeenCalledWith(testThreadId);
        });

        test("should display spinner while sending request", () => {
            // Arrange
            const state = createState();
            renderComponent(state);

            // Act
            const likeButton = findActionPillByIcon("👍");
            fireEvent.click(likeButton);

            // Assert
            const actionPill = findActionPillByIcon("👍");
            const spinner = findSpinner(actionPill);
            expect(spinner).toBeVisible();
        });
    });

    describe("when tread is liked,", () => {
        const responseDelay = 100;
        let unlikeCallMock;
        beforeEach(() => {
            unlikeCallMock = jest.spyOn(LikeThreadClient.prototype, 'unlike')
                .mockImplementation(() => {
                    // We need this delay for the loading spinner test.
                    // Otherwise, the loading spinner will never show.
                    return new Promise((resolve) => setTimeout(resolve, 100))
                });
        });

        afterEach(() => {
            unlikeCallMock.mockRestore();
        });

        test("should change action pill to unliked color", async () => {
            // Arrange
            const state = createState({threadLiked: true});
            renderComponent(state);

            // Act
            const likeButton = findActionPillByIcon("👍");
            fireEvent.click(likeButton);
            await new Promise((resolve) => setTimeout(resolve, responseDelay));

            // Assert
            expect(findActionPillByIcon("👍")).toHaveStyle(`background-color: ${testTheme.primaryColor};`);
        });

        test("should call backend unlike endpoint", async () => {
            // Arrange
            const state = createState({threadLiked: true});
            renderComponent(state);

            // Act
            const likeButton = findActionPillByIcon("👍");
            fireEvent.click(likeButton);
            await new Promise(process.nextTick);

            // Assert
            expect(unlikeCallMock).toHaveBeenCalledWith(testThreadId);
        });

        test("should display spinner while sending request", () => {
            // Arrange
            const state = createState();
            renderComponent(state);

            // Act
            const likeButton = findActionPillByIcon("👍");
            fireEvent.click(likeButton);

            // Assert
            const actionPill = findActionPillByIcon("👍");
            const spinner = findSpinner(actionPill);
            expect(spinner).toBeVisible();
        });
    });
});

describe("comment action pill,", () => {
    test("should display count of comments in thread", () => {
        // Arrange
        const testNumberOfComments = "36";
        const state = createState({numberOfComments: testNumberOfComments})
        renderComponent(state);

        // Assert
        const actionPillNumber = findActionPillNumberByIcon("💬");
        expect(actionPillNumber).toEqual(testNumberOfComments);
    });

    test("when clicked while comments are toggled off, should get and display comments from backend", async () => {
        // Arrange
        const newCommentContent1 = "new test comment 1";
        const newCommentContent2 = "new test comment 2";
        const newComments =
            {
                comments: [
                    {
                        id: 1,
                        content: newCommentContent1
                    },
                    {
                        id: 2,
                        content: newCommentContent2
                    }
                ]
            };
        jest.spyOn(CommentsClient.prototype, 'comments')
            .mockImplementation(() => Promise.resolve(newComments));

        const state = createState({commentsToggledOn: false});
        renderComponent(state);

        // Act
        const commentPill = findActionPillByIcon("💬");
        fireEvent.click(commentPill);
        await new Promise(process.nextTick);

        // Arrange
        expect(screen.getByText(newCommentContent1)).toBeVisible();
        expect(screen.getByText(newCommentContent2)).toBeVisible();
    });

    test("when clicked while comments toggled on, should toggle the comments component off", () => {
        // Arrange
        const state = createState({commentsToggledOn: true});
        renderComponent(state);

        // Act
        const commentPill = findActionPillByIcon("💬");
        fireEvent.click(commentPill);

        // Arrange
        expect(screen.queryByText(testCommentContent)).toBeNull();
    });
});